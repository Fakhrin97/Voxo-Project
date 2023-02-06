using Microsoft.AspNetCore.Mvc;

namespace Voxo_Project.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
