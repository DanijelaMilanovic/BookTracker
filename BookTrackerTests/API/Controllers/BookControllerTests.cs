using API.Controllers;
using Application.Books;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Persistence;
using System.Reflection;
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
        //Integracioni testovi, moze da radi funkciju samo ako je korisnik autorizovan
        //tj ako je prijavljen i posjeduje knjige 
 
        [Fact]
        public async Task CanListBooks()
        {
            var services = new ServiceCollection();
            var context = new DataContext(CreateNewContextOptions());

            // Dodajte DataContext servis u servis provider za testiranje
            services.AddSingleton(context);

            var serviceProvider = services.BuildServiceProvider();
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var userManager = CreateUserManager(context);
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id),
                new Claim(ClaimTypes.Name, appUser.UserName)
            }, "mock"));

            var mockMediator = new Mock<IMediator>();

            var controller = new BooksController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };

            // Upotreba refleksije za postavljanje Mediator-a
            var mediatorField = typeof(BaseApiController).GetField("_mediator", BindingFlags.NonPublic | BindingFlags.Instance);
            mediatorField.SetValue(controller, mockMediator.Object);

            // Postavite povratni rezultat za Mock<IMediator>
            mockMediator.Setup(m => m.Send(It.Is<List.Query>(q => q.UserId == appUser.Id), default))
                       .ReturnsAsync(Result<List<Book>>.Success(books));

            // Izvršite akciju GetBooks
            var result = await controller.GetBooks();

            // Provjerite povratni rezultat
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsAssignableFrom<List<Book>>(objectResult.Value);
            Assert.Equal(2, returnedBooks.Count);

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
            var book2 = CreateBook(appUser.Id);

            context.Book.Add(book2);
            context.Book.Add(book);

            context.SaveChanges();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id),
                new Claim(ClaimTypes.Name, appUser.UserName)
            }, "mock"));

            var mockMediator = new Mock<IMediator>();

            var controller = new BooksController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };

            // Upotreba refleksije za postavljanje Mediator-a
            var mediatorField = typeof(BaseApiController).GetField("_mediator", BindingFlags.NonPublic | BindingFlags.Instance);
            mediatorField.SetValue(controller, mockMediator.Object);

            // Postavite povratni rezultat za Mock<IMediator>
            mockMediator.Setup(m => m.Send(It.Is<Details.Query>(q => q.UserId == appUser.Id && q.BookId == book.BookId), default))
           .ReturnsAsync(Result<Book>.Success(book));

            //citaj knjige kao prijavljen korisnik 
            var result = await controller.GetBook(book.BookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedBook = Assert.IsAssignableFrom<Book>(objectResult.Value);

            Assert.Equal(book.BookId, returnedBook.BookId);

            context.Database.CloseConnection();
        }
    }
}
