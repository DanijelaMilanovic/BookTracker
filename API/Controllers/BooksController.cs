using Application.Books;
using Domain;
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

        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book) 
        {
            return HandleResult(await Mediator.Send(new Create.Command { Book = book }));
        } 

        [HttpPut("{id}")]
        public async Task<IActionResult> EditBook(Book book, Guid bookId) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await Mediator.Send(new Edit.Command { Book = book, BookId = bookId, UserId = userId }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid bookId) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return HandleResult(await Mediator.Send(new Delete.Command { BookId = bookId, UserId = userId }));
        }
    }
}