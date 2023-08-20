using Domain;
using FluentValidation;

namespace Application.Books
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.ISBN).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Image).NotEmpty();
            RuleFor(x => x.NoOfPages).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Rate).NotEmpty();
            RuleFor(x => x.IsRead).NotEmpty();
            RuleFor(x => x.PublisherId).NotEmpty();
            RuleFor(x => x.FormatId).NotEmpty();
        }
    }
}