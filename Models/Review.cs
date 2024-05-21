using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int BookID { get; set; }

        [StringLength(450)]
        public string AppUser { get; set; }

        public IdentityUser? User { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        public int? Rating { get; set; }

        public Book? Book { get; set; }
    }
}
