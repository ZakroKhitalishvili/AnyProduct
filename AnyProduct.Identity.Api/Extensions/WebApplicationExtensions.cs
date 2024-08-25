
using AnyProduct.Identity.Api.Data;
using AnyProduct.Identity.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnyProduct.Identity.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void EnsureMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<IdentityContext>();

                if (context.Database.IsNpgsql())
                {
                    context.Database.Migrate();

                    var userManager = services.GetRequiredService<UserManager<User>>();

                    var admins = userManager.GetUsersInRoleAsync("Administrator").Result;

                    if (!admins.Any())
                    {

                        var admin = new User
                        {
                            UserName = "admin@admin.com",
                            FullName = "Admin",
                            Email = "admin@admin.com",
                            PersonalNumber = "",
                            PhoneNumber = ""
                        };

                        var createResult = userManager.CreateAsync(admin, "Admin123@").Result;

                        var roleResult = userManager.AddToRoleAsync(admin, "Administrator").Result;

                        if (!(createResult.Succeeded && createResult.Succeeded))
                        {
                            throw new Exception("Failed to add a default admin");
                        }

                    }


                }

            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }
        }
    }
}
