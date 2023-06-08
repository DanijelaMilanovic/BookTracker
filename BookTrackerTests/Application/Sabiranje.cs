using Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTrackerTests.Application
{
    public class Sabiranje
    {

        [Fact]
        public void Test()
        {
            var value = Saberi.Add(1, 2);
            Assert.Equal(4, value);
        }
    }
}
