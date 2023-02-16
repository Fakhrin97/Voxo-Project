using Microsoft.AspNetCore.Mvc;

namespace Voxo_Project.Areas.Admin.Controllers
{
    public class ContactMessagesController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public ContactMessagesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _dbContext.ContactMessages
                .ToListAsync();

            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _dbContext.ContactMessages.Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (message is null) return NotFound();

            _dbContext.ContactMessages.Remove(message);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
