using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [HttpGet]
        public async Task<IActionResult> CreateCampaign()
        {
            try
            {
                var roleDropdownData = await _marketingModel.GetAllMembershipLevels();
                ViewBag.MembershipLevelList = roleDropdownData.Data.Select(x => new SelectListItem
                {
                    Value = x.Membership_Id.ToString(),
                    Text = x.Membership_Name
                });
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCampaign(MarketingCampaignEnt entity)
        {
            var apiResponse = await _marketingModel.CreateMarketingCampaign(entity);

            if (apiResponse.Success)
            {
                return RedirectToAction("CreateCampaign", "Marketing");
            }
            else
            {
                return RedirectToAction("CreateCampaign", "Marketing");
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
