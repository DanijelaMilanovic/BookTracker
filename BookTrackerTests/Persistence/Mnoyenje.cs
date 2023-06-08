using Persistence;

namespace BookTrackerTests.Persistence
{
    public class Mnoyenje
    {
        [Fact]
        public void Test()
        {
            var value = Djeljenje.djeljenje(4, 2);
            Assert.Equal(2, value);
        }
    }
}
