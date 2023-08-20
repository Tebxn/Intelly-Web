using Microsoft.AspNetCore.Mvc;

namespace Intelly_Web.Controllers
{
    public class MailController : Controller
    {
        public IActionResult Emails()
        {
            return View();
        }
    }
}
