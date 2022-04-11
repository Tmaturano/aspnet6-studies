using Blog.Models;

namespace Blog.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
