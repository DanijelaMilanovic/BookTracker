using System.Globalization;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            var authors = new List<Author>
            {
                new Author
                {
                    Forename = "Sarah J.",
                    Surename = "Mass",
                    Bio = "Sarah Janet Maas (born March 5, 1986) is an American fantasy author known for her fantasy series Throne of Glass, A Court of Thorns and Roses,[1] and Crescent City. As of 2022, she has sold over twelve million copies of her books and her work has been translated into 37 languages"
                },
                new Author
                {
                    Forename = "Alan",
                    Surename = "Shalloway",
                    Bio = "Alan Shalloway is the founder and CEO of Net Objectives. With over 40 years of experience, Alan is an industry thought leader in Lean, Kanban, product portfolio management, Scrum and agile design. He helps companies transition to Lean and Agile methods enterprise-wide as well teaches courses in these areas."
                },
                new Author
                {
                    Forename = "James R.",
                    Surename = "Trott",
                    Bio = "James R. Trott is a senior consultant for a large software company in the Pacific Northwest and formerly was a senior engineer for a large aerospace company. He holds a master of science in applied mathematics, an MBA, and a master of arts in intercultural studies."
                }
            };
            var users = new List<AppUser> 
            {
                 new AppUser
                    {
                        Forename = "Danijela",
                        Surname = "Milanovic",
                        UserName = "DanijelaMilanovic",
                        Email = "danijela@test.com"
                    }
            };
            /*
            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
            */
            var books = new List<Book>
            {
                new Book
                {
                    ISBN = "9788681856062",
                    Title = "Dvor trnja i ruža",
                    Image = "",
                    NoOfPages = 410, 
                    YearOfPublishing = 2021,  
                    PurshaseDate = DateOnly.Parse("13.07.2023", new CultureInfo("hr-HR")), 
                    Price = 20.00m, 
                    Rate = 5.0m, 
                    Description = "", 
                    IsRead = true,
                    FormatId = new Guid("0390d402-1700-4995-86f4-6de7b79b1c6a"),
                    PublisherId = new Guid("efc3a0cf-79cb-4611-8fbc-98c16f53b4c6"),
                    Authors = new List<BookAuthors> {
                        new BookAuthors {
                            Author = authors[1]
                        },
                        new BookAuthors {
                            Author = authors[2]
                        }
                    },
                    AppUser = users[0]
                },
                new Book
                {
                    ISBN = "8675552386",
                    Title = "Projektni obrasci: Nove tehnike objektno orjentisanog pogramiranja",
                    Image = "",
                    NoOfPages = 254, 
                    YearOfPublishing = 2004,  
                    PurshaseDate = DateOnly.Parse("17.02.2023", new CultureInfo("hr-HR")), 
                    Price = 21.00m, 
                    Description = "", 
                    IsRead = true,
                    FormatId = new Guid("0390d402-1700-4995-86f4-6de7b79b1c6a"),
                    PublisherId = new Guid("f0b651a1-b419-4b5f-8092-774fdb130427"),
                    Authors = new List<BookAuthors> {
                        new BookAuthors {
                            Author = authors[0]
                        }
                    },
                    AppUser = users[0]  
                }
            };
            //await context.Author.AddRangeAsync(authors);
            //await context.Book.AddRangeAsync(books);
            await context.SaveChangesAsync();
            
        }
    }
}