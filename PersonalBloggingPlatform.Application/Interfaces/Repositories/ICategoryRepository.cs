using BloggingPlatform.Application.DTOs.Category;
using BloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}
