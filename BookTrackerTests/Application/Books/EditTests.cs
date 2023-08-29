using Application.Books;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;

namespace BookTrackerTests.Application.Books
{
    public class EditTests
    {
        [Fact]
        public async Task CanEditBookApplication()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            var userManager = TestSetup.CreateUserManager(context);

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfiles()));
            var mockMapper = new Mapper(mapperConfig);

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

            book.Title = "New edited title";

            var handler = new Edit.Handler(context, mockMapper, userAccessorMock.Object);

            Result<Unit> result = await handler.Handle(new Edit.Command { Book = book }, CancellationToken.None);
            Book edit = (await context.Book.FindAsync(appUser.Id, book.BookId))!;

            Assert.True(result.IsSuccess);
            Assert.Equal(edit.Title, book.Title);

            await context.Database.CloseConnectionAsync();
        }
    }
}