﻿namespace Intelly_Web.Entities
{
    public class UserEnt
    {
        public long User_Id { get; set; }
        public long? User_Company_Id { get; set; }
        public string? User_Name { get; set; } = string.Empty;
        public string? User_LastName { get; set; } = string.Empty;
        public string? User_Email { get; set; } = string.Empty;
        public string? User_Password { get; set; } = string.Empty;
        public int? User_Type { get; set; }
        public bool User_State { get; set; } = true;
        public bool? User_Password_IsTemp { get; set; }
        public string User_PasswordTemp { get; set; } = string.Empty;
        public string User_Secure_Id { get; set; } = string.Empty;
        public string UserToken { get; set; } = string.Empty;
    }

    public class EmployeeEntAnswer
    {
        public UserEnt? Object { get; set; } = null;
        public List<UserEnt> Objects { get; set; } = new List<UserEnt>();
    }
}
