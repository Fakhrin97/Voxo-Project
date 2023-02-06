using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxo.DAL.Entities;

namespace Voxo.BLL.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<ProductImage> Images { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public byte DiscountDegree { get; set; }
        public int Rating { get; set; }
        public bool IsFavori { get; set; }
    }
}
