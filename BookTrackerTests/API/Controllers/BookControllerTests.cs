using API.Controllers;
using Application.Books;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var context = new DataContext(TestSetup.CreateNewContextOptions());

            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

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
            var expectedBooks = new List<BookDto> {
                TestSetup.CreateBookDto(books[0]) ,
                TestSetup.CreateBookDto(books[1]) ,
            };

            context.Book.AddRange(books);
            await context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, appUser.Id),
                new(ClaimTypes.Name, appUser.UserName)
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
            mediatorField!.SetValue(controller, mockMediator.Object);

            mockMediator.Setup(m => m.Send(It.IsAny<List.Query>(), default))
                .ReturnsAsync(Result<List<BookDto>>.Success(expectedBooks));

            var result = await controller.GetBooks();

            var objectResult = Assert.IsType<OkObjectResult>(result);

            var returnedBooks = Assert.IsAssignableFrom<List<BookDto>>(objectResult.Value);
            Assert.Equal(2, returnedBooks.Count);
            Assert.Equal(expectedBooks[0].Title, returnedBooks[0].Title);

            await context.Database.CloseConnectionAsync();
        }
        
        [Fact]
        public async Task CanGetBookById()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

            var userManager = TestSetup.CreateUserManager(context);
            var appUser = new AppUser 
            { 
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var book = TestSetup.CreateBook(appUser.Id);
            var expected = TestSetup.CreateBookDto(book);

            context.Book.Add(book);

            await context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, appUser.Id),
                new(ClaimTypes.Name, appUser.UserName)
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
            mediatorField!.SetValue(controller, mockMediator.Object);

            mockMediator.Setup(m => m.Send(It.Is<Details.Query>(q => q.BookId == book.BookId), default))
                .ReturnsAsync(Result<BookDto>.Success(expected));

            var result = await controller.GetBook(book.BookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedBook = Assert.IsAssignableFrom<BookDto>(objectResult.Value);

            Assert.Equal(book.BookId, returnedBook.BookId);

            await context.Database.CloseConnectionAsync();
        }

        [Fact]
        public async Task CanCreateBook()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

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
                new(ClaimTypes.NameIdentifier, appUser.Id),
                new(ClaimTypes.Name, appUser.UserName)
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
            mediatorField!.SetValue(controller, mockMediator.Object);

            mockMediator.Setup(m => m.Send(It.Is<Create.Command>(q => q.Book == book), default))
                .ReturnsAsync(Result<Unit>.Success(Unit.Value));

            var result = await controller.CreateBook(book);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            await context.Database.CloseConnectionAsync();
        }

        [Fact]
        public async Task CanEditBook()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

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

            await context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, appUser.Id),
                new(ClaimTypes.Name, appUser.UserName)
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
            mediatorField!.SetValue(controller, mockMediator.Object);

            mockMediator.Setup(m => m.Send(It.Is<Edit.Command>(q =>q.Book == edit), default))
               .ReturnsAsync(Result<Unit>.Success(Unit.Value));
            
            var result = await controller.EditBook(edit.BookId, edit);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            await context.Database.CloseConnectionAsync();
        }

        [Fact]
        public async Task CanDeleteBook()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

            var userManager = TestSetup.CreateUserManager(context);
            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var book = TestSetup.CreateBook(appUser.Id);
            context.Book.Add(book);

            await context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, appUser.Id),
                new(ClaimTypes.Name, appUser.UserName)
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
            mediatorField!.SetValue(controller, mockMediator.Object);

            mockMediator.Setup(m => m.Send(It.Is<Delete.Command>(q => q.BookId == book.BookId), default))
                .ReturnsAsync(Result<Unit>.Success(Unit.Value));

            var result = await controller.DeleteBook(book.BookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            await context.Database.CloseConnectionAsync();
        }
    }
}
