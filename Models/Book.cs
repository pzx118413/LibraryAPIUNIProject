using System.ComponentModel.DataAnnotations;

namespace LibraryAPIUNIProject.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Year { get; set; }

        // Foreign key
        public int AuthorId { get; set; }

        // Navigation property
        public Author? Author { get; set; }

        public ICollection<Copy>? Copies { get; set; }
    }
}
