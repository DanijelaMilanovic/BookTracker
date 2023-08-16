using Application.Core;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Books
{
    public class Details
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        public Details(DataContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Result<Book>> GetBooks(Guid bookId, string userId)
        {
            var book = await _context.Book.FindAsync(userId, bookId); 
            return Result<Book>.Success(book);
        }
    }
}