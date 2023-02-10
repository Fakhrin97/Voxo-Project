using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Voxo.BLL.ViewModels;
using Voxo.DAL.Entities;

namespace Voxo_Project.Controllers
{
    public class ContactController : Controller
    {
        private readonly UserManager<User> _userManager;

        public ContactController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContactMessageVM();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                model.Email = user.Email;
                model.Name = user.UserName;
            }   

            return View(model);
        }
    }
}
