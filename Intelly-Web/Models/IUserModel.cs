using Intelly_Web.Entities;

namespace Intelly_Web.Models
{
    public interface IUserModel
    {

        public int AddUser(UserEntity  entity);

        public List<UserEntity> GetAllUsers();
        // public void EditUser(UserEntity entity);

        // public void DeleteUser(UserEntity entity);

        public UserEntity? Login(UserEntity entity);


    }
}
