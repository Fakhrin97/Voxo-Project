using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Voxo.DAL.Entities;

namespace Voxo.BLL.ViewModels
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public string Manufacturer { get; set; }  
        public decimal Price { get; set; }
        public byte DiscountDegree { get; set; }
        public string? RemovedImagesIds { get; set; }   
        public List<ProductImage>? ProductImages { get; set; }   
        public IFormFile[]? Images { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public int CategoryId { get; set; }
    }
}
