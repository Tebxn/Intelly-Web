using Intelly_Web.Entities;

namespace Intelly_Web.Interfaces
{
    public interface ICustomer
    {
        Task<ApiResponse<List<CustomerEnt>>> GetAllCustomers();
    }
}
