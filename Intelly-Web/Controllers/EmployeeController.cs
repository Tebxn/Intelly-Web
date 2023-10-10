using Intelly_Web.Entities;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeModel _employeeModel;

        //Inyeccion de dependencias, para que por medio de la interfaz el controller vea los metodos
        public EmployeeController(IEmployeeModel employeeModel)
        {
            _employeeModel = employeeModel;
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeEnt entity)
        {
            var resp = _employeeModel.AddEmployee(entity);
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
                return View(await _employeeModel.GetAllUsers());
            }
            catch (Exception)
            {
                List<EmployeeEnt> errors = new List<EmployeeEnt>();
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
