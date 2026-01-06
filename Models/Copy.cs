namespace LibraryAPIUNIProject.Models
{
    public class Copy
    {
        public int Id { get; set; }

        // Foreign key
        public int BookId { get; set; }

        // Navigation property
        public Book? Book { get; set; }
    }
}