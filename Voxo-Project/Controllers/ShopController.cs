
namespace Voxo_Project.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ShopController(AppDbContext dbContext, IMapper mapper, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? categoryId, int page = 1)
        {
            var productList = new List<Product>();

            if (categoryId is not null)
            {
                productList = await _dbContext.Products
                     .Where(product => product.CategoryId == categoryId && !product.Published)
                     .Include(p => p.Category)
                     .Include(product => product.Images)
                     .ToListAsync();
            }
            else
            {
                productList = await _dbContext.Products
                    .Where(product => !product.Published)
                    .Include(product => product.Images)
                    .Include(p => p.Category)
                    .ToListAsync();
            }

            var productVM = _mapper.Map<List<ProductVM>>(productList);

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var wishList = await _dbContext
                    .WishList
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.WishListProducts)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.Images)
                    .FirstOrDefaultAsync();

                if (wishList is not null)
                {
                    foreach (var products in wishList.WishListProducts)
                    {
                        var product = productVM.Where(p => p.Id == products.ProductId).FirstOrDefault();

                        if (product is not null)
                        {
                            product.IsFavori = true;
                        }

                    }
                }

            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.WISH_LIST_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    foreach (var productId in productIdList)
                    {
                        var product = productVM.Where(p => p.Id == productId).FirstOrDefault();

                        if (product is not null)
                        {
                            product.IsFavori = true;
                        }

                    }

                }
            }

            var model = new ShopPageVM();

            int perPage = 4;
            int pageCount = (int)Math.Ceiling((double)productVM.Count() / perPage);

            if (page <= 0) page = 1;
            if (page > pageCount) page = pageCount;

            ViewBag.CurrentPage = page;
            ViewBag.PageCount = pageCount;

            var selectedProduct = productVM.Skip((page - 1) * perPage).Take(perPage).ToList();
            model.Products = selectedProduct;

            var catigories = await _dbContext.Categories
                .Where(c => !c.Published)
                .ToListAsync();

            var categorieVM = _mapper.Map<List<CategoryShopVM>>(catigories);
            model.Categories = categorieVM;

            productList = await _dbContext.Products
                   .Where(product => !product.Published)
                   .OrderByDescending(x => x.ViewersCount)
                   .Take(6)
                   .Include(p => p.Category)
                   .Include(product => product.Images)
                   .ToListAsync();

            var mostPapularProducts = _mapper.Map<List<MostPapularProduct>>(productList);
            model.MostPapularProducts = mostPapularProducts;

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _dbContext.Products
                .Where(product => product.Id == id && !product.Published)
                .Include(product => product.Images)
                .Include(product => product.Category)
                .FirstOrDefaultAsync();

            if (product is null) return NotFound();

            product.ViewersCount++;

            _dbContext.SaveChanges();

            var model = _mapper.Map<ProductDetailsVM>(product);
            model.Categoryname = product.Category.Name;

            return View(model);
        }

        public async Task<IActionResult> Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return NoContent();

            var products = await _dbContext.Products
                .Where(course => !course.Published && course.Name.ToLower().Contains(searchText.ToLower()))
                .Include(product => product.Images)
                .ToListAsync();

            var model = new List<ProductVM>();

            products.ForEach(product => model.Add(new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Images = product.Images,
                Price = product.Price,
            }));

            return PartialView("_ProductListPartial", model);
        }
    }
}
