
namespace Voxo_Project.Controllers
{
    public class BlogsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public BlogsController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs
                .Where(blog => !blog.Published)
                .ToListAsync();

            var blogsVM = _mapper.Map<List<BlogPageVM>>(blogs);

            return View(blogsVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _dbContext.Blogs
                .Where(blog => blog.Id == id)
                .FirstOrDefaultAsync();

            if(blog == null) return NotFound();

            blog.ViewersCount++;

            _dbContext.SaveChanges();

            var blogDetails = _mapper.Map<BlogDetailsVM>(blog);

            return View(blogDetails);  
        }
    }
}
