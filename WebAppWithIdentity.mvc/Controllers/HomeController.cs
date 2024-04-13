using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppWithIdentity.mvc.Interfaces;
using WebAppWithIdentity.mvc.Models;

namespace WebAppWithIdentity.mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppUserRepository _appUserRepository;

        public HomeController(ILogger<HomeController> logger, IAppUserRepository appUserRepository)
        {
            _logger = logger;
            _appUserRepository = appUserRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
