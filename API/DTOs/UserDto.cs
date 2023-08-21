using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserDto
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }

    }
}