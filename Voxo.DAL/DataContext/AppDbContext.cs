using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Voxo.DAL.Entities;

namespace Voxo.DAL.DataContext
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<FooterLogo> FooterLogos { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderImages> SliderImages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<WishListProduct> WishListProduct { get; set; }
        public DbSet<Compare> Compares { get; set; }
        public DbSet<CompareProduct> CompareProducts { get; set; }
    }
}
