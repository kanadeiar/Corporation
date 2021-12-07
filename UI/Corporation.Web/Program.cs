
var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices(services =>
{
    services.AddDbContext<Plant1Context>(options => options.UseSqlite( builder.Configuration.GetConnectionString("Plant1Connection") ));

    services.AddDbContext<IdentityContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("IdentityConnection")));
    services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

    services.AddScoped<IProductTypeData, DatabaseProductTypeData>();

    services.AddScoped<ProductsInfoService>();

    services.AddControllersWithViews().AddRazorRuntimeCompilation();
    services.AddRazorPages().AddRazorRuntimeCompilation();
});
builder.Services.AddServerSideBlazor();

var app = builder.Build();

SeedTestData.SeedTestDataToDatabase(app.Services);
IdentitySeedTestData.SeedDatabaseTestData(app.Services, builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseStatusCodePagesWithRedirects("~/home/error/{0}");

app.MapControllerRoute("controllers", "controllers/{controller=Home}/{action=Index}/{id?}");
app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("online/{param?}", "/Shared/_Host");

app.Run();
