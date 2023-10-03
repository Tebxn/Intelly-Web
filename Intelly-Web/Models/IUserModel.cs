using Intelly_Web.Entities;

namespace Intelly_Web.Models
{
    public interface IUserModel
    {

        public void AddUser(UserEntitie  entitie);

        public void ShowUsers(UserEntitie entitie);

        public void EditUser(UserEntitie entitie);

        public void DeleteUser(UserEntitie entitie);

    }
}
