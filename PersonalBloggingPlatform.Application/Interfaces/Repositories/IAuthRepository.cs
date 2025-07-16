using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<User> RegisterAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
