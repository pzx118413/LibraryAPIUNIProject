using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LibraryAPIUNIProject.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        public string First_name { get; set; } = string.Empty;

        [Required]
        public string Last_name { get; set; } = string.Empty;

        public ICollection<Book>? Books { get; set; }
    }
}