

using BloggingPlatform.Application.DTOs.Category;
using BloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> CreateAsync(CategoryDto request);
        Task<Category> UpdateAsync(CategoryDto request);
        Task DeleteAsync(int id);
    }
}
