using Intelly_Web.Entities;

namespace Intelly_Web.Models
{
    public interface IUserModel
    {

        public int AddUser(UserEnt  entity);

        Task<List<UserEnt>> GetAllUsers();

        public UserEnt? Login(UserEnt entity);


    }
}
