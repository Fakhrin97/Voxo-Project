
namespace Voxo.BLL.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public DateTime LastLogin { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get ; set; }
        public string Email { get; set; }
        public string Role { get; set; }    
    }
}
