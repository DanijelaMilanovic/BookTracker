using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Books
{
    public class Details
    {
        public class Query : IRequest<Result<Book>>
        {
            public string UserId { get; set; }
            public Guid BookId { get; set; }

        }
        public class Handler : IRequestHandler<Query, Result<Book>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Book>> Handle(Query request, CancellationToken cancellationToken)
            {
                var book = await _context.Book.FindAsync(request.UserId, request.BookId);
                return Result<Book>.Success(book);
            }
        }
    }
}