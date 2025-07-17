using BloggingPlatform.Application.DTOs.Category;
using BloggingPlatform.Application.Exceptions;
using BloggingPlatform.Application.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(CategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {id} not found.");
            }
            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryDto categoryDto)
        {
            _logger.LogInformation("Creating a new category with data: {@CategoryDto}", categoryDto);
            if (categoryDto == null)
            {
                _logger.LogError("Category data is null when trying to create a new category.");
                throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null.");
            }
            var createdCategory = await _categoryService.CreateAsync(categoryDto);
            _logger.LogInformation("Category created with ID: {Id}", createdCategory.Id);
            return CreatedAtAction(nameof(GetCategoryByIdAsync), new { id = createdCategory.Id }, createdCategory);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] CategoryDto categoryDto)
        {
            _logger.LogInformation("Updating category with data: {@CategoryDto}", categoryDto);
            if (categoryDto == null)
            {
                throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null.");
            }

            var updatedCategory = await _categoryService.UpdateAsync(categoryDto);
            _logger.LogInformation("Category updated with ID: {Id}", updatedCategory.Id);

            return Ok(updatedCategory);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            _logger.LogInformation("Deleting category with ID: {Id}", id);
            if (id <= 0)
            {
                throw new ArgumentException("Invalid category ID", nameof(id));
            }
                
            await _categoryService.DeleteAsync(id);
            _logger.LogInformation("Category with ID: {Id} deleted successfully.", id);

            return NoContent(); 
        }
    }
}
