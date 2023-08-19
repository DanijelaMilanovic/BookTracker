using Application.Books;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;

namespace BookTrackerTests.Application.Books
{
    public class EditTests
    {
        public async Task CanEditBookApplication()
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

            var editedBook = TestSetup.CreateBook(appUser.Id);
            editedBook.Title = "New edited title";

            var handler = new Edit.Handler(context);

            Result<Unit> result = await handler.Handle(new Edit.Command { Book = editedBook, UserId = appUser.Id, BookId = book.BookId }, CancellationToken.None);
            Book edit = context.Book.Find(appUser.Id, book.BookId);

            Assert.True(result.IsSuccess);
            Assert.Equal(edit.Title, editedBook.Title);

            context.Database.CloseConnection();
        }
    }
}