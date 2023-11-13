using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

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
                var apiResponse = await _marketingModel.GetAllMarketingCampaigns();

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
        }//Principal Listado de campanias

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

        [HttpGet]
        public async Task<IActionResult> CreateCampaignEmail()
        {
            try
            {
                var respMarketingCampaigns = await _marketingModel.GetAllMarketingCampaigns();

                if (respMarketingCampaigns.Success)
                {
                    var listMarketingCampaign = new List<SelectListItem>();

                    foreach (var item in respMarketingCampaigns.Data)
                    {
                        listMarketingCampaign.Add(new SelectListItem { Value = item.MarketingCampaign_Id.ToString(), Text = item.MarketingCampaign_Name ?? string.Empty });
                    }

                    ViewBag.ListMarketingCampaign = listMarketingCampaign;

                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "Error al obtener las campañas de marketing.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error inesperado: " + ex.Message;
                return View();
            }
        }


    }
}
