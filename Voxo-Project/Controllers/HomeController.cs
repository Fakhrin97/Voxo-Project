using Microsoft.AspNetCore.Mvc;
using Voxo.DAL.DataContext;

namespace Voxo_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {            
            return View();
        }
    }
}