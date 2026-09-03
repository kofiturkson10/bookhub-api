using bookhub_api.Data;
using bookhub_api.Dtos;
using bookhub_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bookhub_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<BookResponse>> CreateBook(CreateBookRequest request)
        {
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                PublishedDate = request.PublishedDate
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var response = new BookResponse(book.Id, book.Title, book.Author, book.PublishedDate);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, response);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooks()
        {
            var books = await _context.Books
                .Select(b => new BookResponse(b.Id, b.Title, b.Author, b.PublishedDate))
                .ToListAsync();

            return Ok(books);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book is null)
                return NotFound();

            return Ok(new BookResponse(book.Id, book.Title, book.Author, book.PublishedDate));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookRequest request)
        {
            var book = await _context.Books.FindAsync(id);

            if (book is null)
                return NotFound();

            book.Title = request.Title;
            book.Author = request.Author;
            book.PublishedDate = request.PublishedDate;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book is null)
                return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
