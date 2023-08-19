using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

        public class CommandValidator : AbstractValidator<Book> 
        {
            public CommandValidator()
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