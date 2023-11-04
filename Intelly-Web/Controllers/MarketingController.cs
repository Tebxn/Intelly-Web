using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Intelly_Web.Controllers
{
    public class MarketingController : Controller
    {
        private readonly IMarketing _marketingModel;


        public MarketingController(IMarketing marketingModel)
        {
            _marketingModel = marketingModel;

        }

        [HttpGet]
        public IActionResult EmailMarketing()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewEmailCampaign(EmailEnt entity)
        {
            var apiResponse = await _marketingModel.EmailMarketingManual(entity);

            if (apiResponse.Success)
            {
                return View("EmailMarketingManual", "Marketing");
            }
            else
            {
                ViewBag.MensajePantalla = "No se realizaron cambios";
                return View();
            }
        }



    }
}
