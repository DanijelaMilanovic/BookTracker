using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Books;
using Application.Core;

namespace BookTrackerTests.Application.Books
{
    public class ListTests
    {
        [Fact]
        public async Task CanListBooksApplication()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            var userManager = TestSetup.CreateUserManager(context);

            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var books = new List<Book> {
                TestSetup.CreateBook(appUser.Id) ,
                TestSetup.CreateBook(appUser.Id) ,
            };

            context.Book.AddRange(books);
            await context.SaveChangesAsync();

            var handler = new List.Handler(context);

            Result<List<Book>> result = await handler.Handle(new List.Query { UserId = appUser.Id }, CancellationToken.None);
            
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);

            context.Database.CloseConnection();
        }
    }
}