
namespace Voxo_Project.Areas.Admin.Controllers
{
    public class SlidersController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public SlidersController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var slider = await _dbContext.Sliders
                .Include(slider => slider.SliderImages)
                .FirstOrDefaultAsync();

            var sliderVM = _mapper.Map<SliderVM>(slider);

            return View(sliderVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill in the boxes completely");
                return View();
            }

            if (!model.CornerImage.IsImage() && !model.BackGroundImage.IsImage())
            {
                ModelState.AddModelError("", "Shekil Secmelisiz");
                return View();
            }

            if (!model.CornerImage.IsAllowedSize(10) && !model.BackGroundImage.IsAllowedSize(10))
            {
                ModelState.AddModelError("", "Shekilin olcusu 10 mbdan az omalidi");
                return View();
            }

            var unicalNameBGImage = await model.CornerImage.GenerateFile(Constants.SliderPath);
            var unicalNameCImage = await model.BackGroundImage.GenerateFile(Constants.SliderPath);

            var slider = new Slider
            {
                Name = model.Name,
                Title = model.Title,
                FreeDeliver = model.FreeDeliver,
                Price = model.Price,
                DiscountDegree = model.DiscountDegree,
                BackGroundImage = unicalNameBGImage,
                CornerImage = unicalNameCImage,
                CreatedAt = DateTime.Now,
            };

            var sliderImages = new List<SliderImages>();
            foreach (var image in model.SliderImages)
            {
                if (!image.IsImage())
                {
                    ModelState.AddModelError("", "Shekil Secmelisiz");
                    return View();
                }

                if (!image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("", "Shekilin olcusu 10 mbdan az omalidi");
                    return View();
                }

                var unicalName = await image.GenerateFile(Constants.SliderPath);

                sliderImages.Add(new SliderImages
                {
                    ImageUrl = unicalName,
                    SliderId = slider.Id,
                });

            }

            slider.SliderImages = sliderImages;

            await _dbContext.Sliders.AddAsync(slider);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var existSlider = await _dbContext.Sliders
                .Where(slider => slider.Id == id)
                .Include(x => x.SliderImages)
                .FirstOrDefaultAsync();

            var model = _mapper.Map<SliderUpdateVM>(existSlider);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill in the boxes completely");
                return View();
            }

            if (id == null) return NotFound();

            var existSlider = await _dbContext.Sliders
                .Where(slider => slider.Id == id)
                .Include(x => x.SliderImages)
            .FirstOrDefaultAsync();

            if (existSlider == null) return NotFound();

            if (model.BGImage != null)
            {
                if (!model.BGImage.IsImage())
                {
                    ModelState.AddModelError("BGImage", "Shekil Secmelisiz");
                    return View();
                }

                if (!model.BGImage.IsAllowedSize(10))
                {
                    ModelState.AddModelError("BGImage", "Shekilin olcusu 10 mbdan az omalidi");
                    return View();
                }

                var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-slider", existSlider.BackGroundImage);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.BGImage.GenerateFile(Constants.SliderPath);

                existSlider.BackGroundImage = unicalName;
            }

            if (model.CImage != null)
            {
                if (!model.CImage.IsImage())
                {
                    ModelState.AddModelError("CImage", "Shekil Secmelisiz");
                    return View();
                }

                if (!model.CImage.IsAllowedSize(10))
                {
                    ModelState.AddModelError("CImage", "Shekilin olcusu 10 mbdan az omalidi");
                    return View();
                }

                var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-slider", existSlider.CornerImage);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.CImage.GenerateFile(Constants.SliderPath);

                existSlider.CornerImage = unicalName;
            }

            if (model.SImages != null)
            {
                var sliderImages = new List<SliderImages>();

                foreach (var image in model.SImages)
                {
                    if (!image.IsImage())
                    {
                        ModelState.AddModelError("", "Shekil Secmelisiz");
                        return View();
                    }

                    if (!image.IsAllowedSize(10))
                    {
                        ModelState.AddModelError("", "Shekilin olcusu 10 mbdan az omalidi");
                        return View();
                    }                   

                    var unicalName = await image.GenerateFile(Constants.SliderPath);

                    sliderImages.Add(new SliderImages
                    {
                        ImageUrl = unicalName,
                        SliderId = existSlider.Id,
                    });
                }

                foreach (var item in existSlider.SliderImages)
                {
                    var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-slider", item.ImageUrl);

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                existSlider.SliderImages = sliderImages;
            }

            existSlider.Price = model.Price;    
            existSlider.Published = model.Published;
            existSlider.DiscountDegree = model.DiscountDegree;
            existSlider.FreeDeliver = model.FreeDeliver;    
            existSlider.Name = model.Name;    
            existSlider.Title = model.Title;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var existSlider = await _dbContext.Sliders
                .Where(slider => slider.Id == id)
                .Include(x => x.SliderImages)   
                .FirstOrDefaultAsync();

            if (existSlider == null) return NotFound();

            _dbContext.Sliders.Remove(existSlider);

            var pathBG = Path.Combine(Constants.RootPath, "assets", "images", "voxo-blog", existSlider.BackGroundImage);

            if (System.IO.File.Exists(pathBG))
                System.IO.File.Delete(pathBG);

            var pathCorner = Path.Combine(Constants.RootPath, "assets", "images", "voxo-blog", existSlider.CornerImage);

            if (System.IO.File.Exists(pathCorner))
                System.IO.File.Delete(pathCorner);

            foreach (var item in existSlider.SliderImages)
            {
                var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-blog", item.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
