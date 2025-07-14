using AutoMapper;
using BloggingPlatform.Application.DTOs.Category;
using BloggingPlatform.Application.Interfaces.Repositories;
using BloggingPlatform.Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _context.Categories
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
            return true;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.Posts)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
