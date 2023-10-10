using Microsoft.AspNetCore.Mvc;

namespace AirAidans.Controllers
{
    public class ShoeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
