
namespace Voxo.DAL.Entities
{
    public class BasketProduct : IEntity
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        public int Count { get; set; }  
    }
}
