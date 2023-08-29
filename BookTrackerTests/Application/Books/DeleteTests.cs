using Application.Books;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
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

            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var userAccessorMock = new Mock<IUserAccessor>();
            userAccessorMock.Setup(x => x.GetUsername()).Returns("user123");

            var book = TestSetup.CreateBook(appUser.Id);

            context.Book.Add(book);
            await context.SaveChangesAsync();

            var handler = new Delete.Handler(context, userAccessorMock.Object);

            Result<Unit> result = await handler.Handle(new Delete.Command { BookId = book.BookId }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Empty(context.Book);

            await context.Database.CloseConnectionAsync();
        }
    }
}