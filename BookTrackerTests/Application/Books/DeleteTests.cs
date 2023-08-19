using Application.Books;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace BookTrackerTests.Application.Books
{
    public class DeleteTests
    {
        [Fact]
        public async Task CanDeleteBookApplication()
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

            context.Book.Add(book);
            context.SaveChanges();

            await context.SaveChangesAsync();

            var handler = new Delete.Handler(context);

            Result<Unit> result = await handler.Handle(new Delete.Command { UserId = appUser.Id, BookId = book.BookId }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Empty(context.Book);

            context.Database.CloseConnection();
        }
    }
}