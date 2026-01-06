using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryAPIUNIProject.Data;
using LibraryAPIUNIProject.Models;
using LibraryAPIUNIProject.DTOs;

namespace LibraryAPIUNIProject.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET books authorId
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadDto>>> GetBooks([FromQuery] int? authorId)
        {
            var query = _context.Books.Include(b => b.Author).AsQueryable();

            if (authorId.HasValue)
            {
                query = query.Where(b => b.AuthorId == authorId.Value);
            }

            var books = await query.ToListAsync();

            var result = books.Select(b => new BookReadDto
            {
                Id = b.Id,
                Title = b.Title,
                Year = b.Year,
                Author = new AuthorDto
                {
                    Id = b.Author!.Id,
                    First_name = b.Author.First_name,
                    Last_name = b.Author.Last_name
                }
            }).ToList();

            return Ok(result);
        }

        // GET books {id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookReadDto>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            var result = new BookReadDto
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                Author = new AuthorDto
                {
                    Id = book.Author!.Id,
                    First_name = book.Author.First_name,
                    Last_name = book.Author.Last_name
                }
            };

            return Ok(result);
        }

        // POST books
        [HttpPost]
        public async Task<ActionResult<BookReadDto>> CreateBook(BookCreateDto dto)
        {
            var author = await _context.Authors.FindAsync(dto.AuthorId);
            if (author == null)
                return BadRequest("Author does not exist.");

            if (string.IsNullOrWhiteSpace(dto.Title) || dto.Year < 0)
                return BadRequest("Invalid title or year.");

            var book = new Book
            {
                Title = dto.Title,
                Year = dto.Year,
                AuthorId = dto.AuthorId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var result = new BookReadDto
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                Author = new AuthorDto
                {
                    Id = author.Id,
                    First_name = author.First_name,
                    Last_name = author.Last_name
                }
            };

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, result);
        }

        // PUT books {id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookCreateDto dto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            var author = await _context.Authors.FindAsync(dto.AuthorId);
            if (author == null)
                return BadRequest("Author does not exist.");

            if (string.IsNullOrWhiteSpace(dto.Title) || dto.Year < 0)
                return BadRequest("Invalid title or year.");

            book.Title = dto.Title;
            book.Year = dto.Year;
            book.AuthorId = dto.AuthorId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE books {id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
