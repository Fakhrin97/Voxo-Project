using Microsoft.AspNetCore.Http;


namespace Voxo.BLL.ViewModels
{
    public class SliderCreateVM
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public bool FreeDeliver { get; set; }
        public decimal Price { get; set; }
        public byte DiscountDegree { get; set; }
        public IFormFile BackGroundImage { get; set; }
        public IFormFile CornerImage { get; set; }
        public IFormFile[] SliderImages { get; set; }
    }
}
