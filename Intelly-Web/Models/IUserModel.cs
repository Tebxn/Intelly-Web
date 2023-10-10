using Intelly_Web.Entities;

namespace Intelly_Web.Models
{
    public interface IUserModel
    {

        public string? AddEmployee(UserEnt entity);

        Task<List<UserEnt>?> GetAllUsers();

        Task<UserEnt?> Login(UserEnt entity);


    }
}
