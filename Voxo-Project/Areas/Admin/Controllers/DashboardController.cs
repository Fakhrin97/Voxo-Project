using Microsoft.AspNetCore.Mvc;

namespace Voxo_Project.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
