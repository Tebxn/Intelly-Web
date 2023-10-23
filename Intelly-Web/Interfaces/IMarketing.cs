using Intelly_Web.Entities;

namespace Intelly_Web.Interfaces
{
    public interface IMarketing
    {
        Task<ApiResponse<EmailEnt>> EmailMarketingManual(EmailEnt entity);
    }
}
