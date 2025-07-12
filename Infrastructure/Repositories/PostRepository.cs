using BloggingPlatform.Application.Interfaces;
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

        public Task<Post> UpdateAsync(int id, Post post)
        {
            var existingPost = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (existingPost == null)
            {
                throw new KeyNotFoundException($"Post with ID {id} not found.");
            }
            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.UpdatedAt = DateTime.UtcNow;
            existingPost.Author = post.Author;
            existingPost.IsPublished = post.IsPublished;
            existingPost.CategoryId = post.CategoryId;
            _context.Posts.Update(existingPost);
            _context.SaveChanges();
            return Task.FromResult(existingPost);
        }
    }
}
