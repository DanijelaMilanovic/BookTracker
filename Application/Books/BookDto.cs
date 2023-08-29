using API.DTOs;
using Domain;

namespace Application.Books
{
    public class BookDto
    {
        public Guid BookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int NoOfPages { get; set; }
        public int YearOfPublishing { get; set; }
        public DateOnly PurshaseDate { get; set; }
        public decimal Price { get; set; }
        public decimal Rate { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }
        public Publisher Publisher { get; set; }
        public Guid PublisherId { get; set; }
        public Format Format { get; set; }
        public Guid FormatId { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<AuthorDto> Authors { get; set; } = new List<AuthorDto>();

    }
}