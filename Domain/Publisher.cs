using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Publisher
    {
        public Guid PublisherId { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
    }
}