using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxo.BLL.ViewModels
{
    public class ShopPageVM
    {
        public List<ProductVM> Products { get; set; }
        public List<CategoryShopVM> Categories { get; set; }
        public List<MostPapularProduct> MostPapularProducts { get; set; }
    }
}
