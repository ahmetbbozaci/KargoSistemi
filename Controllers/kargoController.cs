using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KargoSistemi.Data; // DbContext için
using KargoSistemi.Models; // Kargo modeli için
using System;

namespace KargoSistemi.Controllers
{
    public class KargoController : Controller
    {
        private readonly UygulamaDbContext _context;

        public KargoController(UygulamaDbContext context)
        {
            _context = context;
        }

        // ... (Diğer Index ve Sonuc metodların burada kalsın)

        [Authorize(Roles = "Mudur, Gise")]
        [HttpGet]
        public IActionResult KargoEkle()
        {
            return View();
        }

        // YENİ: Formdan gelen verileri SQL'e kaydeden metod
        [Authorize(Roles = "Mudur, Gise")]
        [HttpPost]
        public IActionResult KargoEkle(int gondericiId, int aliciId, double agirlik, string boyut, string icerik)
        {
            try 
            {
                // 1. Yeni bir Kargo (Shipment) nesnesi oluştur
                // Not: Model ismin veritabanında farklıysa (örn: Kargo) ona göre düzelt
                Kargo yeniKargo = new Kargo();
                yeniKargo.GondericiId = gondericiId;
                yeniKargo.AliciId = aliciId;
                yeniKargo.Agirlik = agirlik;
                yeniKargo.Boyut = boyut;
                yeniKargo.Icerik = icerik;
                yeniKargo.Durum = "Hazırlanıyor"; // Başlangıç durumu
                yeniKargo.KayitTarihi = DateTime.Now;
                
                // Benzersiz takip no üret (Örn: TR123456)
                Random rnd = new Random();
                yeniKargo.TakipNo = "TR" + rnd.Next(100000, 999999).ToString();

                // 2. SQL'e ekle ve kaydet
                _context.Kargolar.Add(yeniKargo);
                _context.SaveChanges();

                TempData["Mesaj"] = "Kargo başarıyla kaydedildi! Takip No: " + yeniKargo.TakipNo;
                return RedirectToAction("KargoEkle");
            }
            catch (Exception ex)
            {
                ViewBag.Hata = "Kayıt sırasında bir hata oluştu: " + ex.Message;
                return View();
            }
        }

        [Authorize(Roles = "Mudur, Kurye")]
        [HttpGet]
        public IActionResult KuryeListesi()
        {
            return View();
        }
    }
}