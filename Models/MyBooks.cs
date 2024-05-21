using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class MyBooks
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}
