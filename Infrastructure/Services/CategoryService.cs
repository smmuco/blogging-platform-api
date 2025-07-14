using AutoMapper;
using Azure.Core;
using BloggingPlatform.Application.DTOs.Category;
using BloggingPlatform.Application.Exceptions;
using BloggingPlatform.Application.Interfaces.Repositories;
using BloggingPlatform.Application.Interfaces.Services;
using BloggingPlatform.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        // Dependency injection for the repository
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ICategoryRepository repository, IMapper mapper, ILogger<CategoryService> logger)
        {
            _categoryRepository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Category> CreateAsync(CategoryDto request)
        {
            _logger.LogInformation("Creating a new category with name: {Name}", request.Name);

            var category = _mapper.Map<Category>(request);
            var created = await _categoryRepository.CreateAsync(category);
            
            _logger.LogInformation("Category created with ID: {Id}", created.Id);
            return created;

        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting category with ID: {id}", id);

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with ID: {id} not found for deletion.", id);
                throw new NotFoundException($"User with id {id} not found.");
            }
            await _categoryRepository.DeleteAsync(id);

            _logger.LogInformation("Category with ID: {id} deleted successfully.", id);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if( category == null)
            {
                _logger.LogWarning("Category with ID: {Id} not found.", id);
                throw new NotFoundException($"Category with id {id} not found.");
            }
            return category;
        }

        public async Task<Category> UpdateAsync(CategoryDto request)
        {
            if (request.Id == null)
            {
                _logger.LogWarning("Update request does not contain a valid category ID.");
                throw new ArgumentException("Invalid category data.");
            }

            _logger.LogInformation("Updating category with ID: {Id}", request.Id);

            var category = await _categoryRepository.GetByIdAsync(request.Id.Value);
            if (category == null)
            {
                _logger.LogWarning("Category with ID: {Id} not found for update.", request.Id);
                throw new KeyNotFoundException("Category not found.");
            }
            _mapper.Map(request,category);

            _logger.LogInformation("Category with ID: {Id} updated successfully.", category.Id);
            return await _categoryRepository.UpdateAsync(category);
        }
    }
}
