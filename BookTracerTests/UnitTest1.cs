using Application;
namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var value = Saberi.Add(1, 2);
            Assert.Equal(3, value);
        }
    }
}