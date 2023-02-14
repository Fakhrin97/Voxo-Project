
namespace Voxo.BLL.ViewModels
{
    public class ProductCreateVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public string Manufacturer { get; set; }  
        public decimal Price { get; set; }
        public byte DiscountDegree { get; set; }
        public IFormFile[] Images { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public int CategoryId { get; set; }
    }
}
