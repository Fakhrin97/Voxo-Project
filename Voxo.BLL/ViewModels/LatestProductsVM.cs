
using Voxo.DAL.Entities;

namespace Voxo.BLL.ViewModels
{
    public class LatestProductsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Published { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
        public byte DiscountDegree { get; set; }
        public bool IsFavori { get; set; }
        public List<ProductImage> Images { get; set; }
    }
}
