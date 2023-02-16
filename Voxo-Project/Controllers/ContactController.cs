
namespace Voxo_Project.Controllers
{
    public class ContactController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public ContactController(UserManager<User> userManager, IMapper mapper, AppDbContext dbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
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

        public async Task<IActionResult> SendMessage(ContactMessageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(viewName: nameof(Index), model);
            }

            var cotactMessage = _mapper.Map<ContactMessage>(model);

            await _dbContext.ContactMessages.AddAsync(cotactMessage);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
