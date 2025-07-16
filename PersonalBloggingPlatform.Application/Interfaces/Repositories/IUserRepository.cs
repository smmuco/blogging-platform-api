using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
