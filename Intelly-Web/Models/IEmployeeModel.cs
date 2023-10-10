using Intelly_Web.Entities;

namespace Intelly_Web.Models
{
    public interface IEmployeeModel
    {

        public string? AddEmployee(EmployeeEnt entity);

        Task<List<EmployeeEnt>?> GetAllUsers();

        Task<EmployeeEnt?> Login(EmployeeEnt entity);


    }
}
