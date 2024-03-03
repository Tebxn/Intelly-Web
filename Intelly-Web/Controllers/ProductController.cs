using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using static Intelly_Web.Entities.SecurityFilter;

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
                // Obtén los datos de la sesión
                string company = HttpContext.Session.GetString("UserCompanyId");

                // Convierte el string a long
                long companyId = Convert.ToInt64(company);

                // Crea un modelo con los datos de la sesión
                var viewModel = new ProductEnt
                {
                    Product_CompanyId = companyId

                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.MensajePantalla = "Error al cargar los datos";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductEnt entity)
        {

            string company = HttpContext.Session.GetString("UserCompanyId");

            long companyId = Convert.ToInt64(company);

            entity.Product_CompanyId = companyId;

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

        [HttpGet]
        public async Task<IActionResult> GetSpecificProduct(int ProductId)
        {
            try
            {
                var apiResponse = await _productModel.GetSpecificProduct(ProductId);
                if (apiResponse.Success)
                {
                    var product = apiResponse.Data;
                    if (product != null)
                    {
                        return RedirectToAction("EditSpecificProduct", new { ProductId = ProductId });
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSpecificProduct(int ProductId)
        {
            try
            {
                var apiResponse = await _productModel.GetSpecificProduct(ProductId);
                if (apiResponse.Success)
                {
                    var product = apiResponse.Data;
                    if (product != null)
                    {
                        return View("EditSpecificProduct", product);
                    }
                    else
                    {
                        ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                        return View("GetAllProducts");
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
        public async Task<IActionResult> EditSpecificProduct(ProductEnt product)
        {
            string company = HttpContext.Session.GetString("UserCompanyId");

            long companyId = Convert.ToInt64(company);

            product.Product_CompanyId = companyId;

            try
            {
                var apiResponse = await _productModel.EditSpecificProduct(product);

                if (apiResponse.Success)
                {
                    var editedCompany = apiResponse.Data;
                    return RedirectToAction("GetAllProducts", "Product");
                }
                else
                {
                    ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajePantalla = "Unexpected Error: " + ex.Message;
                return View();
            }
        }

        //[HttpPut]
        //public async Task<IActionResult> UpdateProductState(ProductEnt entity)
        //{

        //    var apiResponse = await _productModel.UpdateProductState(entity);

        //    if (apiResponse.Success)
        //    {
        //        return RedirectToAction("GetAllProducts", "Product");
        //    }
        //    else
        //    {
        //        ViewBag.MensajePantalla = "No se realizaron cambios";
        //        return View();
        //    }
        //}


        [HttpGet]
        public async Task<IActionResult> UpdateProductState(int ProductId)
        {
            try
            {
                // Obtener el producto por su ID
                var apiResponse = await _productModel.GetSpecificProduct(ProductId);

                if (apiResponse.Success)
                {
                    var product = apiResponse.Data;

                    // Llamar al método en el modelo para actualizar el estado del producto
                    var updateResponse = await _productModel.UpdateProductState(product);

                    if (updateResponse.Success)
                    {
                        // Éxito al actualizar el estado del usuario
                        return RedirectToAction("GetAllProducts", "Product"); // Redirigir a la vista deseada después de la actualización
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
        public async Task<IActionResult> UpdateProductState(ProductEnt entity)
        {

            var apiResponse = await _productModel.UpdateProductState(entity);

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
