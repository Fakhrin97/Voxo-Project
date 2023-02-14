
namespace Voxo.DAL.Entities
{
    public class FooterLogo : TimeStample, IEntity
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public string ImageUrl { get; set; }
    }
}
