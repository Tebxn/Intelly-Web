using Intelly_Web.Entities;

namespace Intelly_Web.Interfaces
{
    public interface ICharts
    {
        Task<ApiResponse<long>> ChartNewCustomersMonth();
    }
}
