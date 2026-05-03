using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KargoSistemi.Data;
using KargoSistemi.Models;
using System.Linq;

namespace KargoSistemi.Controllers
{
    [Authorize(Roles = "Mudur, Gise")]
    public class MusteriController : Controller
    {
        private readonly UygulamaDbContext _context;

        public MusteriController(UygulamaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult MusteriEkle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Güvenlik için: Formun dışarıdan manipüle edilmesini engeller
        public IActionResult MusteriEkle(Musteri model)
        {
            // 1. Model üzerindeki [Required], [RegularExpression] vb. kuralları kontrol eder
            if (ModelState.IsValid)
            {
                try
                {
                    // 2. Doğrudan Musteri tablosuna ekleme yapıyoruz
                    _context.Musteriler.Add(model);
                    _context.SaveChanges();

                    TempData["Mesaj"] = $"{model.Customer_name} {model.Customer_lastname} başarıyla sisteme kaydedildi.";
                    return RedirectToAction("MusteriEkle");
                }
                catch (System.Exception ex)
                {
                    // İç hata (Database hatası vb.) varsa yakalarız
                    var asilHata = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ViewBag.Hata = "Veritabanı Hatası: " + asilHata;
                }
            }
            else
            {
                // Eğer validasyon geçmezse (Örn: Telefon harf içeriyorsa)
                ViewBag.Hata = "Lütfen formdaki hataları düzelterek tekrar deneyin.";
            }

            // Hata varsa veya model geçersizse verilerle birlikte sayfaya geri döner
            return View(model);
        }
    }
}