using AirAidans.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AirAidans.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #region Main Actions 
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Browse()
        {
            return View();
        }

        public IActionResult Locker()
        {
            return View();
        }

        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        #endregion 

        #region Extra Actions 
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}