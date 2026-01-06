using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryAPIUNIProject.Data;
using LibraryAPIUNIProject.Models;
using LibraryAPIUNIProject.DTOs;

namespace LibraryAPIUNIProject.Controllers
{
    [Route("copies")]
    [ApiController]
    public class CopiesController : ControllerBase
    {
        private readonly LibraryContext _context;

        public CopiesController(LibraryContext context)
        {
            _context = context;
        }

        // GET copies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CopyReadDto>>> GetCopies()
        {
            var copies = await _context.Copies
                .Include(c => c.Book)
                .ThenInclude(b => b.Author)
                .ToListAsync();

            var result = copies.Select(c => new CopyReadDto
            {
                Id = c.Id,
                Book = new BookDTO
                {
                    Id = c.Book!.Id,
                    Title = c.Book.Title,
                    Year = c.Book.Year,
                    Author = new AuthorDto
                    {
                        Id = c.Book.Author!.Id,
                        First_name = c.Book.Author.First_name,
                        Last_name = c.Book.Author.Last_name
                    }
                }
            }).ToList();

            return Ok(result);
        }

        // GET copies {id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CopyReadDto>> GetCopy(int id)
        {
            var copy = await _context.Copies
                .Include(c => c.Book)
                .ThenInclude(b => b.Author)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (copy == null)
                return NotFound();

            var result = new CopyReadDto
            {
                Id = copy.Id,
                Book = new BookDTO
                {
                    Id = copy.Book!.Id,
                    Title = copy.Book.Title,
                    Year = copy.Book.Year,
                    Author = new AuthorDto
                    {
                        Id = copy.Book.Author!.Id,
                        First_name = copy.Book.Author.First_name,
                        Last_name = copy.Book.Author.Last_name
                    }
                }
            };

            return Ok(result);
        }

        // POST copies
        [HttpPost]
        public async Task<ActionResult<CopyReadDto>> CreateCopy(CopyCreateDto dto)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == dto.BookId);

            if (book == null)
                return BadRequest("Book does not exist.");

            var copy = new Copy
            {
                BookId = dto.BookId
            };

            _context.Copies.Add(copy);
            await _context.SaveChangesAsync();

            var result = new CopyReadDto
            {
                Id = copy.Id,
                Book = new BookDTO
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
                }
            };

            return CreatedAtAction(nameof(GetCopy), new { id = copy.Id }, result);
        }

        // PUT copies {id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCopy(int id, CopyCreateDto dto)
        {
            var copy = await _context.Copies.FindAsync(id);
            if (copy == null)
                return NotFound();

            var book = await _context.Books.FindAsync(dto.BookId);
            if (book == null)
                return BadRequest("Book does not exist.");

            copy.BookId = dto.BookId;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE copies {id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCopy(int id)
        {
            var copy = await _context.Copies.FindAsync(id);
            if (copy == null)
                return NotFound();

            _context.Copies.Remove(copy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}