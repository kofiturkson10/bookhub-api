using bookhub_api.Data;
using bookhub_api.Dtos;
using bookhub_api.Models;
using Microsoft.EntityFrameworkCore;

namespace bookhub_api.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly AppDbContext _db;

        public QuoteService(AppDbContext db) => _db = db;

        public async Task<List<QuoteResponse>> GetForUserAsync(int userId)
        {
            return await _db.Quotes
                .Where(q => q.UserId == userId)
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => new QuoteResponse(q.Id, q.Text, q.Author, q.CreatedAt))
                .ToListAsync();
        }

        public async Task<QuoteResponse?> GetByIdAsync(int userId, int Id)
        {
            return await _db.Quotes
                .Where(q => q.Id == Id && q.UserId == userId)
                .Select(q => new QuoteResponse(q.Id, q.Text, q.Author, q.CreatedAt))
                .FirstOrDefaultAsync();
        }

        public async Task<QuoteResponse> CreateAsync(int userId, CreateQuoteRequest request)
        {
            var quote = new Quote
            {
                Text = request.Text,
                Author = request.Author,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _db.Quotes.Add(quote);
            await _db.SaveChangesAsync();

            return new QuoteResponse(quote.Id, quote.Text, quote.Author, quote.CreatedAt);
        }

        public async Task<bool> UpdateAsync(int userId, int id, UpdateQuoteRequest request)
        {
            var quote = await _db.Quotes
                .FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

            if (quote is null)
                return false;

            quote.Text = request.Text;
            quote.Author = request.Author;

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int userId, int id)
        {
            var quote = await _db.Quotes
                .FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

            if (quote is null) 
                return false;

            _db.Quotes.Remove(quote);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
