using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Interfaces;

namespace Application.Books
{
    public class List
    {
        public class Query : IRequest<Result<List<Book>>> 
        { 
            
        }

        public class Handler : IRequestHandler<Query, Result<List<Book>>>
        {
            private readonly DataContext _context;
            private readonly ILogger<List> _logger;
            private readonly IUserAccessor _userAccessor;
            
            public Handler(DataContext context, ILogger<List> logger,  IUserAccessor userAccessor)
            {
                _logger = logger;
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<List<Book>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => 
                    x.UserName == _userAccessor.GetUsername());

                try 
                {
                    var books = await _context.Book.Where(x => x.AppUserId == user.Id).ToListAsync();
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