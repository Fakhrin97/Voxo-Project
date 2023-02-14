
namespace Voxo_Project.ViewComponents
{
    public class PopularPostsViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public PopularPostsViewComponent(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = await _dbContext.Blogs
                .Where(blog => !blog.Published)
                .OrderByDescending(blog => blog.ViewersCount)
                .Take(5)
                .ToListAsync();

            var model = _mapper.Map<List<PopularBlogsVM>>(blogs);

            return View(model);
        }
    }
}
