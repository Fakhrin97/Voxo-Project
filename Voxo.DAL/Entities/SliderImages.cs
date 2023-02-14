
namespace Voxo.DAL.Entities
{
    public class SliderImages : TimeStample, IEntity
    {
        public int Id { get ; set ; }
        public bool Published { get ; set ; }
        public string ImageUrl {  get ; set ; } 
        public Slider Slider { get; set ; }
        public int SliderId { get; set ; }    

    }
}
