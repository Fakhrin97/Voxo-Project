using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voxo.BLL.ViewModels;
using Voxo.DAL.DataContext;
using Voxo.DAL.Entities;

namespace Voxo_Project.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public OrdersController(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _dbContext.Orders
                .Include(order => order.OrderProducts)
                .ThenInclude(order => order.Product)
                .ToListAsync();
           
            var model = new List<OrderVM>();

            if(orders is not null)
            {
                foreach (var order in orders)
                {
                    var user = await _userManager.FindByIdAsync(order.UserId);
                    decimal amount = 0;
                    foreach (var product in order.OrderProducts)
                    {
                        amount = amount +(product.Count * product.Product.Price);
                    }

                    model.Add(new OrderVM
                    {
                        Name = user.UserName,
                        Id = order.Id,   
                        Time = order.CreateTime,
                        Amount = amount,
                        Status = order.Status,  
                    });                    
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _dbContext.Orders.Where(order=>order.Id == id)
              .Include(order => order.OrderProducts)
              .ThenInclude(order => order.Product)
              .FirstOrDefaultAsync();

            List<OrderProductVM> model = new();

            foreach (var item in order.OrderProducts)
            {
                var product = _dbContext.Products
                            .Where(p => p.Id == item.Product.Id)
                            .Include(x => x.Images)
                            .FirstOrDefault();

                model.Add(new OrderProductVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Count = item.Count,
                    Images = product.Images,
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _dbContext.Orders.Where(order => order.Id == orderId)             
             .FirstOrDefaultAsync();

             _dbContext.Orders.Remove(order);  

            await _dbContext.SaveChangesAsync();


            return NoContent();
        }

    }
}
