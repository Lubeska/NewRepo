using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "Genre Name")]
        public string GenreName { get; set; }

        public ICollection<BookGenre> BookGenres { get; set; }
    }
}
