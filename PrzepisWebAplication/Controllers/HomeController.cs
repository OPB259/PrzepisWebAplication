using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PrzepisWebAplication.Models;
using PrzepisyWebApplication.Models;

namespace PrzepisyWebApplication.Controllers
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
            // 1. Pobranie wartoœci z HttpContext.Items["LastVisit"] (ustawianej przez LastVisitMiddleware)
            var lastVisitObj = HttpContext.Items["LastVisit"];
            DateTime? lastVisit = lastVisitObj as DateTime?;

            // 2. Jeœli lastVisit ma wartoœæ, wyœwietlamy j¹ w ViewData. W przeciwnym razie informacja o pierwszej wizycie.
            if (lastVisit.HasValue)
            {
                ViewData["LastVisit"] = lastVisit.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                ViewData["LastVisit"] = "Pierwsza wizyta? Witamy w PrzepisyWebApplication!";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }
    }
}