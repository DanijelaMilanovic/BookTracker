using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Core;
using MediatR;
using Microsoft.Extensions.Logging;

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
            private readonly ILogger<List> _logger;
            public Handler(DataContext context, ILogger<List> logger)
            {
                _logger = logger;
                _context = context;
            }

            public async Task<Result<List<Book>>> Handle(Query request, CancellationToken cancellationToken)
            {
                try 
                {
                    var books = await _context.Book.Where(x => x.AppUserId == request.UserId).ToListAsync();
                    _logger.LogInformation("Task was successfull");
                    return Result<List<Book>>.Success(books);
                }
                catch(System.Exception)
                {
                    _logger.LogInformation("Error while performing task");
                    return Result<List<Book>>.Faliure("Error while performing task");
                }
                
            }
        }
    }
}