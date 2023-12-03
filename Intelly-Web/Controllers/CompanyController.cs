﻿using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using static Intelly_Web.Entities.SecurityFilter;

namespace Intelly_Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyModel _companyModel;

        public CompanyController(ICompanyModel companyModel)
        {
            _companyModel = companyModel;
        }

        [SecurityFilter]

        public async Task<IActionResult> CreateCompany()
        {
            return View();
        }

        [HttpPost]
        [SecurityFilter]

        public async Task<IActionResult> CreateCompany(CompanyEnt entity)
        {
            var apiResponse = await _companyModel.CreateCompany(entity);

            if (apiResponse.Success)
            {
                return RedirectToAction("GetAllCompanies", "Company");
            }
            else
            {
                ViewBag.MensajePantalla = "No se realizaron cambios";
                return View();
            }
        }

        [HttpGet]
        [SecurityFilter]

        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                var apiResponse = await _companyModel.GetAllCompanies();
                var listCompanies = apiResponse.Data.ToList();
                return View(listCompanies);
            }
            catch (Exception)
            {
                List<CompanyEnt> errors = new List<CompanyEnt>();
                return View(errors);
            }
        }

        [HttpGet]
        [SecurityFilter]

        public async Task<IActionResult> GetSpecificCompany(int CompanyId)
        {
            try
            {
                var apiResponse = await _companyModel.GetSpecificCompany(CompanyId);
                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;
                    if (user != null)
                    {
                        return RedirectToAction("EditSpecificCompany", new { CompanyId = CompanyId });
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

        public async Task<IActionResult> EditSpecificCompany(int CompanyId)
        {
            try
            {
                var apiResponse = await _companyModel.GetSpecificCompany(CompanyId);
                if (apiResponse.Success)
                {
                    var company = apiResponse.Data;
                    if (company != null)
                    {
                        return View("EditSpecificCompany", company);
                    }
                    else
                    {
                        ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                        return View("GetAllCompanies");
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
        public async Task<IActionResult> EditSpecificCompany(CompanyEnt company)
        {
            var apiResponse = await _companyModel.EditSpecificCompany(company);

            if (apiResponse.Success)
            {
                var editedCompany = apiResponse.Data;
                return RedirectToAction("GetAllCompanies", "Company");
            }
            else
            {
                ViewBag.MensajePantalla = apiResponse.ErrorMessage;
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
