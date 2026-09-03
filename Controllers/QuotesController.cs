using bookhub_api.Data;
using bookhub_api.Dtos;
using bookhub_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bookhub_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteService _service;

        public QuotesController(IQuoteService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuoteResponse>>> GetMyQuotes()
        {
            var userId = GetUserId();
            var quotes = await _service.GetForUserAsync(userId);
            return Ok(quotes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuoteResponse>> GetQuote(int id)
        {
            var userId = GetUserId();
            var quote = await _service.GetByIdAsync(userId, id);

            return quote is null ? NotFound() : Ok(quote);
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<ActionResult<QuoteResponse>> CreateQuote(CreateQuoteRequest request)
        {
            var userId = GetUserId();
            var created = await _service.CreateAsync(userId, request);

            return CreatedAtAction(nameof(GetMyQuotes), new { id = created.id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuote(int id, UpdateQuoteRequest request)
        {
            var userId = GetUserId();
            var success = await _service.UpdateAsync(userId, id, request);

            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var userId = GetUserId();
            var success = await _service.DeleteAsync(userId, id);

            return success ? NoContent() : NotFound();
        }
    }
}
