using Intelly_Web.Entities;

namespace Intelly_Web.Interfaces
{
    public interface IUserModel
    {

        Task<ApiResponse<UserEnt>> AddEmployee(UserEnt entity);

        Task<ApiResponse<List<UserEnt>>> GetAllUsers();

        Task<ApiResponse<UserEnt>> Login(UserEnt entity);

        Task<ApiResponse<UserEnt>> GetSpecificUser(int UserId);

        Task<ApiResponse<UserEnt>> EditSpecificUser(UserEnt entity);

        Task<ApiResponse<List<UserTypeEnt>>> GetAllUsersRoles();

        Task<ApiResponse<UserEnt>> PwdRecovery(UserEnt entity);

        Task<ApiResponse<UserEnt>> UpdateUserPassword(UserEnt entity);

        Task<ApiResponse<UserEnt>> GetUser(UserEnt entity);


    }
}
