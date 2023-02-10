using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voxo.DAL.DataContext;
using Voxo.DAL.Entities;

namespace Voxo_Project.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public OrderController(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OrderProduct()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("LogIn", "Account");

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user is null) return BadRequest();

            var basket = await _dbContext
                    .Baskets
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.BasketProducts)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync();

            if (basket is null) return NotFound();

            _dbContext.Baskets.Remove(basket);

            var order = new Order
            {
                UserId = user.Id,
                CreateTime = DateTime.Now,
                OrderProducts = new List<OrderProduct>()
            };

            foreach (var product in basket.BasketProducts)
            {
                order.OrderProducts.Add(new OrderProduct
                {
                    ProductId = product.ProductId,
                    OrderId = order.Id,
                    Count = product.Count,
                });
            }
            
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
