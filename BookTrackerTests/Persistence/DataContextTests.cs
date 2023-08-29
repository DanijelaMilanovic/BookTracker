using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace BookTrackerTests.Persistence
{
    public class DataContextTests
    {
        [Fact]
        public void CanConnectToSqLite()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            Assert.True(context.Database.IsSqlite());

            context.Database.CloseConnection();      
        }

        [Fact]
        public void CanWriteFormat()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());

            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var format = new Format
            {
                Name = "Paperback"
            };
            context.Format.Add(format);
            context.SaveChanges();

            Assert.Equal(1, context.Format.Count());

            context.Database.CloseConnection();
        }
        [Fact]
        public void CanLoadFormat()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            var format = new List<Format>
            {
                new() { Name = "Paperback"},
                new() { Name = "Hardback"}
            };
            
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            context.Format.AddRange(format);
            context.SaveChanges();

            Assert.Equal(2, context.Format.Count());

            context.Database.CloseConnection();
        }

        [Fact]
        public void CanLoadFormatById()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
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
            var context = new DataContext(TestSetup.CreateNewContextOptions());
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
            var context = new DataContext(TestSetup.CreateNewContextOptions());
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

            var deletedFormat = context.Format.Find(format.FormatId);
            Assert.Null(deletedFormat);

            context.Database.CloseConnection();
        }

        [Fact]
        public async Task CanAddUserToDatabase()
        {
            var options = TestSetup.CreateNewContextOptions();

            var context = new DataContext(options);
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

            var userManager = TestSetup.CreateUserManager(context);

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
            var options = TestSetup.CreateNewContextOptions();

            var context = new DataContext(options);
     
            var userManager = TestSetup.CreateUserManager(context);
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

            var user = new AppUser
            {
                UserName = "TestUser"
            };

            await userManager.CreateAsync(user, "password");

            var savedUser = await userManager.FindByIdAsync(user.Id);

            Assert.NotNull(savedUser);
            Assert.Equal("TestUser", savedUser.UserName);
        }
        
        [Fact]
        public async Task CanAddAuthorsToBook()
        {
            var options = TestSetup.CreateNewContextOptions();
            var context = new DataContext(options);

            var userManager = TestSetup.CreateUserManager(context);
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync(); 

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
                Address = "Address"
            };

            var author = new Author
            {
                AuthorId = Guid.NewGuid(),
                Forename = "John Doe"
            };

            var book = TestSetup.CreateBook(appUser.Id);

            context.Format.Add(format);
            context.Publisher.Add(publisher);
            context.Author.Add(author);
            context.Book.Add(book);

            await context.SaveChangesAsync();

            var bookAuthors = new BookAuthors 
            { 
                AppUserId = appUser.Id, 
                BookId = book.BookId, 
                AuthorId = author.AuthorId 
            };

            context.BookAuthors.Add(bookAuthors);
            await context.SaveChangesAsync();

            var savedBookAuthors = context.BookAuthors
                .FirstOrDefault(ba => ba.AppUserId == appUser.Id && ba.BookId == book.BookId && ba.AuthorId == author.AuthorId);

            Assert.NotNull(savedBookAuthors);
            await context.Database.CloseConnectionAsync();
        }
    }
}
