using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intelly_Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomer _customerModel;

        public CustomerController(ICustomer customerModel)
        {
            _customerModel = customerModel;
        }

        [HttpGet]
        public async Task<IActionResult> Customers()
        {
            try
            {
                var apiResponse = await _customerModel.GetAllCustomers();

                if (apiResponse.Success)
                {
                    var listCostumers = apiResponse.Data.ToList();
                    return View(listCostumers);
                }
                else
                {
                    return View(new List<CustomerEnt>());
                }
            }
            catch (Exception)
            {
                return View(new List<CustomerEnt>());
            }
        }
    }
}
