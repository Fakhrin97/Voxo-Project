
namespace Voxo_Project.Areas.Admin.Controllers
{
    public class ContactMessageViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public ContactMessageViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var messages = await _dbContext.ContactMessages
                .OrderByDescending(x => x.Id)
                .Take(3)
                .ToListAsync();

            var isNotReadMessages = await _dbContext.ContactMessages
                .Where(x => !x.IsRead)
                .ToListAsync();

            var model = new NotiticationsVM
            {
                ContactMessages = messages,
                Count = isNotReadMessages.Count,
            };

            return View(model);
        }
    }
}