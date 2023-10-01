using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Books;
using Application.Core;
using Moq;
using Application.Interfaces;
using AutoMapper;

namespace BookTrackerTests.Application.Books
{
    public class ListTests
    {
        [Fact]
        public async Task CanListBooksApplication()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            var userManager = TestSetup.CreateUserManager(context);

            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

            var userAccessorMock = new Mock<IUserAccessor>();
            userAccessorMock.Setup(x => x.GetUsername()).Returns("user123");

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

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfiles()));
            var mockMapper = new Mapper(mapperConfig);

            var handler = new List.Handler(context, userAccessorMock.Object, mockMapper);

            Result<List<BookDto>> result = await handler.Handle(new List.Query(), CancellationToken.None);
            
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);

            await context.Database.CloseConnectionAsync();
        }
    }
}