
namespace Voxo_Project.Areas.Admin.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductsController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _dbContext.Products
                .Include(product => product.Images)
                .ToListAsync();

            var productsVM = new List<ProductAdminVM>();

            products.ForEach(x =>
            {
                productsVM.Add(new ProductAdminVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Published = x.Published,
                    Images = x.Images,
                });
            });

            //var productsVM = _mapper.Map<ProductAdminVM>(products);             

            return View(productsVM);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _dbContext.Categories
                .Where(category => !category.Published)
                .ToListAsync();

            var categoriesSlectListItem = new List<SelectListItem>();

            categories.ForEach(category =>
            {
                categoriesSlectListItem.Add(new SelectListItem(category.Name, category.Id.ToString()));
            });

            var model = new ProductCreateVM
            {
                Categories = categoriesSlectListItem
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            var catigories = await _dbContext.Categories
                .Where(category => !category.Published)
                .ToListAsync();

            var catigoriesSlectListItem = new List<SelectListItem>();

            catigories.ForEach(category =>
            {
                catigoriesSlectListItem.Add(new SelectListItem(category.Name, category.Id.ToString()));
            });

            var viewModel = new ProductCreateVM
            {
                Categories = catigoriesSlectListItem
            };

            if (!ModelState.IsValid) return View(viewModel);

            var existProduct = await _dbContext.Products
                .Where(product => product.Name == model.Name)
                .FirstOrDefaultAsync();

            if (existProduct != null)
            {
                ModelState.AddModelError("Name", "The product with the same name is already available");
                return View(viewModel);
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Weight = model.Weight,
                Manufacturer = model.Manufacturer,
                Price = model.Price,
                DiscountDegree = model.DiscountDegree,
                DateFirstAvailable = DateTime.Now,
            };

            var productImages = new List<ProductImage>();

            foreach (var image in model.Images)
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

                var unicalName = await image.GenerateFile(Constants.ProductPath);

                productImages.Add(new ProductImage
                {
                    Name = unicalName,
                    ProductId = product.Id,
                });
            }

            var selectedCategory = catigories.Where(x => x.Id == model.CategoryId).FirstOrDefault();

            if (selectedCategory == null) return BadRequest();

            product.Images = productImages;
            product.Category = selectedCategory;

            await _dbContext.Products.AddAsync(product);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();

            var product = await _dbContext.Products
                .Where(p => p.Id == id)
                .Include(p => p.Images)
                .FirstOrDefaultAsync();

            if (product is null) return NotFound();

            var categories = await _dbContext.Categories
                .Where(category => !category.Published)
                .ToListAsync();

            var catigoriesSlectListItem = new List<SelectListItem>();

            categories.ForEach(category =>
            {
                catigoriesSlectListItem.Add(new SelectListItem(category.Name, category.Id.ToString()));
            });

            var productUpdateVM = new ProductUpdateVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Manufacturer = product.Manufacturer,
                Weight = product.Weight,
                Price = product.Price,
                DiscountDegree = product.DiscountDegree,
                ProductImages = product.Images,
                Categories = catigoriesSlectListItem,
                CategoryId = product.CategoryId,
                Published = product.Published,
            };

            return View(productUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM model)
        {
            if (id is null) return BadRequest();

            var existProduct = await _dbContext.Products
                .Where(p => p.Id == id)
                .Include(p => p.Images)
                .FirstOrDefaultAsync();

            if (existProduct is null) return NotFound();

            var categories = await _dbContext.Categories
                .Where(category => !category.Published)
                .ToListAsync();

            var catigoriesSlectListItem = new List<SelectListItem>();

            categories.ForEach(category =>
            {
                catigoriesSlectListItem.Add(new SelectListItem(category.Name, category.Id.ToString()));
            });

            var productUpdateVM = new ProductUpdateVM
            {
                Id = existProduct.Id,
                ProductImages = existProduct.Images,
                Categories = catigoriesSlectListItem,
                CategoryId = existProduct.CategoryId,
            };

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill in the boxes completely");
                return View(productUpdateVM);
            };

            if (model.Name != existProduct.Name)
            {
                var sameNameProduct = await _dbContext.Products
                    .Where(p => p.Name == model.Name)
                    .FirstOrDefaultAsync();

                if (sameNameProduct is not null)
                {
                    ModelState.AddModelError("Name", "The product with the same name is already available");
                    return View(productUpdateVM);
                };
            };

            if (model.RemovedImagesIds is not null)
            {
                var removedImagesIds = model.RemovedImagesIds
                    .Split(",")
                    .ToList()
                    .Select(imagesId => Int32.Parse(imagesId));

                var removeImages = await _dbContext.ProductImages.Where(img => removedImagesIds.Contains(img.Id)).ToListAsync();

                _dbContext.ProductImages.RemoveRange(removeImages);

                foreach (var productImage in removeImages)
                {
                    var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-product", productImage.Name);

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                };

            };

            var productImages = new List<ProductImage>();

            if (model.Images is not null)
            {

                foreach (var image in model.Images)
                {
                    if (!image.IsImage())
                    {
                        ModelState.AddModelError("", "Shekil Secmelisiz");
                        return View(productUpdateVM);
                    };

                    if (!image.IsAllowedSize(10))
                    {
                        ModelState.AddModelError("", "Shekilin olcusu 10 mbdan az omalidi");
                        return View(productUpdateVM);
                    };

                    var unicalName = await image.GenerateFile(Constants.ProductPath);

                    productImages.Add(new ProductImage
                    {
                        Name = unicalName,
                        ProductId = existProduct.Id,
                    });
                };

                existProduct.Images.AddRange(productImages);
            };

            var selectedCategory = await _dbContext.Categories
                .Where(x => x.Id == model.CategoryId)
                .FirstOrDefaultAsync();

            if (selectedCategory == null) return BadRequest();

            existProduct.Category = selectedCategory;
            existProduct.Weight = model.Weight; 
            existProduct.Manufacturer = model.Manufacturer;
            existProduct.Description = model.Description;
            existProduct.DiscountDegree = model.DiscountDegree;
            existProduct.Price = model.Price;
            existProduct.Published = model.Published;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var existProduct = await _dbContext.Products
                .Where(product => product.Id == id)
                .Include(x => x.Images)
                .FirstOrDefaultAsync();

            if (existProduct == null) return NotFound();

            _dbContext.Products.Remove(existProduct);

            foreach (var item in existProduct.Images)
            {
                var path = Path.Combine(Constants.RootPath, "assets", "images", "voxo-product", item.Name);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
