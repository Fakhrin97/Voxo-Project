
namespace Voxo.DAL.Entities
{
    public class Basket :IEntity
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public string UserId { get; set; }
        public List<BasketProduct> BasketProducts { get; set; }
    }
}
