using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Voxo.BLL.Data;
using Voxo.BLL.ViewModels;
using Voxo.DAL.DataContext;
using Voxo.DAL.Entities;

namespace Voxo_Project.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public BasketViewComponent(AppDbContext dbContext, IMapper mapper, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketProductVM> model = new List<BasketProductVM>();
            BasketProductVM productModel = new BasketProductVM();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user is not null)
                {
                    var basket = await _dbContext
                    .Baskets
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.BasketProducts)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.Images)
                    .FirstOrDefaultAsync();

                    if (basket is not null)
                    {
                        foreach (var item in basket.BasketProducts)
                        {
                            var product = _dbContext.Products
                                .Where(p => p.Id == item.ProductId && !p.Published)
                                .Include(x => x.Images)
                                .FirstOrDefault();

                            if (product is not null)
                            {
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

                }
            }
            else
            {
                var basket = Request.Cookies[Constants.BASKET_COOKIE_NAME];
                var basketItems = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

                if (basketItems is not null)
                {
                    foreach (var item in basketItems)
                    {
                        var product = await _dbContext.Products
                                .Where(p => p.Id == item.Id)
                                .Include(p => p.Images)
                                .FirstOrDefaultAsync();

                        if (product is not null)
                        {
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
            }

            return View(model);
        }
    }
}
