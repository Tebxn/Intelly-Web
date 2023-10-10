using Intelly_Web.Entities;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUserModel _userModel;

        //Inyeccion de dependencias, para que por medio de la interfaz el controller vea los metodos
        public EmployeeController(IUserModel userModel)
        {
            _userModel = userModel;
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(UserEnt entity)
        {
            var resp = _userModel.AddEmployee(entity);
            if (resp == "Success")
            {
                return RedirectToAction("Employees", "Employee");
            }
            else
            {
                ViewBag.MensajePantalla = "No se realizaron cambios";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Employees()
        {
            try
            {
                return View(await _userModel.GetAllUsers());
            }
            catch (Exception)
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
