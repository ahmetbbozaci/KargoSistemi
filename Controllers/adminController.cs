using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using KargoSistemi.Models;
using KargoSistemi.Data; 
using System;
using System.Linq; 

namespace KargoSistemi.Controllers
{
    // ====================================================================
    // YENİ EKLENEN KISIM: Sadece yaka kartında "Mudur" yazanlar girebilir!
    // ====================================================================
    [Authorize(Roles = "Mudur")] 
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

            // Eskiden burada kullanıcı adının sonundaki "_mudur" kelimesini kesip şube adı bulmaya 
            // çalışıyorduk. Artık veritabanında gerçek bir Şube (Branches) tablomuz olduğu için 
            // o ilkel (hardcoded) yapıyı kaldırıp şimdilik sadece "Şube Yöneticisi" yazdırıyoruz.
            // İlerleyen günlerde, yöneticinin ID'sine bakıp veritabanından hangi şubenin müdürü 
            // olduğunu bulan SQL sorgusunu buraya yazacağız.
            string gorunurSubeAdi = "Şube Yöneticisi";

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