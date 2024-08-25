using AnyProduct.Orders.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AnyProduct.Orders.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void EnsureMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<OrderContext>();

                if (context.Database.IsNpgsql())
                {
                    context.Database.Migrate();
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
