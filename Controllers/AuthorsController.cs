using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryAPIUNIProject.Data;
using LibraryAPIUNIProject.Models;

namespace LibraryAPIUNIProject.Controllers
{
    [Route("authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public AuthorsController(LibraryContext context)
        {
            _context = context;
        }

        // GET authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _context.Authors
                .Include(a => a.Books)
                .ToListAsync();
        }

        // GET authors {id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
                return NotFound();

            return author;
        }

        // POST authors
        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(Author author)
        {
            if (string.IsNullOrWhiteSpace(author.First_name) ||
                string.IsNullOrWhiteSpace(author.Last_name))
            {
                return BadRequest("First and last name cannot be empty.");
            }

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        // PUT authors {id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, Author author)
        {
            if (id != author.Id)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(author.First_name) ||
                string.IsNullOrWhiteSpace(author.Last_name))
            {
                return BadRequest("First and last name cannot be empty.");
            }

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Authors.Any(a => a.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE authors {id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                return NotFound();

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}