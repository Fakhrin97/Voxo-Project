
namespace Voxo_Project.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoryViewComponent(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _dbContext.Categories
                .Where(category => !category.Published)
            .ToListAsync();

            var model = _mapper.Map<List<CategoryVM>>(categories);

            return View(model);
        }
    }
}
