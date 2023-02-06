using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Voxo.BLL.Data;
using Voxo.BLL.ViewModels;
using Voxo.DAL.DataContext;

namespace Voxo_Project.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public BasketViewComponent(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           

            //var model = _mapper.Map<List<BlogPageVM>>(blogs);
            List<BasketProductVM> model = new List<BasketProductVM>();

            if (User.Identity.IsAuthenticated) { }
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

                        BasketProductVM productmodel = new BasketProductVM();

                        if (product is not null)
                        {
                            productmodel.Id = product.Id;
                            productmodel.Name = product.Name;
                            productmodel.Count = item.Count;
                            productmodel.Images = product.Images;
                            productmodel.Price = product.Price;
                        }
                        model.Add(productmodel);
                    }

                }
            }

            return View(model);
        }
    }
}
