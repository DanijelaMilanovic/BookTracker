using API.Controllers;
using Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Api
{
    public class MnozenjeTest
    {
        [Fact]
        public void Test()
        {
            var value = Mnozenje.mnozenje(1, 2);
            Assert.Equal(2, value);
        }
    }
}
