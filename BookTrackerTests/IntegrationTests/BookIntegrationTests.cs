using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using API.DTOs;
using Application.Books;
using System.Net.Http.Headers;
using Domain;
using Newtonsoft.Json.Linq;

namespace BookTrackerTests.IntegrationTests
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public IntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task CanRegisterAndLogin()
        {
            var registerDto = new RegisterDto
            {
                Forename = "Forename",
                Surname = "Surname",
                Email = "test1@example.com",
                Username = "test1",
                Password = "Password123"
            };
            
            var registerResponse = await _client.PostAsJsonAsync("/api/Account/register", registerDto);
            registerResponse.EnsureSuccessStatusCode();
            
            var loginDto = new LoginDto
            {
                Email = "test1@example.com",
                Password = "Password123"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/Account/login", loginDto);
            loginResponse.EnsureSuccessStatusCode();

            var userDto = await loginResponse.Content.ReadFromJsonAsync<UserDto>();
            Assert.Equal("test1", userDto?.Username);

        }

        [Fact]
        public async Task CanLoginAndCreateBook()
        {
            var registerDto = new RegisterDto
            {
                Forename = "Forename",
                Surname = "Surname",
                Email = "test2@example.com",
                Username = "test2",
                Password = "Password123"
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/Account/register", registerDto);
            registerResponse.EnsureSuccessStatusCode();

            var loginDto = new LoginDto
            {
                Email = "test2@example.com",
                Password = "Password123"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/Account/login", loginDto);
            loginResponse.EnsureSuccessStatusCode();

            var responseContent = await loginResponse.Content.ReadAsStringAsync();
            var token = JObject.Parse(responseContent)["token"].ToString();

            var authorizedClient = _factory.CreateClient();
            authorizedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var authors = new List<Author>
            {
                new()
                {
                    AuthorId = Guid.NewGuid(),
                    Forename = "Forename1",
                    Surename = "Surname1",
                    Bio = "Bio1"
                },
                new()
                {
                    AuthorId = Guid.NewGuid(),
                    Forename = "Forename2",
                    Surename = "Surname2",
                    Bio = "Bio2"
                }
            };

            var format = new Format
            {
                FormatId = Guid.NewGuid(),
                Name = "Format1"
            };
            var publisher = new Publisher
            {
                PublisherId = Guid.NewGuid(),
                Name = "Publisher",
                Address = "Address"
            };

            var book = new Book
            {
                BookId = Guid.NewGuid(),
                Title = "Test Book",
                NoOfPages = 200,
                YearOfPublishing = 2023,
                PurshaseDate = new DateOnly(2023, 1, 1),
                Price = 20.00m,
                Rate = 5.00m,
                Publisher = publisher,
                Format = format,
                BookAuthors = new List<BookAuthors> {
                    new()
                    {
                        Author = authors[0]
                    },
                    new()
                    {
                        Author = authors[1]
                    }
                }
            };

            var createBookResponse = await authorizedClient.PostAsJsonAsync("/api/Books", book);
            createBookResponse.EnsureSuccessStatusCode();

            var getBookResponse = await authorizedClient.GetAsync($"/api/Books/{book.BookId}", CancellationToken.None);
            getBookResponse.EnsureSuccessStatusCode();

            var retrievedBookDto = await getBookResponse.Content.ReadFromJsonAsync<BookDto>();
            Assert.Equal("Test Book", retrievedBookDto?.Title);
        }

        [Fact]
        public async Task CanLoginAndCreateBookAndEdit()
        {
            var registerDto = new RegisterDto       
            {
                Forename = "Forename",
                Surname = "Surname",
                Email = "test3@example.com",
                Username = "test3",
                Password = "Password123"
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/Account/register", registerDto);
            registerResponse.EnsureSuccessStatusCode();

            var loginDto = new LoginDto    
            {
                Email = "test3@example.com",
                Password = "Password123"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/Account/login", loginDto);
            loginResponse.EnsureSuccessStatusCode();

            var responseContent = await loginResponse.Content.ReadAsStringAsync();
            var token = JObject.Parse(responseContent)["token"].ToString();

            var authorizedClient = _factory.CreateClient();
            authorizedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var format = new Format
            {
                FormatId = Guid.NewGuid(),
                Name = "Format1"
            };
            var publisher = new Publisher
            {
                PublisherId = Guid.NewGuid(),
                Name = "Publisher",
                Address = "Address"
            };

            var book = new Book
            {
                BookId = Guid.NewGuid(),
                Title = "Test Book",
                NoOfPages = 200,
                YearOfPublishing = 2023,
                PurshaseDate = new DateOnly(2023, 1, 1),
                Price = 20.00m,
                Rate = 5.00m,
                Publisher = publisher,
                Format = format
            };

            var createBookResponse = await authorizedClient.PostAsJsonAsync("/api/Books", book);
            createBookResponse.EnsureSuccessStatusCode();

            book.Title = "New Title";

            var putBookResponse = await authorizedClient.PutAsJsonAsync($"/api/Books/{book.BookId}", book);
            putBookResponse.EnsureSuccessStatusCode();

            var getBookResponse = await authorizedClient.GetAsync($"/api/Books/{book.BookId}", CancellationToken.None);
            getBookResponse.EnsureSuccessStatusCode();

            var retrievedBookDto = await getBookResponse.Content.ReadFromJsonAsync<BookDto>();
            Assert.Equal("New Title", retrievedBookDto?.Title);
        }

        [Fact]
        public async Task CanLoginAndCreateBookAndDelete()
        {
            var registerDto = new RegisterDto
            {
                Forename = "Forename",
                Surname = "Surname",
                Email = "test4@example.com",
                Username = "test4",
                Password = "Password123"
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/Account/register", registerDto);
            registerResponse.EnsureSuccessStatusCode();

            var loginDto = new LoginDto
            {
                Email = "test4@example.com",
                Password = "Password123"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/Account/login", loginDto);
            loginResponse.EnsureSuccessStatusCode();

            var responseContent = await loginResponse.Content.ReadAsStringAsync();
            var token = JObject.Parse(responseContent)["token"].ToString();

            var authorizedClient = _factory.CreateClient();
            authorizedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var format = new Format
            {
                FormatId = Guid.NewGuid(),
                Name = "Format1"
            };
            var publisher = new Publisher
            {
                PublisherId = Guid.NewGuid(),
                Name = "Publisher",
                Address = "Address"
            };

            var book = new Book
            {
                BookId = Guid.NewGuid(),
                Title = "Test Book",
                NoOfPages = 200,
                YearOfPublishing = 2023,
                PurshaseDate = new DateOnly(2023, 1, 1),
                Price = 20.00m,
                Rate = 5.00m,
                Publisher = publisher,
                Format = format
            };

            var createBookResponse = await authorizedClient.PostAsJsonAsync("/api/Books", book);
            createBookResponse.EnsureSuccessStatusCode();

            var deleteBookResponse = await authorizedClient.DeleteAsync($"/api/Books/{book.BookId}");
            deleteBookResponse.EnsureSuccessStatusCode();
        }
    }
}
