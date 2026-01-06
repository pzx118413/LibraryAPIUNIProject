using System.ComponentModel.DataAnnotations;
using LibraryAPIUNIProject.Models;

namespace LibraryAPIUNIProject.DTOs
{
    public class CopyCreateDto
    {
        [Required]
        public int BookId { get; set; }
    }

    public class CopyReadDto
    {
        public int Id { get; set; }
        public BookDTO Book { get; set; } = new BookDTO();
    }

    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public AuthorDto Author { get; set; } = new AuthorDto();
    }
}