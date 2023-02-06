using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxo.DAL.Base;

namespace Voxo.DAL.Entities
{
    public class Slider : TimeStample, IEntity
    {
        public int Id { get ; set ; }
        public string Name { get; set ; }
        public string Title { get; set ; }
        public bool Published { get ; set ; }
        public bool FreeDeliver { get ; set ; }
        public decimal Price { get ; set ; }
        public byte DiscountDegree { get ; set ; }
        public string BackGroundImage { get; set; }
        public string CornerImage { get; set; }
        public List<SliderImages> SliderImages { get; set; }

    }
}
