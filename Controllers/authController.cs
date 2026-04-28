using Microsoft.AspNetCore.Mvc;
using KargoSistemi.Data;
using KargoSistemi.Models;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

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
         
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
          
            Personel? personel = _context.Personeller.FirstOrDefault(p => p.KullaniciAdi == username);

           
            if (personel != null && BCrypt.Net.BCrypt.Verify(password, personel.SifreHash))
            {
               
                List<Claim> claims = new List<Claim>();
                
                claims.Add(new Claim(ClaimTypes.Name, personel.KullaniciAdi));

                
                string rol = "Personel";
                if (personel.Rol != null)
                {
                    rol = personel.Rol;
                }
                claims.Add(new Claim(ClaimTypes.Role, rol));
                
                claims.Add(new Claim("PersonelId", personel.Id.ToString()));

                // Kimliği oluşturuyoruz
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "KargoAuth");
                ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

                
                await HttpContext.SignInAsync("KargoAuth", principal);

                
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