using System.Globalization;
using Domain;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            //if (context.Author.Any()) return;
            if (context.Book.Any()) return;
            var authors = new List<Author>
            {
                new Author
                {
                    Forename = "Sarah",
                    Surename = "J Mass",
                    Bio = "Sarah Janet Maas (born March 5, 1986) is an American fantasy author known for her fantasy series Throne of Glass, A Court of Thorns and Roses,[1] and Crescent City. As of 2022, she has sold over twelve million copies of her books and her work has been translated into 37 languages"
                },
                new Author
                {
                    Forename = "Leigh",
                    Surename = "Bardugo",
                    Bio = "Leigh Bardugo is an Israeli-American fantasy author. She is best known for her young adult Grishaverse novels, which include the Shadow and Bone trilogy, the Six of Crows dilogy, and the King of Scars dilogy. She also received acclaim for her paranormal fantasy adult debut, Ninth House. The Shadow and Bone and Six of Crows series have been adapted into Shadow and Bone by Netflix and Ninth House will be adapted by Amazon Studios; Bardugo is an executive producer on both works."
                },
                new Author
                {
                    Forename = "Ali",
                    Surename = "Hazelwood",
                    Bio = "Ali Hazelwood is the pen name of an Italian neuroscience professor[1] and writer of romance novels. Her stories center around women in STEM fields and academia. Her debut novel, The Love Hypothesis, was a New York Times best seller.[2]"
                }
            };
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
                    IsRead = true
                },
                new Book
                { 
                    ISBN = "9781408725764",
                    Title = "The Love Hypothesis",
                    Image = "",
                    NoOfPages = 376, 
                    YearOfPublishing = 2021,  
                    PurshaseDate = DateOnly.Parse("05.02.2023", new CultureInfo("hr-HR")),  
                    Price = 26.00m, 
                    Rate = 5.0m, 
                    Description = "", 
                    IsRead = true
                    },
                new Book
                {
                    ISBN = "9781408725771",
                    Title = "Love on the Brain",
                    Image = "",
                    NoOfPages = 350, 
                    YearOfPublishing = 2022,  
                    PurshaseDate = DateOnly.Parse("05.02.2023", new CultureInfo("hr-HR")), 
                    Price = 20.00m, 
                    Rate = 5.0m, 
                    Description = "", 
                    IsRead = true  
                }
            };

            //await context.Author.AddRangeAsync(authors);
            await context.Book.AddRangeAsync(books);
            await context.SaveChangesAsync();
        }
    }
}