using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Core;
using MediatR;

namespace Application.Books
{
    public class List
    {
        public class Query : IRequest<Result<List<Book>>> 
        { 
            public string UserId {get; set;}
            
        }

        public class Handler : IRequestHandler<Query, Result<List<Book>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<Book>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var books = await _context.Book.Where(x => x.AppUserId == request.UserId).ToListAsync();
                return Result<List<Book>>.Success(books);
            }
        }
    }
}