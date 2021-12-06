

namespace Corporation.Dal.Data;

public static class SeedTestData
{
    public async static void SeedTestDataToDatabase(IServiceProvider serviceProvider)
    {
        serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        using (var context = new Plant1Context(serviceProvider.GetRequiredService<DbContextOptions<Plant1Context>>()))
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Plant1Context>>();
            if (context == null || context.ProductTypes == null)
            {
                logger.LogError("Null Plant1Context");
                throw new ArgumentNullException("Null Plant1Context");
            }
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Applying migrations: {string.Join(",", pendingMigrations)}");
                await context.Database.MigrateAsync();
            }
            if (context.ProductTypes.Any())
            {
                logger.LogInformation("Database contains data - database init with test data is not required");
                return;
            }

            logger.LogInformation("Begin writing test data to database plant1 ...");

            var pt1 = new ProductType { Name = "Кирпич полуторный 250x120x65", Number = 1, Units = 360, Volume = 1.1, Weight = 500.0, Price = 5000.0M };
            var pt2 = new ProductType { Name = "Кирпич пустотелый 250x120x65", Number = 2, Units = 360, Volume = 1.1, Weight = 550.0, Price = 6000.0M };
            var pt3 = new ProductType { Name = "Кирпич полнотелый 250x120x88", Number = 3, Units = 280, Volume = 1.2, Weight = 490.0, Price = 5500.0M };
            var pt4 = new ProductType { Name = "Кирпич двойной 250x120x138", Number = 4, Units = 180, Volume = 1.3, Weight = 520.0, Price = 7000.0M };
            var pt5 = new ProductType { Name = "Кирпич двойной пустотелый 250x120x138", Number = 5, Units = 180, Volume = 1.05, Weight = 530.0, Price = 6500.0M };
            var pt6 = new ProductType { Name = "Кирпич евро пустотелый 250x120x120", Number = 6, Units = 260, Volume = 1.0, Weight = 540.0, Price = 6000.0M };
            var pt7 = new ProductType { Name = "Кирпич евро 250x120x120", Number = 7, Units = 260, Volume = 1.15, Weight = 520.0, Price = 5000.0M };
            var pt8 = new ProductType { Name = "Кирпич евро полнотелый 250x120x65", Number = 8, Units = 380, Volume = 1.2, Weight = 510.0, Price = 6500.0M };
            context.ProductTypes.AddRange(pt1, pt2, pt3, pt4, pt5, pt6, pt7, pt8);
            await context.SaveChangesAsync();

            logger.LogInformation("Complete writing test data to database plant1 ...");
        }

    }
}

