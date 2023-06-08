using API.Controllers;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Persistence
{
    public class DjeljenjeTest
    {
        [Fact]
        public void Test()
        {
            var value = Djeljenje.djeljenje(4, 2);
            Assert.Equal(2, value);
        }
    }
}
