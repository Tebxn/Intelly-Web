namespace Intelly_Web.Entities
{
    public class UserEnt
    {
        public long User_Id { get; set; }
        public long User_Company_Id { get; set; }
        public string User_Name { get; set; } = string.Empty;
        public string User_LastName { get; set; } = string.Empty;
        public string User_Email { get; set; } = string.Empty;
        public string User_Password { get; set; } = string.Empty;
        public int User_Type { get; set; }
        public bool User_State { get; set; }
        public bool User_Password_IsTemp { get; set; }
        public List<UserEnt> List { get; set; } = new List<UserEnt>();
    }

    public class UserEntAnswer
    {

       
        public UserEnt Object { get; set; } = null;
        public List<UserEnt> Objects { get; set; } = new List<UserEnt>();
    

    }
}
