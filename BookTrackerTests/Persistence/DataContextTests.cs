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
            //kreira options koji koristi sqlite
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            //kreira kontekst tj klon opcije 
            var context = new DataContext(options);
            //Otvara konekciju 
            context.Database.OpenConnection();
            //Provjerava da li postoji baza, ako ne onda je kreira
            context.Database.EnsureCreated();

            Assert.True(context.Database.IsSqlite());
        }
        
    }
}
