using Books.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace Books.Models
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>())) 
            {

                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.EnsureCreated();

                if (!context.Roles.Any()) 
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                if(!context.Users.Any(u => u.Email == "admin@bookstore.com"))
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = "admin@bookstore.com",
                        Email = "admin@bookstore.com",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin@123");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
                

                if (context.Book.Any() || context.Author.Any() || context.Genre.Any())
                {
                    return; // DB has been seeded
                }

                context.Author.AddRange(
                    new Author { FirstName = "Colleen", LastName = "Hoover", BirthDate = DateTime.Parse("1979-9-11"), Nationality = "American", Gender = "Female" },
                    new Author { FirstName = "Lori", LastName = "Gottlieb", BirthDate = DateTime.Parse("1966-9-20"), Nationality = "American", Gender = "Female" },
                    new Author { FirstName = "Agatha", LastName = "Christie", BirthDate = DateTime.Parse("1976-9-15"), Nationality = "British", Gender = "Female" },
                    new Author { FirstName = "Franz", LastName = "Kafka", BirthDate = DateTime.Parse("1883-7-3"), Nationality = "Czech", Gender = "Male" },
                    new Author { FirstName = "John", LastName = "Green", BirthDate = DateTime.Parse("1977-8-24"), Nationality = "American", Gender = "Male" }
                );
                context.SaveChanges();

                context.Book.AddRange(
                    new Book
                    {
                        Title = "Verity",
                        YearPublished = 2018,
                        NumPages = 336,
                        Description = "Verity is a 2018 psychological thriller by New York Times bestselling author Colleen Hoover. The novel is set between New York City and Vermont, and follows struggling writer Lowen Ashleigh as she ghostwrites a novel on behalf of Verity Crawford, a woman who is in a vegetative state following a traumatic accident.",
                        Publisher = "Hachette Book Group",
                        FrontPage = "/verity.jpg",
                        DownloadUrl = "https://www.google.com",
                        AuthorId = context.Author.Single(d => d.FirstName == "Colleen" && d.LastName == "Hoover").Id
                    },
                    new Book
                    {
                        Title = "All Your Perfects",
                        YearPublished = 2018,
                        NumPages = 320,
                        Description = "All Your Perfects is a profound novel about a damaged couple whose potential future hinges on promises made in the past. This is a heartbreaking page-turner that asks: Can a resounding love with a perfect beginning survive a lifetime between two imperfect people?",
                        Publisher = "Atria Books",
                        FrontPage = "/perfects.jpg",
                        DownloadUrl = "https://www.google.com",
                        AuthorId = context.Author.Single(d => d.FirstName == "Colleen" && d.LastName == "Hoover").Id
                    },
                    new Book
                    {
                        Title = "Maybe You Should Talk to Someone",
                        YearPublished = 2019,
                        NumPages = 432,
                        Description = "Maybe You Should Talk To Someone by Lori Gottlieb is a memoir highlighting the transformative power of therapy through the author's personal journey and experiences working as a therapist. The book guides readers to better understand themselves and others while offering insight into the therapeutic process.",
                        Publisher = "Houghton Mifflin Harcourt",
                        FrontPage = "/talk.jpg",
                        DownloadUrl = "https://www.google.com",
                        AuthorId = context.Author.Single(d => d.FirstName == "Lori" && d.LastName == "Gottlieb").Id
                    },
                    new Book
                    {
                        Title = "The Trial",
                        YearPublished = 1925,
                        NumPages = 160,
                        Description = "The Trial by Franz Kafka is a haunting novel that follows the story of a man named Joseph K. who is arrested and put on trial for an unknown crime in a nightmarish and absurd legal system. It delves into themes of guilt, bureaucracy, and the individual's struggle against an incomprehensible and oppressive society.",
                        Publisher = "Verlag Die Schmiede",
                        FrontPage = "/trial.jpg",
                        DownloadUrl = "https://www.google.com",
                        AuthorId = context.Author.Single(d => d.FirstName == "Franz" && d.LastName == "Kafka").Id
                    },
                    new Book
                    {
                        Title = "Partners in Crime",
                        YearPublished = 1929,
                        NumPages = 277,
                        Description = "A collection of short stories within a story. Tommy and Tuppence impersonate Mr Blunt who runs an international detective agency which is a front for Russian spies. The two detectives undertake a series of investigations which parodies the styles of well-known detective writers of that era.",
                        Publisher = "William Collins, Sons & Co.",
                        FrontPage = "/partners.jpg",
                        DownloadUrl = "https://www.google.com",
                        AuthorId = context.Author.Single(d => d.FirstName == "Agatha" && d.LastName == "Christie").Id
                    },
                    new Book
                    {
                        Title = "A Daughter's a Daughter",
                        YearPublished = 1952,
                        NumPages = 272,
                        Description = "Ann Prentice falls in love with Richard Cauldfield and hopes for new happiness. Her only child, Sarah, cannot contemplate the idea of her mother marrying again and wrecks any chance of her remarriage. Resentment and jealousy corrode their relationship as each seeks relief in different directions.",
                        Publisher = "Heinemann",
                        FrontPage = "/daughter.jpg",
                        DownloadUrl = "https://www.google.com",
                        AuthorId = context.Author.Single(d => d.FirstName == "Agatha" && d.LastName == "Christie").Id
                    },
                    new Book
                    {
                        Title = "Paper Towns",
                        YearPublished = 2008,
                        NumPages = 336,
                        Description = "Paper Towns is a coming of age story set in Orlando, Florida. It focuses on Quentin, a young man about to embark on his adult life, and the adventure he and his friends, Ben and Radar, have their senior year that centers on the disappearance of their classmate Margo Roth Spiegelman.",
                        Publisher = "Dutton Books",
                        FrontPage = "/paper.jpg",
                        DownloadUrl = "https://www.google.com",
                        AuthorId = context.Author.Single(d => d.FirstName == "John" && d.LastName == "Green").Id
                    },
                    new Book
                    {
                        Title = "The Fault in Our Stars",
                        YearPublished = 2012,
                        NumPages = 336,
                        Description = "The Fault in Our Stars by John Green is a young adult fiction novel that narrates the story of a 16-year-old girl who is diagnosed with cancer. She joins a support group where she meets Augustus, and there is a rollercoaster of emotions throughout this novel as the relationship between Hazel and Augustus develops.",
                        Publisher = "Penguin Young Readers Group",
                        FrontPage = "/fault.jpg",
                        DownloadUrl = "https://www.google.com",
                        AuthorId = context.Author.Single(d => d.FirstName == "John" && d.LastName == "Green").Id
                    }
                );
                context.SaveChanges();

                context.Genre.AddRange(
                    new Genre { GenreName = "Romance" },
                    new Genre { GenreName = "Fiction" },
                    new Genre { GenreName = "Thriller" },
                    new Genre { GenreName = "Memoir" },
                    new Genre { GenreName = "Mystery" },
                    new Genre { GenreName = "Crime" }
                );
                context.SaveChanges();

                context.BookGenre.AddRange(
                    new BookGenre { BookId = 1, GenreId = 1 },
                    new BookGenre { BookId = 1, GenreId = 2 },
                    new BookGenre { BookId = 1, GenreId = 3 },
                    new BookGenre { BookId = 2, GenreId = 1 },
                    new BookGenre { BookId = 2, GenreId = 2 },
                    new BookGenre { BookId = 3, GenreId = 4 },
                    new BookGenre { BookId = 4, GenreId = 2 },
                    new BookGenre { BookId = 5, GenreId = 2 },
                    new BookGenre { BookId = 5, GenreId = 5 },
                    new BookGenre { BookId = 6, GenreId = 1 },
                    new BookGenre { BookId = 6, GenreId = 6 },
                    new BookGenre { BookId = 7, GenreId = 5 },
                    new BookGenre { BookId = 8, GenreId = 1 }
                );
                context.SaveChanges();

                await context.SaveChangesAsync();
            }
        }
    }
}
