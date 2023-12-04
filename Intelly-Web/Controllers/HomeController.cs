using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Intelly_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICharts _charts;


        public HomeController(ILogger<HomeController> logger, ICharts charts)
        {
            _logger = logger;
            _charts = charts;
        }

        public async Task<IActionResult> IndexAsync()
        {
                var newCustomersMonth = await _charts.ChartNewCustomersMonth();
                var activeMarketingCampaigns = await _charts.ChartActivesMarketingCampaigns();
                var emailsSendedMonth = await _charts.ChartEmailsSendedMonth();
                var sellsWithCampaign = await _charts.ChartSellsWithCampaignMonth();
                var sumTotalByMonthActualYear = await _charts.ChartSumTotalByMonthActualYear();
                var topCampaigns = await _charts.ChartTopCampaignsByTotal();

            List<float> sells = new List<float>();
            List<string> topCampaignsName = new List<string>();
            List<long> topCampaignsSells = new List<long>();

            foreach (var item in sumTotalByMonthActualYear.Data)
            {
                sells.Add((float)item.Total);
            }
            foreach (var item in topCampaigns.Data)
            {
                topCampaignsName.Add((string)item.MarketingCampaign_Name);
                topCampaignsSells.Add((long)item.Total);
            }

            var model = new HomeViewModel
            {
                NewCustomersMonth = newCustomersMonth.Data,
                ActiveMarketingCampaigns = activeMarketingCampaigns.Data,
                EmailsSendedMonth = emailsSendedMonth.Data,
                SellsWithCampaign = sellsWithCampaign.Data,
                SumTotalByMonthActualYear = sells,
                TopCampaigns = topCampaignsName,
                TopCampaignsSells = topCampaignsSells
            };

            return View("Home", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}