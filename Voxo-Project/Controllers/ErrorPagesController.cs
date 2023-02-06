using Microsoft.AspNetCore.Mvc;

namespace Voxo_Project.Controllers
{
    public class ErrorPagesController : Controller
    {
        public IActionResult Error(int? code)
        {
            switch (code)
            {
                case 404:
                    return RedirectToAction(nameof(Error404));

                default:
                    return RedirectToAction(nameof(Error404));
            }

        }
        public IActionResult Error404()
        {
            return View();
        }

    }
}
