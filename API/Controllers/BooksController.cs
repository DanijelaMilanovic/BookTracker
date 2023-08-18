using Application.Books;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class BooksController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetBooks() 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return HandleResult(await Mediator.Send(new List.Query { UserId = userId }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(Guid BookId) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await Mediator.Send(new Details.Query { UserId = userId, BookId = BookId})); 
        }

    }
}