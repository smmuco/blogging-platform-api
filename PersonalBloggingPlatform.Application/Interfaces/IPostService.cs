using BloggingPlatform.Application.DTOs.Post;
using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int id);
        Task<Post> CreateAsync(CreatePostRequest request);
        Task<Post> UpdateAsync(UpdatePostRequest request);
        Task<bool> DeleteAsync(int id);
        Task<List<Post>> GetPostsByCategoryAsync(int categoryId);
    }
}
