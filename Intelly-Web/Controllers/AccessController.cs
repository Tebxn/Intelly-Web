using Microsoft.AspNetCore.Mvc;

namespace Intelly_Web.Controllers
{
    public class AccessController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public AccessController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult NewUser()
        {
            return View();
        }
    }
}
