using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Books;

namespace BookTrackerTests
{
    public static class TestSetup
    {
        public static DbContextOptions<DataContext> CreateNewContextOptions()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            return options;
        }

        public static UserManager<AppUser> CreateUserManager(DataContext context)
        {
            var userStore = new UserStore<AppUser>(context);
            var passwordHasher = new PasswordHasher<AppUser>();
            
            var userManager = new UserManager<AppUser>(
                userStore, null, passwordHasher, null, null, null, null, null, null);
            return userManager;
        }

        public static Book CreateBook(string appUserId)
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
                Address = "Address"
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
        public static BookDto CreateBookDto(Book book)
        {
            var bookDto = new BookDto
            {
                AppUserId = book.AppUserId,
                BookId = book.BookId,
                Title = book.Title,
                NoOfPages = book.NoOfPages,
                YearOfPublishing = book.YearOfPublishing,
                PurshaseDate = book.PurshaseDate,
                Price = book.Price,
                Rate = book.Rate,
                Publisher = book.Publisher,
                Format = book.Format
            };
            return bookDto;
        }
    }
}
