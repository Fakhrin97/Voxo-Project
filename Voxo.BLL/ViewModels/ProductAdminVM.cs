namespace Voxo.BLL.ViewModels
{
    public class ProductAdminVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Published { get; set; }
        public List<ProductImage> Images { get; set; }
    }
}



