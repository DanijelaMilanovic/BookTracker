namespace Domain
{
    public class Author
    {
        public Guid AuthorId { get; set; }
        public string Forename { get; set; }
        public string Surename { get; set; }
        public string Bio { get; set; }
        public ICollection<BookAuthors> Books { get; } = new List<BookAuthors>();
    }
}