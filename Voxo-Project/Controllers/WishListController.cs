using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Voxo.BLL.Data;
using Voxo.BLL.ViewModels;
using Voxo.DAL.DataContext;
using Voxo.DAL.Entities;

namespace Voxo_Project.Controllers
{
    public class WishListController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public WishListController(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<WishListVM> model = new();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null) return BadRequest();

                var wishList = await _dbContext
                    .WishList
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.WishListProducts)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.Images)
                    .FirstOrDefaultAsync();

                foreach (var item in wishList.WishListProducts)
                {
                    model.Add(new WishListVM
                    {
                        Id = item.ProductId,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        DiscountDegree = item.Product.DiscountDegree,
                        ImageUrl = item.Product.Images.FirstOrDefault()?.Name,
                    });
                }

            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.WISH_LIST_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    foreach (var productId in productIdList)
                    {
                        var product = await _dbContext.Products
                            .Where(product => product.Id == productId && !product.Published)
                            .Include(product => product.Images)
                            .FirstOrDefaultAsync();

                        model.Add(new WishListVM
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Price = product.Price,
                            DiscountDegree = product.DiscountDegree,
                            ImageUrl = product.Images.FirstOrDefault()?.Name
                        });
                    }
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddProductToWishList(int? productId)
        {
            if (productId is null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var existProduct = await _dbContext.Products
                    .Where(product => product.Id == productId)
                    .FirstOrDefaultAsync();

                if (existProduct == null) return NotFound();

                if (user == null) return BadRequest();

                var existWishList = await _dbContext
                    .WishList
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.WishListProducts)
                    .FirstOrDefaultAsync();

                if (existWishList != null)
                {

                    var existFavoriProduct = existWishList.WishListProducts
                        .Where(x => x.ProductId == existProduct.Id)
                        .FirstOrDefault();

                    if (existFavoriProduct is not null)
                    {
                        existWishList.WishListProducts.Remove(existFavoriProduct);
                    }
                    else
                    {
                        var createdWishList = new WishList
                        {
                            UserId = user.Id,
                            WishListProducts = new List<WishListProduct>()
                        };

                        existWishList.WishListProducts.Add(new WishListProduct
                        {
                            WishListId = createdWishList.Id,
                            ProductId = existProduct.Id
                        });
                    }

                    _dbContext.Update(existWishList);
                }
                else
                {
                    var createdWishList = new WishList
                    {
                        UserId = user.Id,
                        WishListProducts = new List<WishListProduct>()
                    };

                    var wishListProducts = new List<WishListProduct>
                    {
                        new WishListProduct
                        {
                            WishListId = createdWishList.Id,
                            ProductId = existProduct.Id
                        }
                    };

                    createdWishList.WishListProducts = wishListProducts;

                    await _dbContext.WishList.AddAsync(createdWishList);
                }

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.WISH_LIST_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    if (productIdList.Contains(productId.Value))
                    {
                        productIdList.Remove(productId.Value);
                    }
                    else
                    {
                        productIdList.Add(productId.Value);
                    }

                    var productIdListJson = JsonConvert.SerializeObject(productIdList);

                    Response.Cookies.Append(Constants.WISH_LIST_COOKIE_NAME, productIdListJson);
                }
                else
                {
                    var productIdListJson = JsonConvert.SerializeObject(new List<int> { productId.Value });

                    Response.Cookies.Append(Constants.WISH_LIST_COOKIE_NAME, productIdListJson);
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductFromWishList(int? productId)
        {
            if (productId == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null) return BadRequest();

                var wishList = await _dbContext.WishList
                    .Where(x => x.UserId == user.Id)
                   .Include(x => x.WishListProducts)
                   .FirstOrDefaultAsync();

                var existProduct = await _dbContext.Products.FindAsync(productId);

                if (existProduct == null) return NotFound();

                var existWishListProduct = wishList.WishListProducts.FirstOrDefault(x => x.ProductId == existProduct.Id);

                wishList.WishListProducts.Remove(existWishListProduct);

                _dbContext.Update(wishList);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.WISH_LIST_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    productIdList.Remove(productId.Value);

                    var productIdListJson = JsonConvert.SerializeObject(productIdList);

                    Response.Cookies.Append(Constants.WISH_LIST_COOKIE_NAME, productIdListJson);
                }
            }

            return NoContent();
        }
    }
}
