using FluentValidation;
using Hangfire;
using Hangfire.Dashboard;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using zizo_shop.API.Services;
using zizo_shop.Application.Common.Behaviors;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Auth.Commands;
using zizo_shop.Infrastructure.Data;
using zizo_shop.Infrastructure.Identity;
using zizo_shop.Infrastructure.Jobs;
using zizo_shop.Infrastructure.Middlewares;
using zizo_shop.Infrastructure.Services;

namespace zizo_shop.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── Controllers & Swagger ─────────────────────────────────────────
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // ── FluentValidation + MediatR pipeline ───────────────────────────
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddValidatorsFromAssembly(typeof(RegisterCommand).Assembly);
            builder.Services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));

            // ── Database ──────────────────────────────────────────────────────
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            // ── Identity ──────────────────────────────────────────────────────
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                    options.Lockout.AllowedForNewUsers = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // ── JWT Authentication ────────────────────────────────────────────
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name
                };
            });

            builder.Services.AddAuthorization();

            // ── Services ──────────────────────────────────────────────────────
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // ── MediatR ───────────────────────────────────────────────────────
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly));

            // ── Swagger ───────────────────────────────────────────────────────
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Zizo Shop API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // ── CORS ──────────────────────────────────────────────────────────
            builder.Services.AddCors(options =>
                options.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod()));

            // ── Hangfire ──────────────────────────────────────────────────────
            builder.Services.AddHangfire(config =>
                config.UseSqlServerStorage(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHangfireServer();
            builder.Services.AddScoped<CleanupJobs>();

            // ── Health checks ─────────────────────────────────────────────────
            //builder.Services.AddHealthChecks()
            //    .AddDbContextCheck<ApplicationDbContext>();

            // ─────────────────────────────────────────────────────────────────
            var app = builder.Build();
            // ─────────────────────────────────────────────────────────────────

            // ── Ensure wwwroot exists for file uploads ─────────────────────────
            var wwwroot = Path.Combine(
                builder.Environment.ContentRootPath, "wwwroot");
            if (!Directory.Exists(wwwroot))
                Directory.CreateDirectory(wwwroot);

            // ── Exception middleware (first so it wraps everything) ────────────
            app.UseMiddleware<ExceptionMiddleware>();

            // ── HSTS (production only) ────────────────────────────────────────
            if (!app.Environment.IsDevelopment())
                app.UseHsts();

            // ── Swagger (development only) ────────────────────────────────────
           
                app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zizo Shop API v1");
                c.RoutePrefix = string.Empty;
            });



            // ── Middleware order ──────────────────────────────────────────────
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // ── Hangfire dashboard (Admin only) ───────────────────────────────
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new IDashboardAuthorizationFilter[]
                {
                    new HangfireAuthFilter()
                }
            });

            // ── Recurring jobs ────────────────────────────────────────────────
            RecurringJob.AddOrUpdate<CleanupJobs>(
                "cleanup-empty-carts",
                job => job.RemoveEmptyCarts(),
                Cron.Daily);

            RecurringJob.AddOrUpdate<CleanupJobs>(
                "expire-old-coupons",
                job => job.ExpireOldCoupon(),
                Cron.Hourly);

            RecurringJob.AddOrUpdate<CleanupJobs>(
                "revoke-expired-refresh-tokens",
                job => job.RevokeExpiredRefreshTokens(),
                Cron.Daily);

            RecurringJob.AddOrUpdate<CleanupJobs>(
                "cancel-abandoned-orders",
                job => job.CancelAbandonedPendingOrders(),
                Cron.Daily);

            // ── Health check endpoint ─────────────────────────────────────────
            //app.MapHealthChecks("/health");

            // ── Database seeding ──────────────────────────────────────────────
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    DbInitializer
                        .SeedRolesAsync(scope.ServiceProvider)
                        .GetAwaiter()
                        .GetResult();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider
                        .GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the database.");
                }
            }

            app.MapControllers();
            app.Run();
        }
    }
}