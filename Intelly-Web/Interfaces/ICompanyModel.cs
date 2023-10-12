using Intelly_Web.Entities;

namespace Intelly_Web.Interfaces
{
    public interface ICompanyModel
    {
        Task<ApiResponse<List<CompanyEnt>>> GetAllCompanies();
        Task<ApiResponse<CompanyEnt>> CreateCompany(CompanyEnt entity);
    }
}
