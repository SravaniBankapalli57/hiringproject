using Microsoft.AspNetCore.Mvc;
using MVCHIRINGOPERATIONS.Models;
using System.Diagnostics;

namespace MVCHIRINGOPERATIONS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult add()
        {
            return View();
        }
        public IActionResult sub()
        {
            return View();
        }
        public IActionResult mul()
        {
            return View();
        }
    }
}
