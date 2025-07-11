using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Interfaces
{
    public interface IPostRepository
    {
        Task <List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int id);
        Task<Post> CreateAsync(Post post);
        Task<Post> UpdateAsync(int id, Post post);
        Task<bool> DeleteAsync(int id);
        Task<List<Post>> GetPostsByCategoryAsync(int categoryId);
    }
}
