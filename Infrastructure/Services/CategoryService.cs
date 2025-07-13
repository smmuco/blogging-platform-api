using System.Runtime.InteropServices;
using BloggingPlatform.Application.DTOs.Category;
using BloggingPlatform.Application.Interfaces.Repositories;
using BloggingPlatform.Application.Interfaces.Services;
using BloggingPlatform.Domain.Entities;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        // Dependency injection for the repository
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository repository)
        {
            _categoryRepository = repository;
        }

        public async Task<Category> CreateAsync(CategoryDto request)
        {
            return await _categoryRepository.CreateAsync(request);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return false; // Category not found
            }
            await _categoryRepository.DeleteAsync(id);
            return true; // Assuming deletion is successful
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null; // Invalid ID
            }
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return null; // Category not found
            }
            return category;
        }

        public async Task<Category> UpdateAsync(CategoryDto request)
        {
            if (request == null || !request.Id.HasValue || request.Id <= 0 )
            {
                throw new ArgumentException("Invalid category data.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }
            
            var updatedCategory = await _categoryRepository.UpdateAsync(request);
            return updatedCategory;
        }
    }
}
