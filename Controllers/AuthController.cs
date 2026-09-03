using bookhub_api.Dtos;
using bookhub_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bookhub_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (result is null)
                return Conflict($"Username '{request.Username}' is already taken.");

            return Ok(result);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result is null)
                return Unauthorized("Invalid username or password");

            return Ok(result);
        }
    }
}
