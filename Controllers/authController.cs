using Microsoft.AspNetCore.Mvc;
using KargoSistemi.Data;
using KargoSistemi.Models;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
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
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            
            var personel = _context.Personeller
                .FirstOrDefault(p => p.KullaniciAdi == username && p.SifreHash == password);

            if (personel != null)
            {
              
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, personel.KullaniciAdi),
                    new Claim(ClaimTypes.Role, personel.Rol ?? "Personel"),
                    new Claim("PersonelId", personel.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, "KargoAuth");

              
                await HttpContext.SignInAsync("KargoAuth", new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            
            await HttpContext.SignOutAsync("KargoAuth");
            return RedirectToAction("Login", "Auth");
        }
    }
}