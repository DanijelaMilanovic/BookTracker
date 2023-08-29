using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Books;
using Application.Core;
using MediatR;
using Application.Interfaces;
using Moq;

namespace BookTrackerTests.Application.Books
{
    public class CreateTests
    {
        [Fact]
        public async Task CanCreateBooksApplication()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
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

            var handler = new Create.Handler(context, userAccessorMock.Object);

            Result<Unit> result = await handler.Handle(new Create.Command { Book = book }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(1, context.Book.Count());

            await context.Database.CloseConnectionAsync();
        }

        [Fact]
        public void ShouldValidateCorrectly()
        {
            var validator = new Create.CommandValidator();

            var validBook = new Book
            {
                Title = "Sample Title",
                ISBN = "1234567890",
                Description = "Sample description",
                Image = "sample.jpg",
                NoOfPages = 250,
                Price = 19.99m,
                Rate = 4.5m,
                IsRead = true,
                PublisherId = Guid.NewGuid(),
                FormatId = Guid.NewGuid()
            };

            var validResults = validator.Validate(new Create.Command { Book = validBook });
            Assert.True(validResults.IsValid);

        }

        [Fact]
        public void ShouldNotValidateCorrectly()
        {
            var validator = new Create.CommandValidator();

            var invalidBook = new Book
            {
                ISBN = "1234567890",
                Description = "Sample description",
                Image = "sample.jpg",
                NoOfPages = 250,
                Price = 19.99m,
                Rate = 4.5m,
                IsRead = true,
                PublisherId = Guid.NewGuid(),
                FormatId = Guid.NewGuid()
            };

            var invalidResults = validator.Validate(new Create.Command { Book = invalidBook });
            Assert.False(invalidResults.IsValid);
        }
    }
}