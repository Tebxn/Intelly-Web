using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyModel _companyModel;

        public CompanyController(ICompanyModel companyModel)
        {
            _companyModel = companyModel;
        }

        public IActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCompany(CompanyEnt entity)
        {
            var resp = _companyModel.CreateCompany(entity);
            if (resp.IsCompletedSuccessfully)
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
