
namespace Voxo_Project.Areas.Admin.Controllers
{
    public class FooterLogosController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public FooterLogosController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var footeLogos = await _dbContext.FooterLogos
                .ToListAsync();

            var footeLogosVM = _mapper.Map<List<FooterLogoVM>>(footeLogos);

            return View(footeLogosVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FooterLogoCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill in the boxes completely");
                return View();
            }

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Shekil Secmelisiz");
                return View();
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "Shekilin olcusu 10 mbdan az omalidi");
                return View();
            }

            var unicalName = await model.Image.GenerateFile(Constants.FooterLogoPath);

            await _dbContext.FooterLogos.AddAsync(new FooterLogo
            {
                ImageUrl = unicalName,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var footerlogo = await _dbContext.FooterLogos
                .Where(logo => logo.Id == id)
                .FirstOrDefaultAsync();

            if (footerlogo == null) return NotFound();

            var footerlogoUpdateVM = _mapper.Map<FooterLogoUpdateVM>(footerlogo);

            return View(footerlogoUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, FooterLogoUpdateVM model)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return NotFound();

            var existFooterLogo = await _dbContext.FooterLogos
                .Where(logo => logo.Id == id)
                .FirstOrDefaultAsync();

            if (existFooterLogo == null) return NotFound();

            existFooterLogo.Published = model.Published;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var existFooterLogo = await _dbContext.FooterLogos
                .Where(logo => logo.Id == id)
                .FirstOrDefaultAsync();

            if (existFooterLogo == null) return NotFound();

            _dbContext.FooterLogos.Remove(existFooterLogo);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-footer-logo", existFooterLogo.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
