using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Core;

namespace Application.Books
{
    public class List
    {
        private readonly DataContext _context;
        public List(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Book>>> ListBooks()
        {
            var books = await _context.Book.ToListAsync();
            return Result<List<Book>>.Success(books);
        }

    }
}