
using Microsoft.AspNetCore.Http;

namespace Voxo.BLL.ViewModels
{
    public class BlogUpdateVM
    {
        public bool Published { get; set; }
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile? Image { get; set; }

    }
}
