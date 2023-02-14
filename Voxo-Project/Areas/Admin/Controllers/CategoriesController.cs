
namespace Voxo_Project.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoriesController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var catigories = await _dbContext.Categories
               .ToListAsync();

            var catigoryVM = _mapper.Map<List<CategoryVM>>(catigories);

            return View(catigoryVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill in the boxes completely");
                return View();
            }

            var existCategory = await _dbContext.Brands
                .Where(category => category.Name == model.Name)
                .FirstOrDefaultAsync();

            if (existCategory != null)
            {
                ModelState.AddModelError("Name", "The category with this name is already available");
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

            var unicalName = await model.Image.GenerateFile(Constants.CategoryPath);

            await _dbContext.Categories.AddAsync(new Category
            {
                ImageUrl = unicalName,
                Name = model.Name,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var category = await _dbContext.Categories
                .Where(category => category.Id == id)
                .FirstOrDefaultAsync();

            if (category == null) return NotFound();

            var categoryUpdateVM = _mapper.Map<CategoryUpdateVM>(category);

            return View(categoryUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM model)
        {
            if (id == null) return NotFound();
           
            var existBCategory = await _dbContext.Categories
                .Where(category => category.Id == id)
            .FirstOrDefaultAsync();

            if (existBCategory == null) return NotFound();

            var viewModel = new CategoryUpdateVM
            {
                ImageUrl = existBCategory.ImageUrl,
            };

            if (!ModelState.IsValid) return View(viewModel);            

            if (existBCategory.Name != model.Name)
            {
                var sameNameCategory = await _dbContext.Categories
                .Where(category => category.Name == model.Name)
            .FirstOrDefaultAsync();

                if(sameNameCategory != null)
                {
                    ModelState.AddModelError("", "The category with this name is already available");
                    return View(viewModel);
                }
            }

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Shekil Secmelisiz");
                    return View(viewModel);
                }

                if (!model.Image.IsAllowedSize(10))
                {
                    ModelState.AddModelError("Image", "Shekilin olcusu 10 mbdan az omalidi");
                    return View(viewModel);
                }

                var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-category", existBCategory.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.Image.GenerateFile(Constants.CategoryPath);

                existBCategory.ImageUrl = unicalName;
            }

            existBCategory.Published = model.Published;
            existBCategory.Name = model.Name;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var existCategory = await _dbContext.Categories
                .Where(ctegory => ctegory.Id == id)
                .FirstOrDefaultAsync();

            if (existCategory == null) return NotFound();

            _dbContext.Categories.Remove(existCategory);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-category", existCategory.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
