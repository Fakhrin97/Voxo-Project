using Voxo.DAL.Entities;

namespace Voxo.BLL.ViewModels
{
    public class SliderHomeVM
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public bool FreeDeliver { get; set; }
        public decimal Price { get; set; }
        public byte DiscountDegree { get; set; }
        public string BackGroundImage { get; set; }
        public string CornerImage { get; set; }
        public List<SliderImages> SliderImages { get; set; }
    }
}
