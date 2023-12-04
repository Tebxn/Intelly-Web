using Intelly_Web.Entities;

namespace Intelly_Web.Models
{
    public class HomeViewModel
    {
        public long NewCustomersMonth { get; set; }
        public long ActiveMarketingCampaigns { get; set; }
        public long EmailsSendedMonth { get; set; }
        public long SellsWithCampaign { get; set; }
        public List<float> SumTotalByMonthActualYear { get; set; }
        public List<string> TopCampaigns { get; set; }
        public List<long> TopCampaignsSells { get; set; }
    }
}
