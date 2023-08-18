using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Books;
using Moq;
using Microsoft.Extensions.Options;
using Application.Core;

namespace BookTrackerTests.Application.Books
{
    public class ListTests
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
        public async Task CanListBooksApplication()
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

            var books = new List<Book> {
                CreateBook(appUser.Id) ,
                CreateBook(appUser.Id) ,
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