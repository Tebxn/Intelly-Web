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
        public UserController(IUserModel userModel)
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
            if (resp.IsCompletedSuccessfully)
            {
                return RedirectToAction("Employees", "User");
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



        [HttpGet]
        public async Task<IActionResult> GetSpecificUser(int UserId)
        {
            try
            {
                var apiResponse = await _userModel.GetSpecificUser(UserId);
                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;
                    return RedirectToAction("EditUser", new { UserId = user.User_Id });
                }
                else
                {
                    // Maneja el caso en que no se pudo obtener el usuario
                    return View("Error"); // Muestra una vista de error
                }
            }
            catch (Exception ex)
            {
                // Maneja el caso en que se produjo una excepción
                return View("Error"); // Muestra una vista de error
            }
        }

        public IActionResult EditUser()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditUser(int userId) // Debes recibir el ID del usuario como parámetro
        {
            // Aquí obtén los detalles del usuario desde tu fuente de datos
            // Reemplaza este ejemplo con tu lógica de obtención de datos
            var user = _userModel.GetSpecificUser(userId);

            if (user != null)
            {
                return View("EditUser", user);
            }
            else
            {
                return View("UserNotFound"); // Puedes crear una vista "UserNotFound" para mostrar un mensaje de usuario no encontrado.
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserEnt user)
        {
            var apiResponse = await _userModel.EditSpecificUser(user);

            if (apiResponse.Success)
            {
                var editedUser = apiResponse.Data;
                return View("EditUser", editedUser); // Muestra la vista UserDetails con los detalles del usuario editado
            }
            else if (apiResponse.Code == 404)
            {
                // Maneja el caso en que no se pudo encontrar el usuario
                return View("UserNotFound"); // Muestra una vista de error
            }
            else
            {
                // Maneja otros errores
                return View("Error"); // Muestra una vista de error
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
