namespace Intelly_Web.Entities
{
    public class CompanyEnt
    {
        public long Company_Id { get; set; }
        public string Company_Name { get; set; } = string.Empty;
        public string Company_Email { get; set; } = string.Empty;
        public string Company_Phone { get; set; } = string.Empty;
        public string? Company_Connection_String { get; set; }
    }

    public class CompanyeEntAnswer
    {
        public CompanyEnt? Object { get; set; } = null;
        public List<CompanyEnt> Objects { get; set; } = new List<CompanyEnt>();
    }
}
