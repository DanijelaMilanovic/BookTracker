using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Core;
using MediatR;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Application.Books
{
    public class List
    {
        public class Query : IRequest<Result<List<BookDto>>> { }

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
                            .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
                
                return Result<List<BookDto>>.Success(books);
            }
        }
    }
}