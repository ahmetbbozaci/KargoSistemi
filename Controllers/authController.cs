using Microsoft.AspNetCore.Mvc;
using KargoSistemi.Data;
using KargoSistemi.Models;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KargoSistemi.Controllers
{
    public class AuthController : Controller
    {
        private readonly UygulamaDbContext _context;

        public AuthController(UygulamaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Eğer kullanıcı zaten giriş yapmışsa rolüne göre yönlendir (Giriş sayfasını tekrar görmesin)
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectBasedOnRole();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            Personel? personel = _context.Personeller.FirstOrDefault(p => p.KullaniciAdi == username);

            if (personel != null && BCrypt.Net.BCrypt.Verify(password, personel.SifreHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, personel.KullaniciAdi),
                    new Claim(ClaimTypes.Role, personel.Rol ?? "Personel"),
                    new Claim("PersonelId", personel.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Rol bazlı yönlendirme metodunu çağırıyoruz
                return RedirectBasedOnRole();
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        // YENİ: Erişim Engellendi Sayfası
        // Program.cs'deki AccessDeniedPath buraya bakar
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        // YENİ: Yardımcı Metod - Rol bazlı yönlendirmeyi tek yerden yönetiyoruz
        private IActionResult RedirectBasedOnRole()
        {
            if (User.IsInRole("Mudur"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (User.IsInRole("Gise"))
            {
                // Değişti: Gişe personeli artık önce müşteri kaydına gidiyor
                return RedirectToAction("MusteriEkle", "Musteri");
            }
            else if (User.IsInRole("Kurye"))
            {
                return RedirectToAction("KuryeListesi", "Kargo");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}