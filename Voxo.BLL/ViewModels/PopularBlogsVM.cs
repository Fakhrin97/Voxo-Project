using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxo.BLL.ViewModels
{
    public class PopularBlogsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ViewersCount { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
