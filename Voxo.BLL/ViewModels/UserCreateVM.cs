
namespace Voxo.BLL.ViewModels
{
    public class UserCreateVM
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public List<SelectListItem>? Roles { get; set; }
        public string Password { get; set; }      
        public string ConfirmPassword { get; set; }
    }
}
