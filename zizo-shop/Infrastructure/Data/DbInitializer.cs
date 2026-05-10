using Microsoft.AspNetCore.Identity;
using zizo_shop.Domain.Entities;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Infrastructure.Data
{
    public class DbInitializer
    {
        public static async Task SeedRolesAsync( IServiceProvider serviceProvider) {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
            var adminEmail = "admin@zizoshop.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNmOWU0NmMxLThhMmMtNDZiYy04ZWQ4LTg0Mzg3YWViYjhjZCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluQHppem9zaG9wLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzc4OTEzNzE2LCJpc3MiOiJ6aXpvLXNob3AtYXBpIiwiYXVkIjoieml6by1zaG9wLXVzZXJzLWNsaWVudCJ9.tVeKt8mUKi70fytgqyRqmtOgclstQ9UJUssrAJduj84
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Id= Guid.NewGuid(),
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                };
                adminUser.Cart = new Cart(adminUser.Id);

                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
