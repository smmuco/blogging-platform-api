using BloggingPlatform.Application.Interfaces;
using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Repositories
{
    public class PostRepository : IPostRepository
    {
        public async Task<Post> CreateAsync(Post post)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Post?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetPostsByCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<Post> UpdateAsync(int id, Post post)
        {
            throw new NotImplementedException();
        }
    }
}
