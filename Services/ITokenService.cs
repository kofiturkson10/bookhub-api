using bookhub_api.Models;

namespace bookhub_api.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
