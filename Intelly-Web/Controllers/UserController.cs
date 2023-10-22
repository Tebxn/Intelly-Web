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

        [HttpGet]
        public async Task<IActionResult> AddEmployee()
        {
            try
            {
                var roleDropdownData = await _userModel.GetAllUsersRoles();
                ViewBag.ListRoles = roleDropdownData.Data.Select(role => new SelectListItem 
                { Value = role.User_Type_Id.ToString(), 
                    Text = role.User_Type_Name });
                var companyDropdownData = await _companyModel.GetAllCompanies();
                ViewBag.ListCompanies = companyDropdownData.Data.Select(company => new SelectListItem 
                { Value = company.Company_Id.ToString(), 
                    Text = company.Company_Name });
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.MensajePantalla = "Error al cargar los datos";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(UserEnt entity)
        {
            var apiResponse = await _userModel.AddEmployee(entity);

            if (apiResponse.Success)
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
                        // Obtener la lista de roles para el usuario
                        var apiRolesResponse = await _userModel.GetAllUsersRoles();

                        if (apiRolesResponse.Success)
                        {
                            var userRoles = apiRolesResponse.Data;
                            ViewBag.ComboRoles = userRoles.Select(role => new SelectListItem { Value = role.User_Type_Id.ToString(), Text = role.User_Type_Name });
                        }
                        else
                        {
                            ViewBag.MensajePantalla = apiRolesResponse.ErrorMessage;
                        }

                        // Obtener la lista de compañías
                        var companiesResponse = await _companyModel.GetAllCompanies();
                        if (companiesResponse.Success)
                        {
                            var companies = companiesResponse.Data;
                            ViewBag.ComboCompanies = companies.Select(company => new SelectListItem { Value = company.Company_Id.ToString(), Text = company.Company_Name});
                        }
                        else
                        {
                            ViewBag.MensajePantalla = companiesResponse.ErrorMessage;
                        }

                        return View("EditSpecificUser", user);
                    }
                    else
                    {
                        ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                        return View("Employees");
                    }
                }
                else
                {
                    ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditSpecificUser(UserEnt user)
        {
            var apiResponse = await _userModel.EditSpecificUser(user);

            if (apiResponse.Success)
            {
                var editedUser = apiResponse.Data;
                return RedirectToAction("Employees");
            }
            else
            {
                ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ActivateAccountGet(int UserId)
        {
            var apiResponse = await _userModel.ActivateAccount(UserId);

            if (apiResponse.Success)
            {
                ViewBag.MensajePantalla = "Cuenta activada con éxito.";
            }
            else
            {
                ViewBag.MensajePantalla = apiResponse.ErrorMessage;
            }

            return RedirectToAction("Employees");
        }

        [HttpPost]
        public async Task<IActionResult> ActivateAccountPost(int userId)
        {
            var apiResponse = await _userModel.ActivateAccount(userId);

            if (apiResponse.Success)
            {
                return RedirectToAction("Employees");
            }
            else
            {
                ViewBag.MensajePantalla = "No se pudo activar la cuenta del usuario.";
                return RedirectToAction("Employees");
            }
        }



        public async Task<IActionResult> Profile(UserEnt entity)
        {
            var data = _userModel.GetUser(entity);
            return View(data);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        


    }
}
