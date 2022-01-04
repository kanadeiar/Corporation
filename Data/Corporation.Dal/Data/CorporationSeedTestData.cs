using Corporation.Domain.Company1;

namespace Corporation.Dal.Data;

public static class CorporationSeedTestData
{
    public async static void SeedTestDataToDatabase(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        using (var context = new CorporationContext(serviceProvider.GetRequiredService<DbContextOptions<CorporationContext>>()))
        {
            var logger = serviceProvider.GetRequiredService<ILogger<CorporationContext>>();
            if (context == null || context.Com1ProductTypes == null)
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
            if (context.Com1ProductTypes.Any())
            {
                logger.LogInformation("Database contains data - database init with test data is not required");
                return;
            }

            logger.LogInformation("Begin writing test data to database CorporationContext ...");

            var corp1 = new Corp { Name = Inits.CopropationName };
            context.Corps.AddRange(corp1);
            await context.SaveChangesAsync();

            var com0 = new Company { Name = Inits.Company0Name, Corp = corp1 };
            var com1 = new Company { Name = Inits.Company1Name, Corp = corp1 };
            context.Companies.AddRange(com0, com1);
            await context.SaveChangesAsync();

            var depC0O1 = new Department { Name = "Главный офис 1", Company = com0 };
            var depC1W1 = new Department { Name = "Склад сырья 1", Company = com1 };
            var depC1P1 = new Department { Name = "Цех производства 1", Company = com1 };
            var depC1W2 = new Department { Name = "Склад продукции 1", Company = com1 };
            var depC1L1 = new Department { Name = "Лаборатория 1", Company = com1 };
            var depC1O1 = new Department { Name = "Офис 1", Company = com1 };
            context.Departments.AddRange(depC0O1, depC1W1, depC1P1, depC1W2, depC1L1, depC1O1);
            await context.SaveChangesAsync();

            var workMasterC1 = new Workstation { Name = "Рабочее место мастера 1", Department = depC1P1 };
            var workC1W1 = new Workstation { Name = "Управление складом сырья 1", Department = depC1W1 };
            var workC1P1 = new Workstation { Name = "Пресс кирпича 1", Department = depC1P1 };
            var workC1A1 = new Workstation { Name = "Автоклавирование кирпича 1", Department = depC1P1 };
            var workC1P2 = new Workstation { Name = "Упаковка кирпича 1", Department = depC1P1 };
            var workC1W2 = new Workstation { Name = "Управление складом продукции 1", Department = depC1W2 };
            var workC1L1 = new Workstation { Name = "Рабочее место лаборанта 1", Department = depC1L1 };
            var workC1O1 = new Workstation { Name = "Рабочее место офисного рабоника 1", Department = depC1O1 };
            context.Workstations.AddRange(workMasterC1, workC1W1, workC1P1, workC1A1, workC1P2, workC1W2, workC1L1, workC1O1);
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
                    await roleManager.CreateAsync(new Role { Name = adminRole, RoleName = "Администраторы", DepartmentId = depC0O1.Id });
                    await roleManager.CreateAsync(new Role { Name = Inits.UserRole, RoleName = "Пользователи", DepartmentId = depC0O1.Id });
                    await roleManager.CreateAsync(new Role { Name = Inits.MasterWorkC1P1, RoleName = "Мастера производства 1", DepartmentId = depC1P1.Id });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1W1, RoleName = "Кладовщики склада сырья 1", DepartmentId = depC1W1.Id });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1P1, RoleName = "Операторы кирпичного пресса 1", DepartmentId = depC1P1.Id });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1A1, RoleName = "Автоклавщики автоклавов 1", DepartmentId = depC1P1.Id });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1P2, RoleName = "Операторы кирпичной упаковки 1", DepartmentId = depC1P1.Id });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1W2, RoleName = "Кладовщики склада продукции 1", DepartmentId = depC1W1.Id });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1L1, RoleName = "Лаборанты лаборатории 1", DepartmentId = depC1L1.Id });
                    await roleManager.CreateAsync(new Role { Name = Inits.OperatorWorkC1O1, RoleName = "Офисные работники офиса 1", DepartmentId = depC1O1.Id });
                }
                var adminUser = new User
                {
                    SurName = "Админов",
                    FirstName = "Админ",
                    Patronymic = "Админович",
                    Birthday = DateTime.Today.AddYears(-18),
                    UserName = adminUsername,
                    Email = adminEmail,
                    CompanyId = com0.Id,
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
                    CompanyId = com1.Id,
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
                    CompanyId = com1.Id,
                };
                result = await userManager.CreateAsync(masterC1P1, "123456");
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
                    CompanyId = com1.Id,
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
                    CompanyId = com1.Id,
                };
                result = await userManager.CreateAsync(operatorC1P1, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1P1, Inits.OperatorWorkC1P1);
                }
                var operatorC1A1 = new User
                {
                    SurName = "Агафонов",
                    FirstName = "Агафон",
                    Patronymic = "Агафонович",
                    Birthday = DateTime.Today.AddYears(-24),
                    UserName = "agafonov",
                    Email = "agafonov@example.com",
                    CompanyId = com1.Id,
                };
                result = await userManager.CreateAsync(operatorC1A1, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1A1, Inits.OperatorWorkC1A1);
                }
                var operatorC1P2 = new User
                {
                    SurName = "Васин",
                    FirstName = "Вася",
                    Patronymic = "Васинович",
                    Birthday = DateTime.Today.AddYears(-24),
                    UserName = "vasin",
                    Email = "vasin@example.com",
                    CompanyId = com1.Id,
                };
                result = await userManager.CreateAsync(operatorC1P2, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1P2, Inits.OperatorWorkC1P2);
                }
                var operatorC1W2 = new User
                {
                    SurName = "Сидоров",
                    FirstName = "Сидор",
                    Patronymic = "Сидорович",
                    Birthday = DateTime.Today.AddYears(-25),
                    UserName = "sidorov",
                    Email = "sidorov@example.com",
                    CompanyId = com1.Id,
                };
                result = await userManager.CreateAsync(operatorC1W2, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1W2, Inits.OperatorWorkC1W2);
                }
                var operatorC1L1 = new User
                {
                    SurName = "Попов",
                    FirstName = "Поп",
                    Patronymic = "Попович",
                    Birthday = DateTime.Today.AddYears(-17),
                    UserName = "popov",
                    Email = "popov@example.com",
                    CompanyId = com1.Id,
                };
                result = await userManager.CreateAsync(operatorC1L1, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1L1, Inits.OperatorWorkC1L1);
                }
                var operatorC1O1 = new User
                {
                    SurName = "Алексеев",
                    FirstName = "Алексей",
                    Patronymic = "Алексеевич",
                    Birthday = DateTime.Today.AddYears(-18),
                    UserName = "alexeev",
                    Email = "alexeev@example.com",
                    CompanyId = com1.Id,
                };
                result = await userManager.CreateAsync(operatorC1O1, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(operatorC1O1, Inits.OperatorWorkC1O1);
                }

            }

            #endregion

            #region Factory 1

            var lr1 = new Com1LooseRaw { Name = "Песок", Price = 100.0M };
            var lr2 = new Com1LooseRaw { Name = "Известь", Price = 1000.0M };
            context.Com1LooseRaws.AddRange(lr1, lr2);
            await context.SaveChangesAsync();

            var pt1 = new Com1ProductType { Name = "Кирпич полуторный 250x120x65", Number = 1, Units = 360, Com1Loose1Raw = lr1, Com1Loose1RawValue = 100.0, Com1Loose2Raw = lr2, Com1Loose2RawValue = 120.0 };
            var pt2 = new Com1ProductType { Name = "Кирпич двойной 250x120x138", Number = 2, Units = 240, Com1Loose1Raw = lr1, Com1Loose1RawValue = 140.0, Com1Loose2Raw = lr2, Com1Loose2RawValue = 160.0 };
            var pt3 = new Com1ProductType { Name = "Кирпич евро пустотелый 250x120x120", Number = 3, Units = 180, Com1Loose1Raw = lr1, Com1Loose1RawValue = 180.0, Com1Loose2Raw = lr2, Com1Loose2RawValue = 170.0 };
            var pt4 = new Com1ProductType { Name = "Кирпич полнотелый 250x120x88", Number = 4, Units = 300, Com1Loose1Raw = lr1, Com1Loose1RawValue = 120.0, Com1Loose2Raw = lr2, Com1Loose2RawValue = 130.0 };
            context.Com1ProductTypes.AddRange( pt1, pt2, pt3, pt4 );
            await context.SaveChangesAsync();

            var shA = new Com1Shift { Name = "Смена А" };
            var shB = new Com1Shift { Name = "Смена Б" };
            var shC = new Com1Shift { Name = "Смена В" };
            var shD = new Com1Shift { Name = "Смена Г" };
            context.Com1Shifts.AddRange( shA, shB, shC, shD );
            await context.SaveChangesAsync();

            var w1sd1 = new Com1Warehouse1ShiftData
            {
                Time = DateTime.Today.AddHours(-16),
                Com1Shift = shB,
                UserId = (await userManager.FindByNameAsync("loginov")).Id,
                Com1Tank1LooseRaw = lr1,
                Com1Tank1LooseRawValue = 220.0,
                Com1Tank2LooseRaw = lr2,
                Com1Tank2LooseRawValue = 320.0,
            };
            var w1sd2 = new Com1Warehouse1ShiftData
            {
                Time = DateTime.Today.AddHours(-4),
                Com1Shift = shA,
                UserId = (await userManager.FindByNameAsync("petrov")).Id,
                Com1Tank1LooseRaw = lr1,
                Com1Tank1LooseRawValue = 210.0,
                Com1Tank2LooseRaw = lr2,
                Com1Tank2LooseRawValue = 310.0,
            };
            context.Com1Warehouse1ShiftDatas.AddRange( w1sd1, w1sd2 );
            await context.SaveChangesAsync();

            var p1sd1 = new Com1Press1ShiftData
            {
                Time = DateTime.Today.AddHours(-16),
                Com1Shift = shB,
                UserId = (await userManager.FindByNameAsync("petrov")).Id,
                Com1ProductType = pt1,
                Com1ProductTypeCount = 10,
                Com1Loose1RawValue = 20.0,
                Com1Loose2RawValue = 25.0,
            };
            var p1sd2 = new Com1Press1ShiftData
            {
                Time = DateTime.Today.AddHours(-4),
                Com1Shift = shA,
                UserId = (await userManager.FindByNameAsync("loginov")).Id,
                Com1ProductType = pt1,
                Com1ProductTypeCount = 11,
                Com1Loose1RawValue = 22.0,
                Com1Loose2RawValue = 27.0,
            };
            context.Com1Press1ShiftDatas.AddRange( p1sd1, p1sd2 );
            await context.SaveChangesAsync();

            var aut1 = new Com1Autoclave
            {
                Name = "Автоклав № 1",
            };
            var aut2 = new Com1Autoclave
            {
                Name = "Автоклав № 2",
            };
            context.Com1Autoclaves.AddRange(aut1, aut2);
            await context.SaveChangesAsync();

            var a1sd1 = new Com1Autoclaves1ShiftData
            {
                Time = DateTime.Today.AddHours(-16),
                Com1Shift = shB,
                UserId = (await userManager.FindByNameAsync("agafonov")).Id,
                Com1Autoclave = aut1,
                TimeStart = DateTime.Today.AddHours(-20),
                AutoclavedTime = new TimeSpan(8,10,0),
                AutoclavedCount = 10,
            };
            var a1sd2 = new Com1Autoclaves1ShiftData
            {
                Time = DateTime.Today.AddHours(-4),
                Com1Shift = shA,
                UserId = (await userManager.FindByNameAsync("vasin")).Id,
                Com1Autoclave = aut2,
                TimeStart = DateTime.Today.AddHours(-8),
                AutoclavedTime = new TimeSpan(8, 30, 0),
                AutoclavedCount = 11,
            };
            context.Com1Autoclaves1ShiftDatas.AddRange(a1sd1, a1sd2);
            await context.SaveChangesAsync();

            var pac1 = new Com1Pack
            {
                Name = "Стандарт",
            };
            var pac2 = new Com1Pack
            {
                Name = "Эконом",
            };
            context.Com1Packs.AddRange(pac1, pac2);
            await context.SaveChangesAsync();

            var pc1sd1 = new Com1Packing1ShiftData
            {
                Time = DateTime.Today.AddHours(-16),
                Com1Shift = shB,
                UserId = (await userManager.FindByNameAsync("vasin")).Id,
                Com1Pack = pac1,
                Com1PackCount = 20,
            };
            var pc1sd2 = new Com1Packing1ShiftData
            {
                Time = DateTime.Today.AddHours(-4),
                Com1Shift = shA,
                UserId = (await userManager.FindByNameAsync("agafonov")).Id,
                Com1Pack = pac2,
                Com1PackCount = 22,
            };
            context.Com1Packing1ShiftDatas.AddRange(pc1sd1, pc1sd2);
            await context.SaveChangesAsync();

            var w2sd1 = new Com1Warehouse2ShiftData
            {
                Time = DateTime.Today.AddHours(-16),
                Com1Shift = shB,
                UserId = (await userManager.FindByNameAsync("sidorov")).Id,
                Com1Pack = pac1,
                Com1PackValue = 20,
            };
            var w2sd2 = new Com1Warehouse2ShiftData
            {
                Time = DateTime.Today.AddHours(-4),
                Com1Shift = shA,
                UserId = (await userManager.FindByNameAsync("sidorov")).Id,
                Com1Pack = pac2,
                Com1PackValue = 22,
            };
            context.Com1Warehouse2ShiftDatas.AddRange(w2sd1, w2sd2);
            await context.SaveChangesAsync();

            #endregion

            logger.LogInformation("Complete writing test data to database CorporationContext ...");
        }
    }
}

