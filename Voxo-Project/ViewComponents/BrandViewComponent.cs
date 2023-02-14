
namespace Voxo_Project.ViewComponents
{
    public class BrandViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public BrandViewComponent(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brands = await _dbContext.Brands.Where(brand => !brand.Published)
            .ToListAsync();

            var model = _mapper.Map<List<BrandImgVM>>(brands);

            return View(model);
        }
    }
}
