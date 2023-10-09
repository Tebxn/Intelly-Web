using Intelly_Web.Entities;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class AccessController : Controller
    {
        private readonly ILogger<AccessController> _logger;
        private readonly IUserModel _userModel;

        public AccessController(ILogger<AccessController> logger, IUserModel userModel)
        {
            _logger = logger;
            _userModel = userModel;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserEnt entity)
        {
            var resp = _userModel.Login(entity);

            if (resp != null)
                return RedirectToAction("NewSession", "Login");
            else
            {
                ViewBag.MensajePantalla = "No se pudo iniciar sesión";
                return View();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
