
using Voxo.DAL.Entities;

namespace Voxo.BLL.ViewModels
{
    public class BasketProductVM
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public List<ProductImage> Images { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
