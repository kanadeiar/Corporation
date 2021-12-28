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
            
            var corp1 = new Corp { Name = "Корпорация" };
            context.Corps.AddRange( corp1 );
            await context.SaveChangesAsync();

            var com1 = new Company { Name = "Завод 1", Corp = corp1 };
            context.Companies.AddRange( com1 );
            await context.SaveChangesAsync();

            var depC1W1 = new Department { Name = "Склад сырья 1", Company = com1 };
            var depC1P1 = new Department { Name = "Цех производства 1", Company = com1 };
            var depC1W2 = new Department { Name = "Склад продукции 1", Company = com1 };
            var depC1L1 = new Department { Name = "Лаборатория 1", Company = com1 };
            var depC1O1 = new Department { Name = "Офис 1", Company = com1 };
            context.Departments.AddRange( depC1W1, depC1P1, depC1W2 );
            await context.SaveChangesAsync();

            var workMasterC1 = new Workstation { Name = "Рабочее место мастера 1", Department = depC1P1 };
            var workC1W1 = new Workstation { Name = "Управление складом сырья 1", Department = depC1W1 };
            var workC1P1 = new Workstation { Name = "Пресс кирпича 1", Department = depC1P1 };
            var workC1P2 = new Workstation { Name = "Упаковка кирпича 1", Department = depC1P1 };
            var workC1W2 = new Workstation { Name = "Управление складом продукции 1", Department = depC1W2 };
            var workC1L1 = new Workstation { Name = "Рабочее место лаборанта 1", Department = depC1L1 };
            var workC1O1 = new Workstation { Name = "Рабочее место офисного рабоника 1", Department = depC1O1 };
            context.Workstations.AddRange(workMasterC1, workC1W1, workC1P1, workC1P2, workC1W2, workC1L1, workC1O1 );
            await context.SaveChangesAsync();

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

            #region Identity

            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<Role> roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            string adminUsername = configuration["Data:AdminUser:Name"] ?? "admin";
            string adminEmail = configuration["Data:AdminUser:Email"] ?? "admin@example.com";
            string adminPassword = configuration["Data:AdminUser:Password"] ?? "secret";
            string adminRole = configuration["Data:AdminUser:Role"] ?? "admins";

            if (await userManager.FindByNameAsync(adminUsername) is null)
            {
                if (await roleManager.FindByNameAsync(adminRole) is null)
                {
                    await roleManager.CreateAsync(new Role { Name = adminRole, RoleName = "Администраторы" });
                    await roleManager.CreateAsync(new Role { Name = Inits.UserRole, RoleName = "Пользователи" });
                    await roleManager.CreateAsync(new Role { Name = Inits.MasterWorkC1P1, RoleName = "Мастера производства 1" });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1W1, RoleName = "Кладовщики склада сырья 1" });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1P1, RoleName = "Операторы кирпичного пресса 1" });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1P2, RoleName = "Операторы кирпичной упаковки 1" });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1W2, RoleName = "Кладовщики склада продукции 1" });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1L1, RoleName = "Лаборанты лаборатории 1" });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1O1, RoleName = "Офисные работники офиса 1" });
                }
                var adminUser = new User
                {
                    SurName = "Админов",
                    FirstName = "Админ",
                    Patronymic = "Админович",
                    Birthday = DateTime.Today.AddYears(-18),
                    UserName = adminUsername,
                    Email = adminEmail,
                    Department = "Отдел 1",
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                    logger.LogInformation("Пользователь {1} успешно создан и наделен ролью {2}", adminUser.UserName, adminRole);
                    await userManager.AddToRoleAsync(adminUser, Inits.UserRole);
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
                    SurName = "Иванов",
                    FirstName = "Иван",
                    Patronymic = "Иванович",
                    Birthday = DateTime.Today.AddYears(-20),
                    UserName = "user",
                    Email = "user@example.com",
                    Department = "Отдел 1",
                };
                result = await userManager.CreateAsync(user, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Inits.UserRole);
                }

                var masterC1P1 = new User
                {
                    SurName = "Мастеров",
                    FirstName = "Мастер",
                    Patronymic = "Мастерович",
                    Birthday = DateTime.Today.AddYears(-30),
                    UserName = "masterov",
                    Email = "masterov@example.com",
                    Department = "Рабочее место мастера 1",
                };
                result = await userManager.CreateAsync(masterC1P1, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(masterC1P1, Inits.MasterWorkC1P1);
                }
                var operatorC1W1 = new User
                {
                    SurName = "Логинов",
                    FirstName = "Логин",
                    Patronymic = "Логинович",
                    Birthday = DateTime.Today.AddYears(-22),
                    UserName = "loginov",
                    Email = "loginov@example.com",
                    Department = "Управление складом сырья 1",
                };
                result = await userManager.CreateAsync(operatorC1W1, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1W1, Inits.OperatorWorkC1W1);
                }
                var operatorC1P1 = new User
                {
                    SurName = "Петров",
                    FirstName = "Петр",
                    Patronymic = "Петрович",
                    Birthday = DateTime.Today.AddYears(-23),
                    UserName = "petrov",
                    Email = "petrov@example.com",
                    Department = "Пресс кирпича 1",
                };
                result = await userManager.CreateAsync(operatorC1P1, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1P1, Inits.OperatorWorkC1P1);
                }
                var operatorC1P2 = new User
                {
                    SurName = "Васин",
                    FirstName = "Вася",
                    Patronymic = "Васинович",
                    Birthday = DateTime.Today.AddYears(-24),
                    UserName = "vasin",
                    Email = "vasin@example.com",
                    Department = "Упаковка кирпича 1",
                };
                result = await userManager.CreateAsync(operatorC1P2, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1P2, Inits.OperatorWorkC1P1);
                }
                var operatorC1W2 = new User
                {
                    SurName = "Сидоров",
                    FirstName = "Сидор",
                    Patronymic = "Сидорович",
                    Birthday = DateTime.Today.AddYears(-25),
                    UserName = "sidorov",
                    Email = "sidorov@example.com",
                    Department = "Управление складом продукции 1",
                };
                result = await userManager.CreateAsync(operatorC1W2, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1W2, Inits.OperatorWorkC1P1);
                }
                var operatorC1L1 = new User
                {
                    SurName = "Попов",
                    FirstName = "Поп",
                    Patronymic = "Попович",
                    Birthday = DateTime.Today.AddYears(-17),
                    UserName = "popov",
                    Email = "popov@example.com",
                    Department = "Рабочее место лаборанта 1",
                };
                result = await userManager.CreateAsync(operatorC1L1, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1L1, Inits.OperatorWorkC1P1);
                }
                var operatorC1O1 = new User
                {
                    SurName = "Алексеев",
                    FirstName = "Алексей",
                    Patronymic = "Алексеевич",
                    Birthday = DateTime.Today.AddYears(-18),
                    UserName = "alexeev",
                    Email = "alexeev@example.com",
                    Department = "Рабочее место офисного рабоника 1",
                };
                result = await userManager.CreateAsync(operatorC1O1, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1O1, Inits.OperatorWorkC1P1);
                }

            }

            #endregion

            logger.LogInformation("Complete writing test data to database CorporationContext ...");
        }
    }
}

