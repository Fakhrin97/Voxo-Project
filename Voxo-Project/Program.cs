using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using University.DAL.Repositories;
using University.DAL.Repositories.Contracts;
using Voxo.BLL.Data;
using Voxo.BLL.Mapping;
using Voxo.BLL.Services;
using Voxo.BLL.Services.Contracts;
using Voxo.BLL.Services.MailService;
using Voxo.DAL.Data;
using Voxo.DAL.DataContext;
using Voxo.DAL.Entities;

namespace Voxo_Project
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Constants.RootPath = builder.Environment.WebRootPath;
            Constants.BrandPath = Path.Combine(Constants.RootPath, "assets", "images", "voxo-brand");
            Constants.FooterLogoPath = Path.Combine(Constants.RootPath, "assets", "images", "voxo-footer-logo");
            Constants.BlogPath = Path.Combine(Constants.RootPath, "assets", "images", "voxo-blog");
            Constants.CategoryPath = Path.Combine(Constants.RootPath, "assets", "images", "voxo-category");
            Constants.SliderPath = Path.Combine(Constants.RootPath, "assets", "images", "voxo-slider");
            Constants.ProductPath = Path.Combine(Constants.RootPath, "assets", "images", "voxo-product");

            // Add services to the container.
            builder.Services
                .AddControllersWithViews()
                .AddNewtonsoftJson
                (opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            builder.Services
              .AddDbContext<AppDbContext>(options =>
              {
                  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
              });

            //builder.Services.AddDbContext<AppDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            //        builder =>
            //        {
            //            builder.MigrationsAssembly(nameof(Voxo.DAL));
            //        });

            //});

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;

                options.SignIn.RequireConfirmedEmail = true;

                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.Configure<AdminUser>(builder.Configuration.GetSection("AdminUser"));           

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddTransient<IMailService, MailManager>();

            builder.Services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
            builder.Services.AddScoped<IBlogService, BlogManeger>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var dataInitalizer = new DataInitializer(serviceProvider);
                await dataInitalizer.SeedData();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithReExecute("/ErrorPages/Error", "?code={0}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}"
               );
            });

            await app.RunAsync();
        }
    }
}