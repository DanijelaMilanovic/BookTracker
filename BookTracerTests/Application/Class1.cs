using Application;

namespace Tests.Application
{
    public class Class1
    {
        [Fact]
        public void Test () {
            var value = Saberi.Add(1, 2);
            Assert.Equal(3, value);
        }

    }
}
