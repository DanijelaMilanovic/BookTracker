using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Books
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>> 
        { 
            public Book Book {get; set;}
        }

        public class CommandValidator : AbstractValidator<Command> 
        {
            public CommandValidator()
            {
                RuleFor(x => x.Book).SetValidator(new BookValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context) 
            {
                _context = context;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Book.Add(request.Book);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Faliure("Failed to create book");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}