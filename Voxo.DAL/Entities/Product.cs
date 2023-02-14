
namespace Voxo.DAL.Entities
{
    public class Product : TimeStample, IEntity
    {
        public int Id { get ; set ; }
        public bool Published { get ; set ; }      
        public string Name { get ; set ; }  
        public string Description { get ; set ; }
        public int ViewersCount { get; set; }
        public int Weight { get; set ; }    
        public string Manufacturer‏ { get; set ; }
        public DateTime DateFirstAvailable { get; set; }    
        public int Rating { get; set; } 
        public decimal Price { get; set; }  
        public byte DiscountDegree { get; set; }
        public List<ProductImage> Images { get; set; }
        public int CategoryId { get; set; } 
        public Category Category { get; set; }
        public List<WishListProduct> WishListProducts { get; set; }
        public List<CompareProduct> CompareProducts { get; set; }

    }
}

