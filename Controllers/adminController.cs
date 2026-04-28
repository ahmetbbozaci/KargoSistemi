using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using KargoSistemi.Models;
using KargoSistemi.Data; // Kendi context adın neyse onu kullan
using System;

namespace KargoSistemi.Controllers
{
    [Authorize] // Sadece giriş yapanlar görebilir
    public class AdminController : Controller
    {
        private readonly UygulamaDbContext _context; // Veritabanı context'in

        public AdminController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string kullaniciAdi = User.Identity?.Name ?? "Bilinmiyor";
            string gorunurSubeAdi;

            // Şube müdürü mü, genel merkez mi kontrolü
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

        // [PROFESYONEL PERSONEL EKLEME EKRANI - GET]
        [HttpGet]
        public IActionResult PersonelEkle()
        {
            // Bu sayfa Layout'u otomatik kullanır.
            return View();
        }

        // [PROFESYONEL PERSONEL EKLEME İŞLEMİ - POST]
        [HttpPost]
        public IActionResult PersonelEkle(string kullaniciAdi, string sifre, string rol)
        {
            // 1. Girdi Kontrolü (Tüm alanlar dolu mu?)
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre) || string.IsNullOrEmpty(rol))
            {
                ViewBag.Hata = "Lütfen tüm zorunlu alanları eksiksiz doldurun.";
                return View();
            }

            // 2. MÜHENDİSLİK KONTROLÜ (Bu kullanıcı adı zaten var mı?)
            var mevcutPersonel = _context.Personeller.FirstOrDefault(p => p.KullaniciAdi == kullaniciAdi);
            if (mevcutPersonel != null)
            {
                ViewBag.Hata = "Bu kullanıcı adı sisteme zaten kayıtlı.";
                return View();
            }

            // 3. SİBER GÜVENLİK ADIMI (Şifreyi zırhlama)
            // Kullanıcının girdiği düz şifre burada hash'leniyor.
            string hashlenmisSifre = BCrypt.Net.BCrypt.HashPassword(sifre);

            // 4. Model Oluşturma ve Veritabanına Kaydetme
            var yeniPersonel = new Personel
            {
                KullaniciAdi = kullaniciAdi,
                SifreHash = hashlenmisSifre, // Veritabanına karmaşık hali gidiyor
                Rol = rol
            };

            try 
            {
                _context.Personeller.Add(yeniPersonel);
                _context.SaveChanges();
                
                // Başarılı ise ana panele veya personel listesine dönebilirsin.
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                ViewBag.Hata = "Kayıt sırasında teknik bir hata oluştu: " + ex.Message;
                return View();
            }
        }
    }
}