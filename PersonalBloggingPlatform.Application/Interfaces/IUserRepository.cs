using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(int id, User user);
        Task<bool> DeleteAsync(int id);
    }
}
