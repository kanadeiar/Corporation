var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices(services =>
{
    services.AddControllersWithViews().AddRazorRuntimeCompilation();
});
builder.Services.AddServerSideBlazor();

var app = builder.Build();

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

app.UseStatusCodePagesWithRedirects("~/home/error/{0}");

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();
app.MapBlazorHub();

app.Run();
