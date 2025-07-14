using AutoMapper;
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
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _categoryRepository = repository;
            _mapper = mapper;
        }

        public async Task<Category> CreateAsync(CategoryDto request)
        {
            var category = _mapper.Map<Category>(request);
            return await _categoryRepository.CreateAsync(category);
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

            return category;
        }

        public async Task<Category> UpdateAsync(CategoryDto request)
        {
            if (request.Id <= 0 || request.Id == null)
            {
                throw new ArgumentException("Invalid category data.");
            }
            var category = await _categoryRepository.GetByIdAsync(request.Id.Value);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }
            _mapper.Map(request,category);
            
            return await _categoryRepository.UpdateAsync(category);
        }
    }
}
