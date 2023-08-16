using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace BookTrackerTests.Persistence
{
    public class DataContextTests
    {
        private DbContextOptions<DataContext> CreateNewContextOptions()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            return options;
        }

        private UserManager<AppUser> CreateUserManager(DataContext context)
        {
            var userStore = new UserStore<AppUser>(context);
            var userManager = new UserManager<AppUser>(userStore, null, null, null, null, null, null, null, null);
            return userManager;
        }

        [Fact]
        public void CanConnectToSQlite()
        {
            var context = new DataContext(CreateNewContextOptions());
            
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            Assert.True(context.Database.IsSqlite());

            context.Database.CloseConnection();      
        }

        [Fact]
        public void CanWriteFormat()
        {
            var context = new DataContext(CreateNewContextOptions());
            var format= new Format
            {
                Name = "Paperback"    
            };
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            context.Format.Add(format);
            context.SaveChanges();

            Assert.Equal(1, context.Format.Count());

            context.Database.CloseConnection();
        }
        [Fact]
        public void CanLoadFormat()
        {
            var context = new DataContext(CreateNewContextOptions());
            var format1 = new Format
            {
                Name = "Paperback"
            };
            var format2 = new Format
            {
                Name = "Hardback"
            };
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            context.Format.Add(format1);
            context.Format.Add(format2);
            context.SaveChanges();

            Assert.Equal(2, context.Format.Count());

            context.Database.CloseConnection();
        }

        [Fact]
        public void CanLoadFormatById()
        {
            var context = new DataContext(CreateNewContextOptions());
            var format = new Format
            {
                Name = "Paperback"
            };
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            context.Format.Add(format);
            context.SaveChanges();

            var loadedFormat = context.Format.Find(format.FormatId);

            Assert.NotNull(loadedFormat);
            Assert.Equal(format.Name, loadedFormat.Name);

            context.Database.CloseConnection();
        }
        [Fact]
        public void CanEditFormat()
        {
            var context = new DataContext(CreateNewContextOptions());
            var format = new Format
            {
                Name = "Paperback"
            };
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            context.Format.Add(format);
            context.SaveChanges();

            var loadedFormat = context.Format.Find(format.FormatId);
            loadedFormat.Name = "Hardback";

            context.SaveChanges();

            var updatedFormat = context.Format.Find(format.FormatId);
            Assert.Equal("Hardback", updatedFormat.Name);

            context.Database.CloseConnection();
        }
        [Fact]
        public void CanDeleteFormat()
        {
            var context = new DataContext(CreateNewContextOptions());
            var format = new Format
            {
                Name = "Paperback"
            };
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            context.Format.Add(format);
            context.SaveChanges();

            context.Format.Remove(format);
            context.SaveChanges();

            var deltedFormat = context.Format.Find(format.FormatId);
            Assert.Null(deltedFormat);

            context.Database.CloseConnection();
        }

        [Fact]
        public async Task CanAddUserToDatabase()
        {
            var options = CreateNewContextOptions();

            var context = new DataContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var userManager = CreateUserManager(context);

            var user = new AppUser
            {
                UserName = "TestUser"
            };

            var result = await userManager.CreateAsync(user);

            Assert.True(result.Succeeded);

            var savedUser = await context.Users
                .FirstOrDefaultAsync(u => u.UserName == "TestUser");

            Assert.NotNull(savedUser);
        }

        [Fact]
        public async Task CanGetUserById()
        {
            var options = CreateNewContextOptions();

            var context = new DataContext(options);
     
            var userManager = CreateUserManager(context);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var user = new AppUser
            {
                UserName = "TestUser"
            };

            await userManager.CreateAsync(user);

            var savedUser = await userManager.FindByIdAsync(user.Id);

            Assert.NotNull(savedUser);
            Assert.Equal("TestUser", savedUser.UserName);
        }
        
        [Fact]
        public async Task CanAddAuthorsToBook()
        {
            var options = CreateNewContextOptions();
            var context = new DataContext(options);

            var userManager = CreateUserManager(context);
            context.Database.OpenConnection();
            context.Database.EnsureCreated(); 

            var appUser = new AppUser 
            { 
                Id = Guid.NewGuid().ToString(),
                UserName = "user123"
            };
            await userManager.CreateAsync(appUser);

            var format = new Format
            {
                FormatId = new Guid(),
                Name = "Format1"
            };
            var publisher = new Publisher
            {
                PublisherId = new Guid(),
                Name = "Publisher",
                Address = "Adress"
            };

            var author = new Author
            {
                AuthorId = Guid.NewGuid(),
                Forename = "John Doe"
            };

            var book = new Book
            { 
                AppUserId = appUser.Id, 
                BookId = Guid.NewGuid(), 
                Title = "Sample Book",
                NoOfPages = 200,
                YearOfPublishing = 2023,
                PurshaseDate = new DateOnly(2023, 1, 1),
                Price = 20.00m,
                Rate = 5.00m,
                Publisher = publisher,
                Format = format 
            };

            context.Format.Add(format);
            context.Publisher.Add(publisher);
            context.Author.Add(author);
            context.Book.Add(book);

            context.SaveChanges();

            var bookAuthors = new BookAuthors 
            { 
                AppUserId = appUser.Id, 
                BookId = book.BookId, 
                AuthorId = author.AuthorId 
            };

            context.BookAuthors.Add(bookAuthors);
            context.SaveChanges();

            var savedBookAuthors = context.BookAuthors
                .Where(ba => ba.AppUserId == appUser.Id && ba.BookId == book.BookId && ba.AuthorId == author.AuthorId)
                .FirstOrDefault();

            Assert.NotNull(savedBookAuthors);
            context.Database.CloseConnection();
        }
        
    }
}
