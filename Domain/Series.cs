namespace Domain
{
    public class Series
    {
        public Guid SeriesId { get; set; }
        public string Title { get; set; }
        public int NoInASeries { get; set; }
        public ICollection<BookSeries> Books { get; set; } = new List<BookSeries>();
    }
}