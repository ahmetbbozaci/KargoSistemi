using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace KargoSistemi.Controllers
{
    [Authorize] 
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // 1. Giriş yapan personelin kullanıcı adını alıyoruz (Örn: ankara_mudur)
            string kullaniciAdi = User.Identity?.Name ?? "Bilinmiyor";

            // 2. Kullanıcı ismine göre şube adını belirleyen mantık
            string gorunurSubeAdi;

            if (kullaniciAdi.Contains("_mudur"))
            {
                // Kullanıcı adındaki "_mudur" kısmını atıp, geri kalanı büyük harfe çeviriyoruz.
                // Örn: "izmir_mudur" -> "İZMİR ŞUBESİ"
                string sehir = kullaniciAdi.Replace("_mudur", "");
                gorunurSubeAdi = sehir.ToUpper(new CultureInfo("tr-TR")) + " ŞUBESİ";
            }
            else
            {
                gorunurSubeAdi = "GENEL MERKEZ";
            }

            // 3. Bilgileri View'a (Arayüze) gönderiyoruz
            ViewBag.AdminAdi = kullaniciAdi;
            ViewBag.SubeAdi = gorunurSubeAdi;

            return View();
        }
    }
}