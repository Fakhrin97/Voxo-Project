
namespace Voxo.DAL.Entities
{
    public class Category : TimeStample , IEntity
    {
        public int Id { get ; set ; }
        public bool Published { get ; set ; }
        public string Name { get ; set ; }
        public string ImageUrl { get ; set ; }
        public List<Product> Products { get; set; }
    }
}
