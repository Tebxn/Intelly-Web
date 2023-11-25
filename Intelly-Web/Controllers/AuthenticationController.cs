using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IUserModel _userModel;

        public AuthenticationController(ILogger<AuthenticationController> logger, IUserModel userModel)
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

        [HttpGet]
        public IActionResult EmailSent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserEnt entity)
        {
          
            var resp = await _userModel.Login(entity);


            if (resp.Success)
            {
                HttpContext.Session.SetString("UserId", resp.Data.User_Id.ToString());
                HttpContext.Session.SetString("UserName", resp.Data.User_Name);
                HttpContext.Session.SetString("UserName", resp.Data.User_LastName);
                HttpContext.Session.SetString("UserType", resp.Data.User_Type.ToString());
                HttpContext.Session.SetString("UserCompanyId", resp.Data.User_Company_Id.ToString());
                HttpContext.Session.SetString("UserToken", resp.Data.UserToken);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.MensajePantalla = resp.ErrorMessage;
                return View();
            }
        }

        [HttpGet]
        public IActionResult EndSession()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Authentication", "Login");
        }

        //[HttpPost]
        //public async Task<IActionResult> PwdRecovery(UserEnt entity)
        //{
        //    var resp = await _userModel.PwdRecovery(entity);

        //    if (resp.Success)
        //        return RedirectToAction("EmailSent", "Authentication");
        //    else
        //    {
        //        ViewBag.MensajePantalla = resp.ErrorMessage;
        //        return View();
        //    }
        //}


        public async Task<IActionResult> UpdateUserPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserPassword(UserEnt entity)
        {
            var resp = _userModel.UpdateUserPassword(entity);
            if (resp.IsCompletedSuccessfully)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.MensajePantalla = "No se realizaron cambios";
                return View();
            }
        }

       
		[HttpGet]
		public async Task<IActionResult> ChangePassword(string id)
		{
			UserEnt entity = new UserEnt();
			entity.User_Secure_Id = id;
			return View(entity);
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(UserEnt entity)
		{
			var apiResponse = await _userModel.ChangePassword(entity);

			if (apiResponse.Success)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				ViewBag.MensajePantalla = "No se realizaron cambios";
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
