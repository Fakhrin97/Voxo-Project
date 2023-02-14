
namespace Voxo_Project.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public SliderViewComponent(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slider = await _dbContext.Sliders
                .Where(slider => !slider.Published)
                .Include(slider => slider.SliderImages)
            .FirstOrDefaultAsync();

            var model = _mapper.Map<SliderHomeVM>(slider);
           
            return View(model);
        }
    }
}
