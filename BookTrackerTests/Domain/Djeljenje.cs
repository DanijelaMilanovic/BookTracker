using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTrackerTests.Domain
{
    public class Djeljenje
    {
        [Fact]
        public void Test()
        {
            var actual = Oduzmi.Sub(3, 2);
            Assert.Equal(1, actual);
        }
    }
}
