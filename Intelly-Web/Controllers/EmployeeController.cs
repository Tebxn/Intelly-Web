using Intelly_Web.Entities;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserModel _userModel;


        //Inyeccion de dependencias, para que por medio de la interfaz el controller vea los metodos
        public EmployeeController(ILogger<UserController> logger, IUserModel userModel)
        {
            _logger = logger;
            _userModel = userModel;
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Employees()
        {
            try
            {
                var data = await _userModel.GetAllUsers();
                return View("Employees", data);
            }
            catch (Exception ex)
            {
                List<UserEnt> errors = new List<UserEnt>();
                return View(errors);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
