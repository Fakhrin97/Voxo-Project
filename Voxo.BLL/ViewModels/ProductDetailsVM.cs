using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxo.DAL.Entities;

namespace Voxo.BLL.ViewModels
{
    public class ProductDetailsVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ViewersCount { get; set; }
        public int Weight { get; set; }
        public string Manufacturer‏ { get; set; }
        public DateTime DateFirstAvailable { get; set; }
        public decimal Price { get; set; }
        public byte DiscountDegree { get; set; }
        public List<ProductImage> Images { get; set; }
        public string Categoryname { get; set; }    
    }
}
