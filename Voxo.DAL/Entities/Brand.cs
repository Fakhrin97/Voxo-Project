
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
