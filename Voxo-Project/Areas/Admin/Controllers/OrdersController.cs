
namespace Voxo_Project.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;

        public OrdersController(AppDbContext dbContext, UserManager<User> userManager, IMailService mailService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mailService = mailService;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _dbContext.Orders.Where(order => order.Id == id)             
             .FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(order.UserId);

            var requestEmail = new RequestEmail
            {
                ToEmail = user.Email,
                Body = $"Sizin {order.Id}-nömrəli sorğunuz ləgv olundu",
                Subject = $"Hi {user.UserName}",
            };

            await _mailService.SendEmailAsync(requestEmail);

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var order = await _dbContext.Orders.Where(order => order.Id == id)
             .FirstOrDefaultAsync();

            var user = await _userManager.FindByIdAsync(order.UserId);

            var requestEmail = new RequestEmail
            {
                ToEmail = user.Email,
                
                Subject = $"Hi {user.UserName}",
            };

            if (order.Status == false)
            {
                order.Status = true;
                requestEmail.Body = $"Sizin {order.Id}-nömrəli sorğunuz qebul olundu , sifarişiniz ən qısa zamanda çatdırılacaq.";
            }
            else
            {
                order.Status = false;
                requestEmail.Body =$"Sizin {order.Id}-nömrəli sifarişinizdə problem yaranib, zəhmet olmasa asdfgh@gmail.com adresi ilə əlaqə saxlayın.";
            }

            await _mailService.SendEmailAsync(requestEmail);

            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
