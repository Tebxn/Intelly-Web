using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserModel _userModel;
        public UserController(IUserModel employeeModel)
        {
            _userModel = employeeModel;
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(UserEnt entity)
        {
            var resp = _userModel.AddEmployee(entity);
            if (resp.IsCompletedSuccessfully)
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
                var apiResponse = await _userModel.GetAllUsers();
                var listUsers = apiResponse.Data.ToList();
                return View(listUsers);
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
