using Application.Books;
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
        public async Task<IActionResult> GetBooks() 
        {
            return HandleResult(await new List(_context).ListBooks());

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(Guid BookId) 
        { 
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

            return HandleResult(await new Details(_context, _userManager).GetBooks(BookId, user.Id));
        }
    }
}