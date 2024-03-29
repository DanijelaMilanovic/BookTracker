using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Genre
    {
        public Guid GenreId { get; set; }
        public string Name { get; set; }
        public ICollection<BookGenres> Books { get; set; } = new List<BookGenres>();
    }
}