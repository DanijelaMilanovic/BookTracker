using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}