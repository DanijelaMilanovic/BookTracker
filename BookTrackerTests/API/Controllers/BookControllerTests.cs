using API.Controllers;
using Application.Books;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        
        [Fact]
        public async Task CanListBooks()
        {
            var services = new ServiceCollection();
            var context = new DataContext(TestSetup.CreateNewContextOptions());

            services.AddSingleton(context);

            var serviceProvider = services.BuildServiceProvider();
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var userManager = TestSetup.CreateUserManager(context);
            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var books = new List<Book> {
                TestSetup.CreateBook(appUser.Id) ,
                TestSetup.CreateBook(appUser.Id) ,
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

            var mediatorField = typeof(BaseApiController).GetField("_mediator", BindingFlags.NonPublic | BindingFlags.Instance);
            mediatorField.SetValue(controller, mockMediator.Object);

            mockMediator.Setup(m => m.Send(It.IsAny<List.Query>(), default))
                .ReturnsAsync(Result<List<Book>>.Success(books));

            var result = await controller.GetBooks();

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedBooks = Assert.IsAssignableFrom<List<Book>>(objectResult.Value);
            Assert.Equal(2, returnedBooks.Count);

            context.Database.CloseConnection();
        }
        
        [Fact]
        public async Task CanGetBookById()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var userManager = TestSetup.CreateUserManager(context);
            var appUser = new AppUser 
            { 
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var book = TestSetup.CreateBook(appUser.Id);
            var book2 = TestSetup.CreateBook(appUser.Id);

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

            var mediatorField = typeof(BaseApiController).GetField("_mediator", BindingFlags.NonPublic | BindingFlags.Instance);
            mediatorField.SetValue(controller, mockMediator.Object);

            mockMediator.Setup(m => m.Send(It.Is<Details.Query>(q => q.UserId == appUser.Id && q.BookId == book.BookId), default))
           .ReturnsAsync(Result<Book>.Success(book));

            var result = await controller.GetBook(book.BookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedBook = Assert.IsAssignableFrom<Book>(objectResult.Value);

            Assert.Equal(book.BookId, returnedBook.BookId);

            context.Database.CloseConnection();
        }

        [Fact]
        public async Task CanCreateBook()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var userManager = TestSetup.CreateUserManager(context);
            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var book = TestSetup.CreateBook(appUser.Id);

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
                    HttpContext = new DefaultHttpContext { User = user },                
                }
            };

            var mediatorField = typeof(BaseApiController).GetField("_mediator", BindingFlags.NonPublic | BindingFlags.Instance);
            mediatorField.SetValue(controller, mockMediator.Object);

            mockMediator.Setup(m => m.Send(It.Is<Create.Command>(q => q.Book == book), default))
                .ReturnsAsync(Result<Unit>.Success(Unit.Value));

            var result = await controller.CreateBook(book);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            context.Database.CloseConnection();
        }

        [Fact]
        public async Task CanEditBook()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var userManager = TestSetup.CreateUserManager(context);
            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var book = TestSetup.CreateBook(appUser.Id);
            var edit = TestSetup.CreateBook(appUser.Id);
            edit.Title = "New edited title";

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

            var mediatorField = typeof(BaseApiController).GetField("_mediator", BindingFlags.NonPublic | BindingFlags.Instance);
            mediatorField.SetValue(controller, mockMediator.Object);

            mockMediator.Setup(m => m.Send(It.Is<Edit.Command>(q =>q.Book == edit && q.UserId == appUser.Id && q.BookId == book.BookId), default))
               .ReturnsAsync(Result<Unit>.Success(Unit.Value));
            
            var result = await controller.EditBook(edit, book.BookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            context.Database.CloseConnection();
        }


        [Fact]
        public async Task CanDeleteBook()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var userManager = TestSetup.CreateUserManager(context);
            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var book = TestSetup.CreateBook(appUser.Id);
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

            var mediatorField = typeof(BaseApiController).GetField("_mediator", BindingFlags.NonPublic | BindingFlags.Instance);
            mediatorField.SetValue(controller, mockMediator.Object);

            mockMediator.Setup(m => m.Send(It.Is<Delete.Command>(q => q.UserId == appUser.Id && q.BookId == book.BookId), default))
                .ReturnsAsync(Result<Unit>.Success(Unit.Value));

            var result = await controller.DeleteBook(book.BookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            context.Database.CloseConnection();
        }
    }
}
