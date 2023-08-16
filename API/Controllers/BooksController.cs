using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;

namespace API.Controllers
{
    public class BooksController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        public BooksController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks() {
           
            var books = await _context.Book.ToListAsync();
            return Ok(books);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(Guid BookId) {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
            var book = await _context.Book.FindAsync(user.Id, BookId); 
            return Ok(book);
        }
    }
}