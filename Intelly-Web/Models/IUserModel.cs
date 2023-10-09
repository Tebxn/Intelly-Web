using Intelly_Web.Entities;

namespace Intelly_Web.Models
{
    public interface IUserModel
    {

        public int AddUser(UserEntity  entity);

        Task<List<UserEntity>> GetAllUsers();

        public UserEntity? Login(UserEntity entity);


    }
}
