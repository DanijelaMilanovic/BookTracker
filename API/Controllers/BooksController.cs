using Application.Books;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BooksController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetBooks() 
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBook(Guid bookId) 
        {
            return HandleResult(await Mediator.Send(new Details.Query {BookId = bookId})); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book) 
        {
            return HandleResult(await Mediator.Send(new Create.Command { Book = book }));
        } 

        [HttpPut("{bookId}")]
        public async Task<IActionResult> EditBook(Guid bookId, Book book)
        {
            book.BookId = bookId;
            return HandleResult(await Mediator.Send(new Edit.Command { Book = book }));
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(Guid bookId) 
        {
            return HandleResult(await Mediator.Send(new Delete.Command { BookId = bookId }));
        }
    }
}