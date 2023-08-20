using Microsoft.AspNetCore.Mvc;

namespace Intelly_Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult AddEmployee()
        {
            return View();
        }

        public IActionResult Employees()
        {
            return View();
        }
    }
}
