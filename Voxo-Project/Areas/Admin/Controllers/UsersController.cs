using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Voxo.BLL.Data;
using Voxo.BLL.ViewModels;
using Voxo.DAL.DataContext;
using Voxo.DAL.Entities;

namespace Voxo_Project.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(AppDbContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var model = new List<UserVM>();

            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _dbContext.UserRoles.ToListAsync();

            foreach (var user in users)
            {
                var userRole = userRoles.FirstOrDefault(x => x.UserId == user.Id);
                var role = roles.FirstOrDefault(x => x.Id == userRole.RoleId).Name;

                model.Add(new UserVM
                {
                    Id = user.Id,
                    LastLogin =user.LastLogin,  
                    Firstname = user.Fristname,
                    Lastname = user.Lastname,
                    Username = user.UserName,
                    Email = user.Email,
                    Role = role,
                    EmailConfirmed= user.EmailConfirmed,

                });
            }

            return View(model);
        }

        public IActionResult Create()
        {
            var model = new UserCreateVM
            {
                Roles = GetRoles()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateVM model)
        {
            var viewModel = new UserCreateVM
            {
                Roles = GetRoles()
            };

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = new User
            {
                Fristname = model.Firstname,
                Lastname = model.Lastname,
                UserName = model.Username,
                Email = model.Email,
                EmailConfirmed = true,               
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View(viewModel);
            }

            var createdUser = await _userManager.FindByNameAsync(model.Username);

            if (createdUser == null)
            {
                ModelState.AddModelError("", "User yaradilmadi");
                return View(viewModel);
            }

            result = await _userManager.AddToRoleAsync(createdUser, model.Role);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var model = new UserUpdateVM
            {
                
                Firstname = user.Fristname,
                Lastname = user.Lastname,
                Username = user.UserName,
                Email = user.Email,
                CurrentRole = currentRole,
                Role = currentRole,
                Roles = GetRoles(),
                EmailConfirmed = user.EmailConfirmed,

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserUpdateVM model, string id)
        {
            var viewModel = new UserUpdateVM
            {
                Roles = GetRoles(),
                Role = model.Role
            };

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var existUser = await _userManager.FindByIdAsync(id);

            if (existUser == null) return NotFound();

            existUser.Fristname = model.Firstname;
            existUser.Lastname = model.Lastname;
            existUser.UserName = model.Username;
            existUser.Email = model.Email;
            existUser.EmailConfirmed= model.EmailConfirmed; 

            if (model.CurrentRole != model.Role)
            {
                var result = await _userManager.RemoveFromRoleAsync(existUser, model.CurrentRole);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View(viewModel);
                }

                result = await _userManager.AddToRoleAsync(existUser, model.Role);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View(viewModel);
                }

            }

            var userResult = await _userManager.UpdateAsync(existUser);

            if (!userResult.Succeeded)
            {
                foreach (var item in userResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }


        private List<SelectListItem> GetRoles()
        {
            var roles = new List<string> { Constants.AdminRole, Constants.UserRole };
            var roleSelectedList = new List<SelectListItem>();

            roles.ForEach(x => roleSelectedList.Add(new SelectListItem(x, x)));

            return roleSelectedList;
        }
    }
}
