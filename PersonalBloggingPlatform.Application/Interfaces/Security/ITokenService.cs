using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Interfaces.Security
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
