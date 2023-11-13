using Intelly_Web.Entities;

namespace Intelly_Web.Interfaces
{
    public interface IMarketing
    {
        Task<ApiResponse<EmailEnt>> EmailMarketingManual(EmailEnt entity);
        Task<ApiResponse<List<MarketingCampaignEnt>>> GetAllMarketingCampaigns();
        Task<ApiResponse<MarketingCampaignEnt>> CreateMarketingCampaign(MarketingCampaignEnt entity);
        Task<ApiResponse<List<MembershipEnt>>> GetAllMembershipLevels();
    }
}
