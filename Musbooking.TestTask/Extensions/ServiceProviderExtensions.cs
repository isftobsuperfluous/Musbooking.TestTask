using Microsoft.EntityFrameworkCore;
using Musbooking.TestTask.Infrastructure;

namespace Musbooking.TestTask.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (dbContext.Database.IsNpgsql())
            {
                await dbContext.Database.MigrateAsync();
            }

            await dbContext.SeedAsync();
        }
    }
}