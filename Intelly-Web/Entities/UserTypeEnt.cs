namespace Intelly_Web.Entities
{
    public class UserTypeEnt
    {
        public long UserType_Id { get; set; }
        public string UserType_Name { get; set; } = string.Empty;
        public List<UserTypeEnt> List { get; set; } = new List<UserTypeEnt>();
    }
}
