using Intelly_Web.Entities;
using Intelly_Web.Implementations;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using static Intelly_Web.Entities.SecurityFilter;

namespace Intelly_Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserModel _userModel;

        private readonly ICompanyModel _companyModel;

        private readonly IHttpContextAccessor _HttpContextAccessor;


        public UserController(IUserModel userModel, ICompanyModel companyModel, IHttpContextAccessor httpContextAccessor)
        {
            _userModel = userModel;
            _companyModel = companyModel;
            _HttpContextAccessor = httpContextAccessor;


        }

        [HttpGet]
        [SecurityFilter]
        [SecurityFilterIsAdmin]
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
        [SecurityFilter]
        [SecurityFilterIsAdmin]
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
        public async Task<IActionResult> GetSpecificUser(long userId)
        {
            try
            {
                var apiResponse = await _userModel.GetSpecificUser(userId);
                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;
                    if (user != null)
                    {
                        return RedirectToAction("EditSpecificUser", new { UserId = userId });
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
        [SecurityFilter]
        [SecurityFilterIsAdmin]
        public async Task<IActionResult> EditSpecificUser(long userId)
        {
            try
            {
                var apiResponse = await _userModel.GetSpecificUser(userId);
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
                            ViewBag.ComboCompanies = companies.Select(company => new SelectListItem { Value = company.Company_Id.ToString(), Text = company.Company_Name });
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
        [SecurityFilter]
        [SecurityFilterIsAdmin]
        public async Task<IActionResult> EditSpecificUser(UserEnt user)
        {
            var apiResponse = await _userModel.EditSpecificUser(user);

            if (apiResponse.Success)
            {
                var editedUser = apiResponse.Data;
                return RedirectToAction("Employees", "User");
            }
            else
            {
                ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActivateAccount(int User_Id)
        {
            try
            {
                var apiResponse = await _userModel.ActivateAccount(User_Id);

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
            catch (Exception ex)
            {
                ViewBag.MensajePantalla = "Error: " + ex.Message;
                return View("Error");
            }
        }


        //[HttpGet]
        //public async Task<IActionResult> GetProfile()
        //{
        //    try
        //    {
        //        // Obtén el userId directamente de la sesión
        //        long userId;

        //        if (long.TryParse(HttpContext.Session.GetString("UserId"), out userId))
        //        {
        //            // Llama al método GetSpecificUser del modelo web con el userId
        //            var apiResponse = await _userModel.GetSpecificUser(userId);

        //            if (apiResponse.Success)
        //            {
        //                var user = apiResponse.Data;
        //                if (user != null)
        //                {
        //                    // Pasa el modelo de usuario a la vista
        //                    return View(user);
        //                }
        //                else
        //                {
        //                    ViewBag.MensajePantalla = "No se pudo desplegar el perfil";
        //                    return View();
        //                }
        //            }
        //            else
        //            {
        //                ViewBag.MensajePantalla = "No se logró conexión con el servidor";
        //                return View();
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.MensajePantalla = "Error al obtener el UserId de la sesión";
        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.MensajePantalla = "Error al cargar los datos: " + ex.Message;
        //        return View();
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                // Obtén los datos de la sesión
                string userName = HttpContext.Session.GetString("UserName");
                string userLastName = HttpContext.Session.GetString("UserLastName");
                string userEmail = HttpContext.Session.GetString("UserEmail");

                // Crea un modelo con los datos de la sesión
                var viewModel = new UserEnt
                {
                    User_Name = userName,
                    User_LastName = userLastName,
                    User_Email = userEmail
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Manejar errores aquí
                ViewBag.MensajePantalla = "Error al cargar los datos: " + ex.Message;
                return View();
            }
        }


        [HttpGet]
        [SecurityFilter]
        [SecurityFilterIsAdmin]
        [HttpGet]
        public async Task<IActionResult> UpdateUserState(long userId)
        {
            try
            {
                // Obtener el usuario por su ID
                var apiResponse = await _userModel.GetSpecificUser(userId);

                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;

                    // Llamar al método en el modelo para actualizar el estado del usuario
                    var updateResponse = await _userModel.UpdateUserState(user);

                    if (updateResponse.Success)
                    {
                        // Éxito al actualizar el estado del usuario
                        return RedirectToAction("Employees", "User"); // Redirigir a la vista deseada después de la actualización
                    }
                    else
                    {
                        ViewBag.ErrorMessage = updateResponse.ErrorMessage;
                        return View(); // Algo salió mal al cambiar el estado del usuario
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = apiResponse.ErrorMessage;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al cambiar el estado del usuario: " + ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [SecurityFilter]
        [SecurityFilterIsAdmin]
        public async Task<IActionResult> UpdateUserState(UserEnt entity)
        {

            var apiResponse = await _userModel.UpdateUserState(entity);

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
