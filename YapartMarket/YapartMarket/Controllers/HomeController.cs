using Microsoft.AspNetCore.Mvc;

namespace YapartMarket.MainApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}