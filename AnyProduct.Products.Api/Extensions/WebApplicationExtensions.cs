using AnyProduct.Products.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void EnsureMigrations([NotNull] this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ProductContext>();

                if (context.Database.IsNpgsql())
                {
                    context.Database.Migrate();
                }

            }
            catch (DbException ex)
            {

                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            }
        }
    }
}
