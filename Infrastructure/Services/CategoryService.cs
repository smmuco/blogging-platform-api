using AutoMapper;
using Azure.Core;
using BloggingPlatform.Application.DTOs.Category;
using BloggingPlatform.Application.Exceptions;
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

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"User with id {id} not found.");
            }
            await _categoryRepository.DeleteAsync(id);
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
                throw new NotFoundException($"Category with id {id} not found.");
            }
            return category;
        }

        public async Task<Category> UpdateAsync(CategoryDto request)
        {
            if (request.Id == null)
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
