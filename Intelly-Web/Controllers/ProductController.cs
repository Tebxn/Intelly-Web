using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Intelly_Web.Controllers
{
    public class ProductController : Controller 
    {

        private readonly IProductModel _productModel;

        private readonly ICompanyModel _companyModel;

        private readonly IHttpContextAccessor _HttpContextAccessor;

        public ProductController(IProductModel productModel, ICompanyModel companyModel, IHttpContextAccessor httpContextAccessor)
        {
            _productModel = productModel;
            _companyModel = companyModel;
            _HttpContextAccessor = httpContextAccessor;


        }
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            try
            {
                var companyDropdownData = await _companyModel.GetAllCompanies();
                ViewBag.ListCompanies = companyDropdownData.Data.Select(company => new SelectListItem
                {
                    Value = company.Company_Id.ToString(),
                    Text = company.Company_Name
                });
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.MensajePantalla = "Error al cargar los datos";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct (ProductEnt entity)
        {
            var apiResponse = await _productModel.AddProduct(entity);

            if (apiResponse.Success)
            {
                return RedirectToAction("GetAllProducts", "Product");
            }
            else
            {
                ViewBag.MensajePantalla = "No se realizaron cambios";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var apiResponse = await _productModel.GetAllProducts();
                var listProducts = apiResponse.Data.ToList();
                return View(listProducts);
            }
            catch (Exception)
            {
                List<ProductEnt> errors = new List<ProductEnt>();
                return View(errors);
            }
        }


    }
}
