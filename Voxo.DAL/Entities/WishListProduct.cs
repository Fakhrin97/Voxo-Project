
using Voxo.DAL.Base;

namespace Voxo.DAL.Entities
{
    public class WishListProduct : IEntity
    {
        public int Id { get ; set; }
        public bool Published { get; set ; }
        public int ProductId { get; set; }  
        public Product Product { get; set; }
        public int WishListId { get; set; } 
        public WishList WishList { get; set; }
    }
}
