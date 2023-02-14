
namespace Voxo_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _mailService = mailService;
        }
        public IActionResult Index()
        {
            return RedirectToAction(nameof(LogIn));
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInVM model)
        {
            if (!ModelState.IsValid) return View();

            var existUser = await _userManager.FindByNameAsync(model.Username);

            if (existUser == null)
            {
                ModelState.AddModelError("", "This User Not Exist");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(existUser, model.Password, false, true);

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "Email must be verified, please check your email");
                return View();
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "This Account Locked Out");
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Password or Username Incorrect");
                return View();
            }

            existUser.LastLogin = DateTime.Now;

            await _userManager.UpdateAsync(existUser);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View();

            var existUser = await _userManager.FindByNameAsync(model.Username);

            if (existUser != null)
            {
                ModelState.AddModelError("", "Username Artiq Istifade Olunub");
                return View();
            }

            var user = new User
            {
               UserName = model.Username,
               Email = model.Email, 
               Fristname = model.Fristname,
               Lastname = model.Lastname,   

            };

            var result = await _userManager.CreateAsync(user, model.Password);
          

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            await _userManager.AddToRoleAsync(user, ConstantsData.UserRole);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var resetLink = Url.Action(nameof(ConfirmEmail), "Account", new { mail = model.Email, token }, Request.Scheme, Request.Host.ToString());

            var requestEmail = new RequestEmail
            {
                ToEmail = model.Email,
                Body = resetLink,
                Subject = "Confirm Email",
            };

            await _mailService.SendEmailAsync(requestEmail);

            return RedirectToAction(nameof(LogIn));
        }
        public async Task<IActionResult> ConfirmEmail(string mail, string token)
        {
            var user = await _userManager.FindByEmailAsync(mail);

            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangePassword()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(LogIn));

            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existUser = await _userManager.FindByNameAsync(User?.Identity?.Name);

            if (existUser == null)
                return BadRequest();

            var result = await _userManager.ChangePasswordAsync(existUser, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(LogIn));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            if (!ModelState.IsValid) return View();

            var existUser = await _userManager.FindByEmailAsync(model.Email);

            if(existUser is null)
            {
                ModelState.AddModelError("", "This email not exist");
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(existUser);

            var resetLink = Url.Action(nameof(ResetPassword), "Account",
                new { email = model.Email, token },
                Request.Scheme, Request.Host.ToString());

            var requestEmail = new RequestEmail
            {
                ToEmail = model.Email,
                Body = resetLink,
                Subject = "Reset Link",
            };

            await _mailService.SendEmailAsync(requestEmail);

            return RedirectToAction(nameof(LogIn));
        }

        public IActionResult ResetPassword(string email, string token)
        {
            return View(new ResetPasswordVM
            {
                Email = email,
                Token = token,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return BadRequest();

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            return RedirectToAction(nameof(LogIn));
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index","Home");
        }

    }
}
