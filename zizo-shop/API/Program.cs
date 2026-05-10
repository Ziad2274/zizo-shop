using FluentValidation;
using Hangfire;
using Hangfire.Dashboard;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddValidatorsFromAssembly(typeof(RegisterCommand).Assembly);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

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
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

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

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly));

            builder.Services.AddSwaggerGen(c =>
            {
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
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddCors(options =>
                options.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            builder.Services.AddHangfire(config =>
                config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddHangfireServer();
            builder.Services.AddScoped<CleanupJobs>();
            builder.Services.AddAuthorization();

            var app = builder.Build();

            var wwwroot = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
            if (!Directory.Exists(wwwroot)) Directory.CreateDirectory(wwwroot);

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new IDashboardAuthorizationFilter[]
                {
                    new HangfireAuthFilter()
                }
            });

            RecurringJob.AddOrUpdate<CleanupJobs>(
                "cleanup-orphaned-carts",
                job => job.RemoveEmptyCarts(),
                Cron.Daily);

            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    DbInitializer.SeedRolesAsync(scope.ServiceProvider).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the database.");
                }
            }

            app.MapControllers();
            app.Run();
        }
    }
}