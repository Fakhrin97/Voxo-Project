using Microsoft.AspNetCore.Http;


namespace Voxo.BLL.ViewModels
{
    public class BrandCreateVM
    {
        public IFormFile Image { get; set; }
        public string Name { get; set; }
    }
}
