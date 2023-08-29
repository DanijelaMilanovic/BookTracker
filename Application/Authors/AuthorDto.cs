namespace API.DTOs
{
    public class AuthorDto
    {
        public Guid AuthorId { get; set; }
        public string Forename { get; set; }
        public string Surename { get; set; }
        public string Bio { get; set; }
    }
}