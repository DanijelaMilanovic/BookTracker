using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Books;
using Application.Core;
using Moq;
using Microsoft.Extensions.Logging;
using Application.Interfaces;

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

            var userAccessorMock = new Mock<IUserAccessor>();
            userAccessorMock.Setup(x => x.GetUsername()).Returns("user123");

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

            var mockLogger = new Mock<ILogger<List>>();

            var handler = new List.Handler(context, mockLogger.Object, userAccessorMock.Object);

            Result<List<Book>> result = await handler.Handle(new List.Query(), CancellationToken.None);
            
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);

            context.Database.CloseConnection();
        }
    }
}