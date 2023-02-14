
namespace Voxo_Project.ViewComponents
{
    public class FooterLogoViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public FooterLogoViewComponent(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerLogos = await _dbContext.FooterLogos.Where(logo => !logo.Published)
            .ToListAsync();

            var model = _mapper.Map<List<FooterLogoImgVM>>(footerLogos);

            return View(model);
        }
    }
}
