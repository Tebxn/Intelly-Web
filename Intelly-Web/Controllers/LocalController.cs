using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using static Intelly_Web.Entities.SecurityFilter;

namespace Intelly_Web.Controllers
{
    public class LocalController : Controller
    {
        private readonly ILocalModel _localModel;

        public LocalController(ILocalModel localModel)
        {
            _localModel = localModel;
        }

        public async Task<IActionResult> CreateLocal()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocal(LocalEnt entity)
        {
            var apiResponse = await _localModel.CreateLocal(entity);

            if (apiResponse.Success)
            {
                return RedirectToAction("GetAllLocals", "Local");
            }
            else
            {
                ViewBag.MensajePantalla = "No se realizaron cambios";
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllLocals()
        {
            try
            {
                var apiResponse = await _localModel.GetAllLocals();

                if (apiResponse.Success)
                {
                    var listCostumers = apiResponse.Data.ToList();
                    return View(listCostumers);
                }
                else
                {
                    return View(new List<LocalEnt>());
                }
            }
            catch (Exception)
            {
                return View(new List<LocalEnt>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecificLocal(int LocalId)
        {
            try
            {
                var apiResponse = await _localModel.GetSpecificLocal(LocalId);
                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;
                    if (user != null)
                    {
                        return RedirectToAction("EditSpecificLocal", new { LocalId = LocalId });
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
        public async Task<IActionResult> EditSpecificLocal(int LocalId)
        {
            try
            {
                var apiResponse = await _localModel.GetSpecificLocal(LocalId);
                if (apiResponse.Success)
                {
                    var local = apiResponse.Data;
                    if (local != null)
                    {
                        return View("EditSpecificLocal", local);
                    }
                    else
                    {
                        ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                        return View("GetAllLocals");
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
        public async Task<IActionResult> EditSpecificLocal(LocalEnt local)
        {
            var apiResponse = await _localModel.EditSpecificLocal(local);

            if (apiResponse.Success)
            {
                var editedCompany = apiResponse.Data;
                return RedirectToAction("GetAllLocals", "Local");
            }
            else
            {
                ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateLocalState(int LocalId)
        {
            try
            {
                // Obtener del local por su ID
                var apiResponse = await _localModel.GetSpecificLocal(LocalId);

                if (apiResponse.Success)
                {
                    var product = apiResponse.Data;

                    // Llamar al método en el modelo para actualizar el estado del local
                    var updateResponse = await _localModel.UpdateLocalState(product);

                    if (updateResponse.Success)
                    {
                        // Éxito al actualizar el estado del local
                        return RedirectToAction("GetAllLocals", "Local"); // Redirigir a la vista deseada después de la actualización
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
        public async Task<IActionResult> UpdateLocalState(LocalEnt entity)
        {

            var apiResponse = await _localModel.UpdateLocalState(entity);

            if (apiResponse.Success)
            {
                return RedirectToAction("GetAllLocals", "Local");
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
