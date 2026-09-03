using bookhub_api.Data;
using bookhub_api.Dtos;
using bookhub_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bookhub_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;

        private readonly ITokenService _tokenService;

        public AuthService(AppDbContext db,
            IPasswordHasher<User> hasher,
            ITokenService tokenService)
        {
            _db = db;
            _hasher = hasher;
            _tokenService = tokenService;
        }

        public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
        {
            var taken = await _db.Users.AnyAsync(u => u.Username == request.Username);

            if (taken)
                return null;

            var user = new User { Username = request.Username };
            user.PasswordHash = _hasher.HashPassword(user, request.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new UserResponse(user.Id, user.Username);
        }

        public async Task<TokenResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _db.Users
                .SingleOrDefaultAsync(u => u.Username == request.Username);

            if (user is null)
                return null;

            var result = _hasher.VerifyHashedPassword(
                user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            var token = _tokenService.CreateToken(user);

            return new TokenResponse(token);
        }
    }
}
