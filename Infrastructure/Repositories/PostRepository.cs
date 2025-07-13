using BloggingPlatform.Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;
        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Post> CreateAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _context.Posts
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
            return true;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _context.Posts
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetPostsByCategoryAsync(int categoryId)
        {
            return await _context.Posts
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Post> UpdateAsync( Post post)
        {
            _context.SaveChanges();
            return post;
        }
    }
}
