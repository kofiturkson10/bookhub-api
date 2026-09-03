using bookhub_api.Dtos;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace bookhub_api.Services
{
    public interface IQuoteService
    {
        Task<List<QuoteResponse>> GetForUserAsync(int userId);

        Task<QuoteResponse?> GetByIdAsync(int userId, int id);

        Task<QuoteResponse> CreateAsync(int userId, CreateQuoteRequest request);

        Task<bool> UpdateAsync(int userId, int Id, UpdateQuoteRequest request);

        Task<bool> DeleteAsync(int userId, int id);
    }
}
