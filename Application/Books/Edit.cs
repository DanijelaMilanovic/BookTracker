using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Books
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>> 
        { 
            public Book Book {get; set;}
        }

        public class CommandValidator : AbstractValidator<Command> {
            public CommandValidator()
            {
                RuleFor(x => x.Book).SetValidator(new BookValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor) 
            {
                _mapper = mapper;
                _context = context;
                _userAccessor = userAccessor;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => 
                    x.UserName == _userAccessor.GetUsername(), cancellationToken: cancellationToken);

                request.Book.AppUserId = user!.Id;
                request.Book.AppUser = user;
                var book = await _context.Book.FindAsync(user.Id, request.Book.BookId);

                _mapper.Map<Book, Book>(request.Book, book!);
                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Unit>.Faliure("Failed to update book");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}