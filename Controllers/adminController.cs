using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using KargoSistemi.Models;
using KargoSistemi.Data; 
using System;
using System.Linq; 

namespace KargoSistemi.Controllers
{
    [Authorize] 
    public class AdminController : Controller
    {
        private readonly UygulamaDbContext _context; 

        public AdminController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string kullaniciAdi = "Bilinmiyor";
            if (User.Identity != null && User.Identity.Name != null)
            {
                kullaniciAdi = User.Identity.Name;
            }

            string gorunurSubeAdi = "";

            
            if (kullaniciAdi.Contains("_mudur"))
            {
                string sehir = kullaniciAdi.Replace("_mudur", "");
                
                gorunurSubeAdi = sehir.ToUpper(new CultureInfo("tr-TR")) + " ŞUBESİ";
            }
            else
            {
                gorunurSubeAdi = "GENEL MERKEZ";
            }

            
            ViewBag.AdminAdi = kullaniciAdi;
            ViewBag.SubeAdi = gorunurSubeAdi;

            return View();
        }

        [HttpGet]
        public IActionResult PersonelEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PersonelEkle(string kullaniciAdi, string sifre, string rol)
        {
            
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre) || string.IsNullOrEmpty(rol))
            {
                ViewBag.Hata = "Lütfen tüm zorunlu alanları doldurun.";
                return View();
            }

            
            Personel? mevcutPersonel = _context.Personeller.FirstOrDefault(p => p.KullaniciAdi == kullaniciAdi);
            
            if (mevcutPersonel != null)
            {
                ViewBag.Hata = "Bu kullanıcı adı sisteme zaten kayıtlı.";
                return View();
            }

            
            string hashlenmisSifre = BCrypt.Net.BCrypt.HashPassword(sifre);

            
            Personel yeniPersonel = new Personel();
            yeniPersonel.KullaniciAdi = kullaniciAdi;
            yeniPersonel.SifreHash = hashlenmisSifre; 
            yeniPersonel.Rol = rol;

            
            _context.Personeller.Add(yeniPersonel);
            _context.SaveChanges();
                
            
            return RedirectToAction("Index", "Admin"); 
        }
    }
}