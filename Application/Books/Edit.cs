using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Books
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>> 
        { 
            public Book Book {get; set;}
            public string UserId { get; set; }
            public Guid BookId { get; set; }
            
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper) 
            {
                _mapper = mapper;
                _context = context;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var book = await _context.Book.FindAsync(request.UserId, request.BookId);

                _mapper.Map(request.Book, book);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Faliure("Failed to update book");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}