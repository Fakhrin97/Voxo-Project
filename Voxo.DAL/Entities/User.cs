using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxo.DAL.Entities
{
    public class User : IdentityUser
    {
        public string Fristname { get; set; }
        public string Lastname { get; set; }
        public DateTime LastLogin { get; set; }

    }
}
