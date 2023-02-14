
namespace Voxo_Project.ViewComponents
{
    public class WishListViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public WishListViewComponent(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int productCount = 0;


            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var wishList = await _dbContext.WishList
                    .Where(x => x.UserId == user.Id)
                   .Include(x => x.WishListProducts)
                   .FirstOrDefaultAsync();

                if (wishList is not null)
                {
                    productCount = wishList.WishListProducts.ToList().Count();
                }

            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.WISH_LIST_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    productCount = productIdList.Count();
                }

            }

            return View(productCount);
        }
    }
}
