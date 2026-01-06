using Microsoft.EntityFrameworkCore;
using LibraryAPIUNIProject.Models;

namespace LibraryAPIUNIProject.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Copy> Copies => Set<Copy>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author!)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Copies)
                .WithOne(c => c.Book!)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}