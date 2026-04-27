using Microsoft.AspNetCore.Mvc;
using System;

namespace KargoSistemi.Controllers
{
    public class HesaplamaController : Controller
    {
    
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Index(double kg, double en, double boy, double yukseklik, string hat)
        {
            
            double desi = (en * boy * yukseklik) / 3000.0;
            
            double ucreteEsasAgirlik = Math.Max(kg, desi);
            
           
            double hatCarpani = 1.0;
            switch (hat)
            {
                case "SehirIci": hatCarpani = 1.0; break;
                case "Kisa": hatCarpani = 1.5; break;
                case "Orta": hatCarpani = 2.0; break;
                case "Uzak": hatCarpani = 2.5; break;
            }
            
            
            double tabanFiyat = 50.0;
            double toplamTutar = tabanFiyat + (ucreteEsasAgirlik * 15 * hatCarpani);
            
           
            ViewBag.Kg = kg;
            ViewBag.Desi = Math.Round(desi, 2);
            ViewBag.EsasAgirlik = Math.Round(ucreteEsasAgirlik, 2);
            ViewBag.Tutar = Math.Round(toplamTutar, 2);
            ViewBag.SonucGoster = true; 

            return View();
        }
    }
}