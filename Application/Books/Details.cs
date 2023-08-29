using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Books
{
    public class Details
    {
        public class Query : IRequest<Result<BookDto>>
        {
            public Guid BookId { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<BookDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<BookDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                 var user = await _context.Users.FirstOrDefaultAsync(x => 
                    x.UserName == _userAccessor.GetUsername(), cancellationToken: cancellationToken);

                var book = await _context.Book
                            .ProjectTo<BookDto>(_mapper.ConfigurationProvider, new { AppUserId = user!.Id})
                            .FirstOrDefaultAsync(b => b.BookId == request.BookId, cancellationToken: cancellationToken);

                return Result<BookDto>.Success(book);
            }
        }
    }
}