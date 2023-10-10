using Intelly_Web.Entities;

namespace Intelly_Web.Interfaces
{
    public interface IUserModel
    {

        Task<ApiResponse<UserEnt>> AddEmployee(UserEnt entity);

        Task<ApiResponse<List<UserEnt>>> GetAllUsers();

        Task<ApiResponse<UserEnt>> Login(UserEnt entity);


    }
}
