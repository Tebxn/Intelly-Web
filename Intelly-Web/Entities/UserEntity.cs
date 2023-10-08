namespace Intelly_Web.Entities
{
    public class UserEntity
    {
        public long User_Id { get; set; }
        public long User_Company_Id { get; set; }
        public string User_Name { get; set; } = string.Empty;
        public string User_LastName { get; set; } = string.Empty;
        public string User_Email { get; set; } = string.Empty;
        public string User_Password { get; set; } = string.Empty;
        public int User_Type { get; set; }
        public bool User_State { get; set; }
    }

    public class UserEntAnswer
    {

        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public UserEntity Object { get; set; } = null;
        public List<UserEntity> Objects { get; set; } = new List<UserEntity>();
        //public bool ResultadoTransaccion { get; set; }

    }
}
