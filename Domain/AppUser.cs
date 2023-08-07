using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String Image { get; set; }
        
    }
}