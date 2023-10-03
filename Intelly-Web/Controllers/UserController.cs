using Intelly_Web.Entities;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserModel _userModel;


        //Inyeccion de dependencias, para que por medio de la interfaz el controller vea los metodos
        public UserController(ILogger<UserController> logger, IUserModel userModel)
        {
            _logger = logger;
            _userModel = userModel;
        }
        

        [HttpPost]
        public IActionResult AddUser()
        {
            _userModel.AddUser();
            return View();
        }

        [HttpGet]
        public IActionResult AddUser(UserEntitie entitie)
        {
            return View();
        }

        public IActionResult ShowUsers()
        {
            _userModel.ShowUsers();
            return View();
        }

        public IActionResult EditUser() 
        
        { 
            _userModel.EditUser();
            return View();
        }

        public IActionResult DeleteUser() 
        {
            _userModel.DeleteUser();
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
