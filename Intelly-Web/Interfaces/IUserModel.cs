using Intelly_Web.Entities;
using System.Data;

namespace Intelly_Web.Interfaces
{
    public interface IUserModel
    {

        Task<ApiResponse<UserEnt>> AddEmployee(UserEnt entity);

        Task<ApiResponse<List<UserEnt>>> GetAllUsers();

        Task<ApiResponse<UserEnt>> Login(UserEnt entity);

        Task<ApiResponse<UserEnt>> GetSpecificUser(long userId);

        Task<ApiResponse<UserEnt>> EditSpecificUser(UserEnt entity);

        Task<ApiResponse<List<UserRoleEnt>>> GetAllUsersRoles();

        Task<ApiResponse<UserEnt>> UpdateUserPassword(UserEnt entity);

        Task<ApiResponse<string>> ActivateAccount(int User_Id);

        Task<ApiResponse<UserEnt>> ChangePassword(UserEnt entity);

        //Task<ApiResponse<UserEnt>> DisableSpecificUser(UserEnt entity);
        Task<ApiResponse<UserEnt>> UpdateUserState(UserEnt entity);

        Task<ApiResponse<UserEnt>> UpdateNewPassword(UserEnt entity);

        Task<ApiResponse<UserEnt>> PwdRecovery(UserEnt entity);




    }
}
