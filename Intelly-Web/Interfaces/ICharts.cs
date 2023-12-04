using Intelly_Web.Entities;

namespace Intelly_Web.Interfaces
{
    public interface ICharts
    {
        Task<ApiResponse<long>> ChartNewCustomersMonth();
        Task<ApiResponse<long>> ChartActivesMarketingCampaigns();
        Task<ApiResponse<long>> ChartEmailsSendedMonth();
        Task<ApiResponse<long>> ChartSellsWithCampaignMonth();
        Task<ApiResponse<List<ChartEnt>>> ChartSumTotalByMonthActualYear();
        Task<ApiResponse<List<ChartEnt>>> ChartTopCampaignsByTotal();

    }
}
