using Application.Books;
using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTrackerTests.Application.Books
{
    public class BookValidatorTests
    {
        [Fact]
        public void ShouldValidateCorrectly()
        {
            var validator = new BookValidator();

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

            var validResults = validator.Validate(validBook);
            Assert.True(validResults.IsValid);

        }

        [Fact]
        public void ShouldNotValidateCorrectly()
        {
            var validator = new BookValidator();

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

            var invalidResults = validator.Validate(invalidBook);
            Assert.False(invalidResults.IsValid);
        }
    }
}
