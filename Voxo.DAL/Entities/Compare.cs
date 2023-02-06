
using Voxo.DAL.Base;

namespace Voxo.DAL.Entities
{
    public class Compare : IEntity
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public string UserId { get; set; }
        public List<CompareProduct> CompareProducts { get; set; }
    }
}
