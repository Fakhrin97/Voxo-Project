using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxo.DAL.Base;

namespace Voxo.DAL.Entities
{
    public class CompareProduct : IEntity
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CompareId { get; set; }
        public Compare Compare { get; set; }
    }
}
