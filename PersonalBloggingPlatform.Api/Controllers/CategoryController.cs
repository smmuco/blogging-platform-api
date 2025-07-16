using BloggingPlatform.Application.DTOs.Category;
using BloggingPlatform.Application.Exceptions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

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

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null.");
            }
            var createdCategory = await _categoryService.CreateAsync(categoryDto);
            return CreatedAtAction(nameof(GetCategoryByIdAsync), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || categoryDto.Id == null)
            {
                throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null or have an invalid ID.");
            }
            var updatedCategory = await _categoryService.UpdateAsync(categoryDto);
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid category ID", nameof(id));
            }
                
            await _categoryService.DeleteAsync(id);
            return NoContent(); 
        }
    }
}
