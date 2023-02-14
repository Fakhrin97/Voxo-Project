
namespace Voxo.DAL.Entities
{
    public class User : IdentityUser
    {
        public string Fristname { get; set; }
        public string Lastname { get; set; }
        public DateTime LastLogin { get; set; }

    }
}
