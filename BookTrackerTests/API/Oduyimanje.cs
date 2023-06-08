using API.Controllers;
using Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTrackerTests.API
{
    public class Oduyimanje
    {
        [Fact]
        public void Test()
        {
            var value = Mnozenje.mnozenje(1, 2);
            Assert.Equal(2, value);
        }
    }
}
