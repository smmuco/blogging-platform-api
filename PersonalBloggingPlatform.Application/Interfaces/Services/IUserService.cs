using BloggingPlatform.Application.DTOs.User;
using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> UpdateAsync(UpdateUserRequest request);
        Task DeleteAsync(int id);
    }
}
