namespace Corporation.Dal.Data;

public static class CorporationSeedTestData
{
    public async static void SeedTestDataToDatabase(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        using (var context = new CorporationContext(serviceProvider.GetRequiredService<DbContextOptions<CorporationContext>>()))
        {
            var logger = serviceProvider.GetRequiredService<ILogger<CorporationContext>>();
            if (context == null || context.ProductTypes == null)
            {
                logger.LogError("Null CorporationContext");
                throw new ArgumentNullException("Null CorporationContext");
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

            logger.LogInformation("Begin writing test data to database CorporationContext ...");

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

            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminUsername = configuration["Data:AdminUser:Name"] ?? "admin";
            string adminEmail = configuration["Data:AdminUser:Email"] ?? "admin@example.com";
            string adminPassword = configuration["Data:AdminUser:Password"] ?? "secret";
            string adminRole = configuration["Data:AdminUser:Role"] ?? "admins";
            string userRole = "users";

            if (await userManager.FindByNameAsync(adminUsername) is null)
            {
                if (await roleManager.FindByNameAsync(adminRole) is null)
                {
                    await roleManager.CreateAsync(new IdentityRole(adminRole));
                    await roleManager.CreateAsync(new IdentityRole(userRole));
                }

                var adminUser = new User
                {
                    UserName = adminUsername,
                    Email = adminEmail,
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                    logger.LogInformation("Пользователь {1} успешно создан и наделен ролью {2}", adminUser.UserName, adminRole);
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description).ToArray();
                    logger.LogError("Учётная запись пользователя {0} не создана по причине: {1}", adminUser.UserName,
                        string.Join(",", errors));
                    throw new InvalidOperationException($"Ошибка при создании пользователя {adminUser.UserName}, список ошибок: {string.Join(",", errors)}");
                }

                var user = new User
                {
                    UserName = "user",
                    Email = "user@example.com",
                };
                result = await userManager.CreateAsync(user, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, userRole);
                }
            }

            logger.LogInformation("Complete writing test data to database CorporationContext ...");
        }
    }
}

