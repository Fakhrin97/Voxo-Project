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
    public class CompareController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public CompareController(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<CompareVM> model = new();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null) return BadRequest();

                var compareList = await _dbContext
                    .Compares
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.CompareProducts)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x=>x.Images)
                    .Include(x => x.CompareProducts)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.Category)
                    .FirstOrDefaultAsync();

                if (compareList is not null)
                {
                foreach (var item in compareList.CompareProducts)
                {
                    model.Add(new CompareVM
                    {
                        Id = item.ProductId,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        Cayegory= item.Product.Category.Name,
                        Weight = item.Product.Weight,
                        DateFirstAvailable= item.Product.DateFirstAvailable,    
                        DiscountDegree = item.Product.DiscountDegree,
                        ImageUrl = item.Product.Images.FirstOrDefault()?.Name,
                    });
                }
                }
            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.COMPARE_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    foreach (var productId in productIdList)
                    {
                        var product = await _dbContext.Products
                            .Where(product => product.Id == productId && !product.Published)
                            .Include(product => product.Images)
                            .Include(product=>product.Category)
                            .FirstOrDefaultAsync();

                        model.Add(new CompareVM
                        {
                            Id = product.Id,
                            Weight = product.Weight,
                            Name = product.Name,
                            Price = product.Price,
                            DiscountDegree = product.DiscountDegree,
                            ImageUrl = product.Images.FirstOrDefault()?.Name,
                            Cayegory = product.Category.Name,
                            DateFirstAvailable = product.DateFirstAvailable,
                           
                        });
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToCompare(int? productId)
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

                var existCompareList = await _dbContext
                    .Compares
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.CompareProducts)
                    .FirstOrDefaultAsync();

                if (existCompareList != null)
                {

                    var existCompareProduct = existCompareList.CompareProducts
                        .Where(x => x.ProductId == existProduct.Id)
                        .FirstOrDefault();

                    if (existCompareProduct is not null)
                    {
                        existCompareList.CompareProducts.Remove(existCompareProduct);
                    }
                    else
                    {
                        var createCopareList = new Compare
                        {
                            UserId = user.Id,
                            CompareProducts = new List<CompareProduct>()
                        };

                        existCompareList.CompareProducts.Add(new CompareProduct
                        {
                            CompareId = createCopareList.Id,
                            ProductId = existProduct.Id
                        });
                    }

                    _dbContext.Update(existCompareList);
                }
                else
                {
                    var createdCompareList = new Compare
                    {
                        UserId = user.Id,
                        CompareProducts = new List<CompareProduct>()
                    };

                    var compareListProducts = new List<CompareProduct>
                    {
                        new CompareProduct
                        {
                            CompareId = createdCompareList.Id,
                            ProductId = existProduct.Id
                        }
                    };

                    createdCompareList.CompareProducts = compareListProducts;

                    await _dbContext.Compares.AddAsync(createdCompareList);
                }

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.COMPARE_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    if (productIdList.Contains(productId.Value))
                        return NoContent();

                    productIdList.Add(productId.Value);                   

                    var productIdListJson = JsonConvert.SerializeObject(productIdList);

                    Response.Cookies.Append(Constants.COMPARE_COOKIE_NAME, productIdListJson);
                }
                else
                {
                    var productIdListJson = JsonConvert.SerializeObject(new List<int> { productId.Value });

                    Response.Cookies.Append(Constants.COMPARE_COOKIE_NAME, productIdListJson);
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductFromCompare(int? productId)
        {
            if (productId == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null) return BadRequest();

                var compareList = await _dbContext.Compares
                    .Where(x => x.UserId == user.Id)
                   .Include(x => x.CompareProducts)
                   .FirstOrDefaultAsync();

                var existProduct = await _dbContext.Products.FindAsync(productId);

                if (existProduct == null) return NotFound();

                var existCompareListProduct = compareList.CompareProducts.FirstOrDefault(x => x.ProductId == existProduct.Id);

                compareList.CompareProducts.Remove(existCompareListProduct);

                _dbContext.Update(compareList);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.COMPARE_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    productIdList.Remove(productId.Value);

                    var productIdListJson = JsonConvert.SerializeObject(productIdList);

                    Response.Cookies.Append(Constants.COMPARE_COOKIE_NAME, productIdListJson);
                }
            }

            return NoContent();
        }
    }
}
