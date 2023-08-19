using Application.Books;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Core;

namespace BookTrackerTests.Application.Books
{
    public class DetailsTests
    {
        [Fact]
        public async Task CanDetailBookApplication()
        {
            var context = new DataContext(TestSetup.    CreateNewContextOptions());
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

            var handler = new Details.Handler(context);

            Result<Book> result = await handler.Handle(new Details.Query { UserId = appUser.Id, BookId = book.BookId }, CancellationToken.None);

            Assert.True(result.IsSuccess);

            context.Database.CloseConnection();
        }
    }
}