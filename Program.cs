using KargoSistemi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. MVC Servislerini Ekle
builder.Services.AddControllersWithViews();

// 2. Veritabanı Bağlantısını Yapılandır
builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VarsayilanBaglanti")));

// 3. Güvenlik (CSRF) Koruması
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

// 4. Kimlik Doğrulama Yapılandırması
// Manuel "KargoAuth" yerine standart Cookie şemasını kullanıyoruz
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "MerkezKargo.Security.Cookie";
        options.Cookie.HttpOnly = true; 
        options.Cookie.SameSite = SameSiteMode.Strict; 
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
        
        // Giriş yapmamış kullanıcılar için
        options.LoginPath = "/Auth/Login"; 
        
        // DÜZELTİLEN KISIM: Yetkisi yetmeyen kullanıcı artık Login'e değil, 
        // kendi oluşturduğumuz uyarı sayfasına gidecek.
        options.AccessDeniedPath = "/Auth/AccessDenied"; 
        
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true; 
    });

var app = builder.Build();

// 5. Hata Yönetimi ve Statik Dosyalar
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// 6. Rotalama ve Yetkilendirme
app.UseRouting();

// SIRALAMA KRİTİK: Önce kimlik, sonra yetki
app.UseAuthentication(); 
app.UseAuthorization();

// 7. Varsayılan Rota Tanımı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Başlangıç sayfasını Login yaptık

app.Run();