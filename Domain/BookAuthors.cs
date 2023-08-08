namespace Domain
{
    public class BookAuthors
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
        
    }
}