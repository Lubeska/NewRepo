using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Books.Models;

namespace Books.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Books.Models.Author> Author { get; set; } = default!;
        public DbSet<Books.Models.Book> Book { get; set; } = default!;
        public DbSet<Books.Models.Genre> Genre { get; set; } = default!;
        public DbSet<Books.Models.Review> Review { get; set; } = default!;
        public DbSet<Books.Models.UserBooks> UserBooks { get; set; } = default!;
        public DbSet<Books.Models.BookGenre> BookGenre { get; set; } = default!;
        public DbSet<Books.Models.MyBooks> MyBooks { get; set; } = default!;
    }
}
