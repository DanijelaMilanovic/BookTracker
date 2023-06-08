using Domen;

namespace Tests.Domen
{
    public class OduzmiTest
    {
        [Fact]
        public void Test()
        {
            var actual = Oduzmi.Sub(3, 2);
            Assert.Equal(1, actual);
        }
    }
}
