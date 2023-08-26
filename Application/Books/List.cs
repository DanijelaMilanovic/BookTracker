using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Core;
using MediatR;
using Application.Interfaces;
using AutoMapper;

namespace Application.Books
{
    public class List
    {
        public class Query : IRequest<Result<List<BookDto>>> 
        { 
            
        }

        public class Handler : IRequestHandler<Query, Result<List<BookDto>>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;
            
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
            {
                _context = context;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }

            public async Task<Result<List<BookDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => 
                    x.UserName == _userAccessor.GetUsername());

                var books = await _context.Book.Where(x => x.AppUserId == user.Id)
                    .Include(p => p.Publisher).Include(f => f.Format).Include(u => u.AppUser).ToListAsync();
                
                var booksToReturn = _mapper.Map<List<BookDto>>(books);
                return Result<List<BookDto>>.Success(booksToReturn);
            }
        }
    }
}