using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace BookTrackerTests.Persistence
{
    public class DataContextTests
    {
        [Fact]
        public void CanConnectToSQlite()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new DataContext(options);
            
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            Assert.True(context.Database.IsSqlite());
        }
        
    }
}
