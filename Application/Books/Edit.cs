using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
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
            public Handler(DataContext context) 
            {
                _context = context;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var book = await _context.Book.FindAsync(request.UserId, request.BookId);

                book.Title = request.Book.Title ?? book.Title;
                book.ISBN = request.Book.ISBN ?? book.ISBN;
                book.Image = request.Book.Image ?? book.Image;
                book.NoOfPages = request.Book.NoOfPages;
                book.YearOfPublishing = request.Book.YearOfPublishing;
                book.PurshaseDate = request.Book.PurshaseDate;
                book.Price = request.Book.Price;
                book.Rate = request.Book.Rate;
                book.Description = request.Book.Description ?? book.Description;
                book.IsRead = request.Book.IsRead;
                book.PublisherId = request.Book.PublisherId;
                book.FormatId = request.Book.FormatId;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Faliure("Failed to update book");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}