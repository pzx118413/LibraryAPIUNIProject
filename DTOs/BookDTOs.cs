using System.ComponentModel.DataAnnotations;
using LibraryAPIUNIProject.Models;

namespace LibraryAPIUNIProject.DTOs
{
    public class BookCreateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Year { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }

    public class AuthorDto
    {
        public int Id { get; set; }
        public string First_name { get; set; } = string.Empty;
        public string Last_name { get; set; } = string.Empty;
    }

    public class BookReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public AuthorDto Author { get; set; } = new AuthorDto();
    }
}