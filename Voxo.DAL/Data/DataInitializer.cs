
namespace Voxo.DAL.Data
{
    public class DataInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _dbContext;
        private readonly AdminUser _adminUser;

        public DataInitializer(IServiceProvider serviceProvider)
        {
            _adminUser = serviceProvider.GetService<IOptions<AdminUser>>().Value;
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        }

        public async Task SeedData()
        {
            await _dbContext.Database.MigrateAsync();

            var roles = new List<string> { ConstantsData.AdminRole, ConstantsData.UserRole };

            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                    continue;

                await _roleManager.CreateAsync(new IdentityRole { Name = role });
            }

            var userExist = await _userManager.FindByNameAsync(_adminUser.Username);

            if (userExist is null )            
            {
                await _userManager.CreateAsync(new User
                {
                    Fristname = "aaaa",
                    Lastname = "bbbb",
                    UserName = _adminUser.Username,
                    Email = _adminUser.Email
                }, _adminUser.Password);

                var existUser = await _userManager.FindByNameAsync(_adminUser.Username);

                await _userManager.AddToRoleAsync(existUser, ConstantsData.AdminRole);
            }
        }
    }
}
