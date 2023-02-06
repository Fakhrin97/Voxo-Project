using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxo.DAL.Base;

namespace Voxo.DAL.Entities
{
    public class Brand : TimeStample, IEntity
    {
        public int Id { get ; set ; }
        public string Name { get ; set ; }
        public string ImageUrl { get ; set ; }
        public bool Published { get ; set ; }
    }
}
