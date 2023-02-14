
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
