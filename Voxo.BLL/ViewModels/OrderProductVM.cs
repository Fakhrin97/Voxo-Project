
namespace Voxo.BLL.ViewModels
{
    public class OrderProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductImage> Images { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
