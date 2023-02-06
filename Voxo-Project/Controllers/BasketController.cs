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
    public class BasketController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public BasketController(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<BasketProductVM> model = new();


            if (User.Identity.IsAuthenticated)
            {
                
            }
            else
            {
                if(Request.Cookies.TryGetValue(Constants.BASKET_COOKIE_NAME, out var cookie))
                {
                    var productList = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);

                    foreach (var item in productList)
                    {
                        var product =  _dbContext.Products
                            .Where(p=>p.Id == item.Id && !p.Published)
                            .Include(x=>x.Images)
                            .FirstOrDefault();    
                            

                        model.Add(new BasketProductVM
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Price = product.Price,
                            Count = item.Count,
                            Images = product.Images,
                        });
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(int? productId)
        {
            if (productId is null) return NotFound();

            var product = await _dbContext.Products
                   .Where(product => product.Id == productId)
                   .FirstOrDefaultAsync();

            if (product is null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {

            }
            else
            {
                var basket = Request.Cookies[Constants.BASKET_COOKIE_NAME];
                var basketItems = new List<BasketVM>();

                var basketItem = new BasketVM
                {
                    Id = product.Id,
                    Count = 1,
                };

                if (basket is not null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

                    var existProduct = basketItems
                        .Where(x => x.Id == product.Id)
                        .FirstOrDefault();

                    if (existProduct is not null) existProduct.Count++;
                    else basketItems.Add(basketItem);
                }
                else
                {
                    basketItems.Add(basketItem);
                }

                Response.Cookies.Append(Constants.BASKET_COOKIE_NAME, JsonConvert.SerializeObject(basketItems));
            }

            return NoContent();
        }

        [HttpPost]
        public  IActionResult ChangeProductQuality(int? productId , int count)
        {
            if (productId is null) return NotFound();

            if(User.Identity.IsAuthenticated)
            { 
            }
            else
            {

                if (Request.Cookies.TryGetValue(Constants.BASKET_COOKIE_NAME, out var cookie))
                {
                    var productList = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);

                    var existProduct = productList.Where(x => x.Id == productId).FirstOrDefault();

                    existProduct.Count = count;

                    var productIdListJson = JsonConvert.SerializeObject(productList);

                    Response.Cookies.Append(Constants.BASKET_COOKIE_NAME, productIdListJson);
                }
            }

            return NoContent();
        }

        [HttpPost]
        public IActionResult DeleteProductBasket(int? productId)
        {
            if (productId == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                
            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.BASKET_COOKIE_NAME, out var cookie))
                {
                    var productList = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);

                    var existProduct = productList.Where(x=>x.Id== productId).FirstOrDefault();

                    productList.Remove(existProduct);

                    var productIdListJson = JsonConvert.SerializeObject(productList);

                    Response.Cookies.Append(Constants.BASKET_COOKIE_NAME, productIdListJson);
                }
            }

            return NoContent();
        }
    }
}
