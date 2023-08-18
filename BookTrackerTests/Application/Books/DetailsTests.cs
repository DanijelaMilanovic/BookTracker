using Application.Books;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Microsoft.AspNetCore.Mvc;
using Application.Core;

namespace BookTrackerTests.Application.Books
{
    public class DetailsTests
    {
        private DbContextOptions<DataContext> CreateNewContextOptions()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            return options;
        }

        private UserManager<AppUser> CreateUserManager(DataContext context)
        {
            var userStore = new UserStore<AppUser>(context);
            var userManager = new UserManager<AppUser>(userStore, null, null, null, null, null, null, null, null);
            return userManager;
        }

        private Book CreateBook(string appUserId)
        {
            var format = new Format
            {
                FormatId = Guid.NewGuid(),
                Name = "Format1"
            };
            var publisher = new Publisher
            {
                PublisherId = Guid.NewGuid(),
                Name = "Publisher",
                Address = "Adress"
            };

            var book = new Book
            {
                AppUserId = appUserId,
                BookId = Guid.NewGuid(),
                Title = "Test Book",
                NoOfPages = 200,
                YearOfPublishing = 2023,
                PurshaseDate = new DateOnly(2023, 1, 1),
                Price = 20.00m,
                Rate = 5.00m,
                Publisher = publisher,
                Format = format
            };
            return book;
        }

        [Fact]
        public async Task CanDetailBookApplication()
        {
            var context = new DataContext(CreateNewContextOptions());
            var userManager = CreateUserManager(context);

            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var book = CreateBook(appUser.Id);

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