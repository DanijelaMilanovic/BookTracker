namespace Domain
{
    public class Book
    {
        public Guid BookId { get; set; }
        public String ISBN { get; set; }
        public String Title { get; set; }
        public String Image { get; set; }
        public int NoOfPages { get; set; }
        public int YearOfPublishing { get; set; }
        public DateOnly PurshaseDate { get; set; }
        public decimal Price { get; set; }
        public decimal Rate { get; set; }
        public String Description { get; set; }
        public Boolean IsRead { get; set; }
        
    }
}