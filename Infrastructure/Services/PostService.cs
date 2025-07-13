using BloggingPlatform.Application.DTOs.Post;
using PersonalBloggingPlatform.Domain.Entities;
using AutoMapper;
using BloggingPlatform.Application.Interfaces.Services;
using BloggingPlatform.Application.Interfaces.Repositories;
namespace Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        // Constructor injection for dependencies
        public PostService(IMapper mapper, IPostRepository repository)
        {
            _mapper = mapper;
            _postRepository = repository;
        }
        public async Task<Post> CreateAsync(CreatePostRequest request)
        {
            var post = _mapper.Map<Post>(request);
            return await _postRepository.CreateAsync(post);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return false; // Invalid ID
            }
            if (await _postRepository.GetByIdAsync(id) == null)
            {
                return false; // Post not found
            }
            await _postRepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _postRepository.GetAllAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null; // Invalid ID
            }
            var post = await _postRepository.GetByIdAsync(id);
            if ( post == null)
            {
                return null; // Post not found
            }
            return post ;
        }

        public async Task<List<Post>> GetPostsByCategoryAsync(int categoryId)
        {
            if (categoryId <= 0)
            {
                return new List<Post>(); // Invalid category ID
            }
            var posts = await _postRepository.GetPostsByCategoryAsync(categoryId);
            if (posts == null)
            {
                return new List<Post>(); // No posts found for the category
            }
            return posts;
        }

        public async Task<Post> UpdateAsync(UpdatePostRequest request)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Invalid post ID.");
            } 
            var existingPost = await _postRepository.GetByIdAsync(request.Id);
            if (existingPost == null)
            {
                throw new KeyNotFoundException($"Post with ID {request.Id} not found.");
            }
            _mapper.Map(request, existingPost);
            existingPost.UpdatedAt = DateTime.UtcNow;
            
            return await _postRepository.UpdateAsync(existingPost);
        }
    }
}
