using Application.Core;
using MediatR;
using Persistence;

namespace Application.Books
{
    public class Delete
    {
         public class Command : IRequest<Result<Unit>> 
        { 
            public string UserId { get; set; }
            public Guid BookId { get; set; }
            
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
                var book = await _context.Book.FindAsync(request.UserId, request.BookId);

                if(book == null) return null;

                _context.Remove(book);

                var result = await _context.SaveChangesAsync() > 0;

                if(!result) return Result<Unit>.Faliure("Failed to delete book");

                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}