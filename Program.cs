using KargoSistemi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VarsayilanBaglanti")));


builder.Services.AddAuthentication("KargoAuth")
    .AddCookie("KargoAuth", options =>
    {
        options.Cookie.Name = "MerkezKargo.Security.Cookie";
        options.Cookie.HttpOnly = true; 
        options.Cookie.SameSite = SameSiteMode.Strict; 
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
        options.LoginPath = "/Auth/Login"; 
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();