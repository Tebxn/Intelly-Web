using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Intelly_Web.Controllers
{
    public class MarketingController : Controller
    {
        private readonly IMarketing _marketingModel;
        private readonly IHttpContextAccessor _HttpContextAccessor;


        public MarketingController(IMarketing marketingModel, IHttpContextAccessor httpContextAccessor)
        {
            _marketingModel = marketingModel;
            _HttpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> EmailMarketing()
        {
            try
            {
                string idCompany = _HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId");
                var apiResponse = await _marketingModel.GetAllMarketingCampaigns(idCompany);

                if (apiResponse.Success)
                {
                    var listMarketingCampaigns = apiResponse.Data.ToList();
                    return View(listMarketingCampaigns);
                }
                else
                {
                    return View(new List<MarketingCampaignEnt>()); // O proporciona un valor predeterminado
                }
            }
            catch (Exception)
            {
                return View(new List<MarketingCampaignEnt>()); // O proporciona un valor predeterminado
            }
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
