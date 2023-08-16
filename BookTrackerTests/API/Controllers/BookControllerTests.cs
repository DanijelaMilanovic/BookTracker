using API.Controllers;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;

namespace BookTrackerTests.API.Controllers
{
    public class BookControllerTests
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
        public async Task CanListBooks()
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
            var controller = new BooksController(context, userManager);

            var result = await controller.GetBooks();

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsAssignableFrom<IEnumerable<Book>>(objectResult.Value);
            Assert.Equal(1, returnedBooks.Count()); ;

            context.Database.CloseConnection();
        }

        [Fact]
        public async Task CanGetBookById()
        {
            var context = new DataContext(CreateNewContextOptions());

            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var userManager = CreateUserManager(context);

            var appUser = new AppUser 
            { 
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var book = CreateBook(appUser.Id);

            context.Book.Add(book);
            context.SaveChanges();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id),
                new Claim(ClaimTypes.Name, appUser.UserName)
            }, "mock"));

            var controller = new BooksController(context, userManager)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };

            var result = await controller.GetBook(book.BookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedBook = Assert.IsAssignableFrom<Book>(objectResult.Value);

            Assert.Equal(book.BookId, returnedBook.BookId);

            context.Database.CloseConnection();
           }
        
    }
}
