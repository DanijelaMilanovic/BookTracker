using Application.Books;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Core;
using Application.Interfaces;
using Moq;
using AutoMapper;

namespace BookTrackerTests.Application.Books
{
    public class DetailsTests
    {
        [Fact]
        public async Task CanDetailBookApplication()
        {
            var context = new DataContext(TestSetup.    CreateNewContextOptions());
            var userManager = TestSetup.CreateUserManager(context);

            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var userAccessorMock = new Mock<IUserAccessor>();
            userAccessorMock.Setup(x => x.GetUsername()).Returns("user123");

            var book = TestSetup.CreateBook(appUser.Id);

            context.Book.Add(book);
            await context.SaveChangesAsync();

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfiles()));
            var mockMapper = new Mapper(mapperConfig);

            var handler = new Details.Handler(context, mockMapper, userAccessorMock.Object);

            Result<BookDto> result = await handler.Handle(new Details.Query { BookId = book.BookId }, CancellationToken.None);

            Assert.True(result.IsSuccess);

            context.Database.CloseConnection();
        }
    }
}