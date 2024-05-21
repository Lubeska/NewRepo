using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class Book
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Year Published")]
        public int? YearPublished { get; set; }

        [Display(Name = "Number of Pages")]
        public int? NumPages { get; set; }

        public string? Description { get; set; }

        [StringLength(50)]
        public string? Publisher { get; set; }

        [Display(Name = "Front Page")]
        public string? FrontPage { get; set; } //slikata e ova

        [Display(Name = "Download URL")]
        public string? DownloadUrl { get; set; }
        public int AuthorId { get; set; }

        public Author? Author { get; set; }

        public ICollection<Review>? Reviews { get; set; }

        public ICollection<UserBooks>? UserBooks { get; set; }

        public ICollection<BookGenre>? BookGenre { get; set; }
    }
}
