using Microsoft.AspNetCore.Http;

namespace Voxo.BLL.ViewModels
{
    public class BrandUpdateVM
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public bool Published { get; set; }
        public IFormFile? Image { get; set; }
    }
}
