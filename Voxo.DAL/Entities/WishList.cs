
namespace Voxo.DAL.Entities
{
    public class WishList : IEntity
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public string UserId { get; set; }  
        public List<WishListProduct> WishListProducts { get; set; } 
    }
}
