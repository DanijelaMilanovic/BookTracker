using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Books;
using Application.Core;
using MediatR;

namespace BookTrackerTests.Application.Books
{
    public class CreateTests
    {
        [Fact]
        public async Task CanCreateBooksApplication()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            var userManager = TestSetup.CreateUserManager(context);

            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var book = TestSetup.CreateBook(appUser.Id);

            var handler = new Create.Handler(context);

            Result<Unit> result = await handler.Handle(new Create.Command { Book = book }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(1, context.Book.Count());

            context.Database.CloseConnection();
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