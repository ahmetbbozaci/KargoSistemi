using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KargoSistemi.Controllers
{
    public class KargoController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Sonuc(string takipNo)
        {
            if (string.IsNullOrEmpty(takipNo))
            {
                return RedirectToAction("Index");
            }

            
            ViewBag.TakipNo = takipNo.ToUpper();
            
            return View();
        }
    }
}