namespace Domain
{
    public class BookGenres
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}