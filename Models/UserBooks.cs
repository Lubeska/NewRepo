using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class UserBooks
    {
        public int Id { get; set; }

        public IdentityUser User { get; set; }

        [StringLength(450)]
        public string AppUser { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}
