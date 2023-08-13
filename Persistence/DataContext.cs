using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Author> Author { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Format> Format { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<BookAuthors> BookAuthors { get; set; }
        public DbSet<BookGenres> BookGenres { get; set; }
        public DbSet<BookSeries> BookSeries{ get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>(b => b.HasKey(a => new {a.AppUserId, a.BookId}));
            builder.Entity<AppUser>()
                .HasMany(b => b.Books)
                .WithOne(a => a.AppUser)
                .HasForeignKey(ap => ap.AppUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            builder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany()
                .HasForeignKey(b => b.PublisherId)
                .IsRequired();
             builder.Entity<Book>()
                .HasOne(b => b.Format)
                .WithMany()
                .HasForeignKey(b => b.FormatId)
                .IsRequired();

            builder.Entity<BookAuthors>(x => x.HasKey(b => new {b.AppUserId,  b.BookId, b.AuthorId}));
            builder.Entity<BookAuthors>()
                .HasOne(a => a.Author)
                .WithMany(b => b.Books)
                .HasForeignKey(aa => aa.AuthorId);
            builder.Entity<BookAuthors>()
                .HasOne(b => b.Book)
                .WithMany(a => a.Authors)
                .HasForeignKey(ba => new {ba.AppUserId, ba.BookId});

            builder.Entity<BookGenres>(x => x.HasKey(b => new {b.AppUserId,  b.BookId, b.GenreId}));
            builder.Entity<BookGenres>()
                .HasOne(g => g.Genre)
                .WithMany(b => b.Books)
                .HasForeignKey(gi => gi.GenreId);
            builder.Entity<BookGenres>()
                .HasOne(b => b.Book)
                .WithMany(a => a.Genres)
                .HasForeignKey(ba => new {ba.AppUserId, ba.BookId});

            builder.Entity<BookSeries>(x => x.HasKey(b => new {b.AppUserId,  b.BookId, b.SeriesId}));
            builder.Entity<BookSeries>()
                .HasOne(g => g.Series)
                .WithMany(b => b.Books)
                .HasForeignKey(gi => gi.SeriesId);
            builder.Entity<BookSeries>()
                .HasOne(b => b.Book)
                .WithMany(a => a.Series)
                .HasForeignKey(ba => new {ba.AppUserId, ba.BookId});
        }

    }
}
