using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxo.BLL.ViewModels
{
    public class CompareVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public byte DiscountDegree { get; set; }
        public string Cayegory { get; set; }
        public DateTime DateFirstAvailable { get; set; }
        public int Weight { get; set; }
        public decimal DiscountAmount => Price * (100 - DiscountDegree) / 100;
    }
}
