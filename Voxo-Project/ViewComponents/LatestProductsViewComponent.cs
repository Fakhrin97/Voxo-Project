
namespace Voxo_Project.ViewComponents
{
    public class LatestProductsViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public LatestProductsViewComponent(AppDbContext dbContext, IMapper mapper, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var latestProducts = await _dbContext.Products
                .Where(product => !product.Published)
                .OrderByDescending(p => p.CreatedAt)
                .Take(6)
                .Include(p => p.Images)
                .ToListAsync();

            var model = _mapper.Map<List<LatestProductsVM>>(latestProducts);

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
                        var product = model.Where(p => p.Id == products.ProductId).FirstOrDefault();

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
                        var product = model.Where(p => p.Id == productId).FirstOrDefault();

                        if (product is not null)
                        {
                            product.IsFavori = true;
                        }
                        
                    }

                }
            }

            return View(model);
        }
    }
}
