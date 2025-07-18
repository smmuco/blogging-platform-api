﻿using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task <List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int id);
        Task<Post> CreateAsync(Post post);
        Task<Post> UpdateAsync(Post post);
        Task DeleteAsync(int id);
        Task<List<Post>> GetPostsByCategoryAsync(int categoryId);
    }
}
