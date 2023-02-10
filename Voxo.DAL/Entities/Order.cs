
using Voxo.DAL.Base;

namespace Voxo.DAL.Entities
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
        public DateTime CreateTime { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
