using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxo.DAL.Entities;

namespace Voxo.BLL.ViewModels
{
    public class OrderProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductImage> Images { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
