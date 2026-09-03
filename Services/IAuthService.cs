using bookhub_api.Dtos;

namespace bookhub_api.Services
{
    public interface IAuthService
    {
        Task<UserResponse?> RegisterAsync(RegisterRequest request);

        Task<TokenResponse?> LoginAsync(LoginRequest request);
    }
}
