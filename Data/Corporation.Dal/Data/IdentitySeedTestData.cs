



namespace Corporation.Dal.Data;

public static class IdentitySeedTestData
{
    public async static void SeedDatabaseTestData(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        using (var context = new IdentityContext(serviceProvider.GetRequiredService<DbContextOptions<IdentityContext>>()))
        {
            if (context == null)
            {
                throw new ArgumentNullException("Null DataContext");
            }
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
        }

        UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string username = configuration["Data:AdminUser:Name"] ?? "admin";
        string email = configuration["Data:AdminUser:Email"] ?? "admin@example.com";
        string password = configuration["Data:AdminUser:Password"] ?? "secret";
        string role = configuration["Data:AdminUser:Role"] ?? "admins";

        if (await userManager.FindByNameAsync(username) is null)
        {
            if (await roleManager.FindByNameAsync(role) is null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = new IdentityUser
            {
                UserName = username,
                Email = email,
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
