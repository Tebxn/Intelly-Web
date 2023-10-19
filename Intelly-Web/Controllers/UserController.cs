using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserModel _userModel;

        private readonly ICompanyModel _companyModel;
        public UserController(IUserModel userModel, ICompanyModel companyModel)
        {
            _userModel = userModel;
            _companyModel = companyModel;
        }

        public async Task<IActionResult> AddEmployee()
        {
            var roleDropdownData = await _userModel.GetAllUsersRoles();
            ViewBag.ListRoles = new SelectList(roleDropdownData.Data, "Id", "Name");
            var companyDropdownData = await _companyModel.GetAllCompanies();
            ViewBag.ListCompanies = new SelectList(companyDropdownData.Data, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(UserEnt entity)
        {
            var apiResponse = await _userModel.AddEmployee(entity);
            
            if (apiResponse.Success)
            {
                var roleDropdownData = await _userModel.GetAllUsersRoles();
                ViewBag.ListRoles = new SelectList(roleDropdownData.Data, "Id", "Name");
                var companyDropdownData = await _companyModel.GetAllCompanies();
                ViewBag.ListCompanies = new SelectList(companyDropdownData.Data, "Id", "Name");
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
                    if (user != null)
                    {
                        return RedirectToAction("EditSpecificUser", new { UserId = UserId });
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                else
                {
                    // Maneja el caso en que la respuesta no sea exitosa
                    return View("Error"); // Muestra una vista de error
                }
            }
            catch (Exception ex)
            {
                // Maneja el caso en que se produjo una excepción
                return View("Error"); // Muestra una vista de error
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSpecificUser(int UserId)
        {
            try
            {
                var apiResponse = await _userModel.GetSpecificUser(UserId);
                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;
                    if (user != null)
                    {
                        return View("EditSpecificUser", user);
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                else
                {
                    // Maneja el caso en que la respuesta no sea exitosa
                    return View("Error"); // Muestra una vista de error
                }
            }
            catch (Exception ex)
            {
                // Maneja el caso en que se produjo una excepción
                return View("Error"); // Muestra una vista de error
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditSpecificUser(UserEnt user)
        {
            var apiResponse = await _userModel.EditSpecificUser(user);

            if (apiResponse.Success)
            {
                var editedUser = apiResponse.Data;
                return RedirectToAction(nameof(EditSpecificUser), new { UserId = editedUser.User_Id });
            }
            else
            {
                ViewBag.MensajePantalla = apiResponse.ErrorMessage ?? "No se realizaron cambios";
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
