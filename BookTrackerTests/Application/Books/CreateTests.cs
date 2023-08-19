using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Books;
using Application.Core;
using MediatR;

namespace BookTrackerTests.Application.Books
{
    public class CreateTests
    {
        [Fact]
        public async Task CanCreateBooksApplication()
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

            var book = TestSetup.CreateBook(appUser.Id);

            var handler = new Create.Handler(context);

            Result<Unit> result = await handler.Handle(new Create.Command { Book = book }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(1, context.Book.Count());

            context.Database.CloseConnection();
        }
    }
}