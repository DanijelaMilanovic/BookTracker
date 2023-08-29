using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Books
{
    public class Delete
    {
         public class Command : IRequest<Result<Unit>> 
        { 
            public Guid BookId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            
            public Handler(DataContext context, IUserAccessor userAccessor) 
            {
                _context = context;
                _userAccessor = userAccessor;

            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => 
                    x.UserName == _userAccessor.GetUsername());
                    
                var book = await _context.Book.FindAsync(user.Id, request.BookId);

                if(book == null) return null;

                _context.Remove(book);

                var result = await _context.SaveChangesAsync() > 0;

                if(!result) return Result<Unit>.Faliure("Failed to delete book");

                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}