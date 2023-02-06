using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxo.BLL.ViewModels
{
    public class UserUpdateVM
    {
        public string Id { get; set; }
        public bool EmailConfirmed { get; set; }  
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string CurrentRole { get; set; }
        public string Role { get; set; }
        public List<SelectListItem>? Roles { get; set; }
    }
}
