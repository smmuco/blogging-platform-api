using AutoMapper;
using BloggingPlatform.Application.DTOs.Category;
using BloggingPlatform.Application.Exceptions;
using BloggingPlatform.Application.Interfaces.Repositories;
using BloggingPlatform.Domain.Entities;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly CategoryService _categoryService;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
        private readonly Mock<ILogger<CategoryService>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        public CategoryServiceTests()
        {
            _categoryService = new CategoryService(
                _categoryRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CreateAsync_WhenCategoryIsValid_ShouldReturnCreatedCategory()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Name = "Test Category"
            };
            var category = new Category
            {
                Id = 1,
                Name = categoryDto.Name
            };
            _mapperMock
                .Setup(m => m.Map<Category>(categoryDto))
                .Returns(category);
            _categoryRepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Category>()))
                .ReturnsAsync(category);
            // Act
            var result = await _categoryService.CreateAsync(categoryDto);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.Name, result.Name);
        }

        [Fact]
        public async Task CreateAsync_WhenCategoryIsNotValid_ShouldReturnNull()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Name = ""
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _categoryService.CreateAsync(categoryDto));

            Assert.Equal("Category name cannot be empty.", ex.Message);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCategoryExists_ShouldReturnCategory()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category
            {
                Id = categoryId,
                Name = "Test Category"
            };
            _categoryRepositoryMock
                .Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(category);
            // Act
            var result = await _categoryService.GetByIdAsync(categoryId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.Name, result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCategoryDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var categoryId = 1;
            _categoryRepositoryMock
                .Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync((Category?)null);
            // Act & Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _categoryService.GetByIdAsync(categoryId));
            Assert.Equal($"Category with id {categoryId} not found.", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryIsValid_ShouldReturnUpdatedCategory()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = 1,
                Name = "Updated Category"
            };
            var category = new Category
            {
                Id = categoryDto.Id.Value,
                Name = categoryDto.Name
            };
            _mapperMock
                .Setup(m => m.Map<Category>(categoryDto))
                .Returns(category);
            _categoryRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(category);
            // Act
            var result = await _categoryService.UpdateAsync(categoryDto);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.Name, result.Name);
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = 1,
                Name = "Updated Category"
            };
            _categoryRepositoryMock
                .Setup(r => r.GetByIdAsync(categoryDto.Id.Value))
                .ReturnsAsync((null as Category));
            // Act & Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _categoryService.UpdateAsync(categoryDto));
            Assert.Equal($"Category with id {categoryDto.Id} not found.", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryIdIsNull_ShouldThrowArgumentException()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Name = "Updated Category"
            };
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _categoryService.UpdateAsync(categoryDto));
            Assert.Equal("Invalid category data.", ex.Message);
        }

        [Fact]
        public async Task DeleteAsync_WhenCategoryExists_ShouldDeleteCategory()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category
            {
                Id = categoryId,
                Name = "Test Category"
            };
            _categoryRepositoryMock
                .Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(category);
            _categoryRepositoryMock
                .Setup(r => r.DeleteAsync(categoryId))
                .Returns(Task.CompletedTask);
            // Act
            await _categoryService.DeleteAsync(categoryId);
            // Assert
            _categoryRepositoryMock.Verify(r => r.DeleteAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenCategoryDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var categoryId = 1;
            _categoryRepositoryMock
                .Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync((null as Category));
            // Act & Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _categoryService.DeleteAsync(categoryId));
            Assert.Equal($"Category with id {categoryId} not found.", ex.Message);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
            _categoryRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(categories);
            // Act
            var result = await _categoryService.GetAllAsync();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Category 1", result[0].Name);
            Assert.Equal("Category 2", result[1].Name);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoCategories_ShouldReturnEmptyList()
        {
            // Arrange
            _categoryRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Category>());
            // Act
            var result = await _categoryService.GetAllAsync();
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
