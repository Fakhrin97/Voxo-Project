

namespace Voxo.BLL.ViewModels
{
    public class CategoryUpdateVM
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public bool Published { get; set; }
        public IFormFile? Image { get; set; }
    }
}
