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
            var builder = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            return builder;
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

        private UserManager<AppUser> CreateUserManager(DataContext context)
        {
            var userStore = new UserStore<AppUser>(context);
            var userManager = new UserManager<AppUser>(userStore, null, null, null, null, null, null, null, null);
            return userManager;
        }

        [Fact]
        public async Task CanAddUserToDatabase()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new DataContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var userManager = CreateUserManager(context);

                var user = new AppUser
                {
                    UserName = "TestUser"
                };

                // Act
                var result = await userManager.CreateAsync(user);

                // Assert
                Assert.True(result.Succeeded);

                var savedUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "TestUser");
                Assert.NotNull(savedUser);
            }
        }
        [Fact]
        public async Task CanGetUserById()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new DataContext(options))
            {
                var userManager = CreateUserManager(context);
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var user = new AppUser
                {
                    UserName = "TestUser"
                };

                await userManager.CreateAsync(user);

                // Act
                var savedUser = await userManager.FindByIdAsync(user.Id);

                // Assert
                Assert.NotNull(savedUser);
                Assert.Equal("TestUser", savedUser.UserName);
            }
        }
        /*
        [Fact]
        public async Task Test_AddAuthorToBook()
        {
            var options = CreateNewContextOptions();
            var context = new DataContext(options);

            var userManager = CreateUserManager(context);
            context.Database.OpenConnection();
            context.Database.EnsureCreated(); 

            // Arrange
            var appUser = new AppUser { UserName = "user123" };
            await userManager.CreateAsync(appUser); // Kreiranje korisnika

            var author = new Author { AuthorId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), Forename = "John Doe" };
            var book = new Book { AppUserId = appUser.Id, BookId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), Title = "Sample Book" };

            context.Author.Add(author);
            context.Book.Add(book);
            context.SaveChanges();

            var bookAuthors = new BookAuthors { AppUserId = appUser.Id, BookId = book.BookId, AuthorId = author.AuthorId };

            // Act
            context.BookAuthors.Add(bookAuthors);
            context.SaveChanges();

            // Assert
            var savedBookAuthors = context.BookAuthors
                .Where(ba => ba.AppUserId == appUser.Id && ba.BookId == book.BookId && ba.AuthorId == author.AuthorId)
                .FirstOrDefault();

            Assert.NotNull(savedBookAuthors);
        }
        */
    }
}
