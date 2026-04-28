using Microsoft.AspNetCore.Mvc;

namespace KargoSistemi.Controllers
{
    public class SubeController : Controller
    {
     
        public IActionResult Index()
        {
       
            return View();
        }
    }
}