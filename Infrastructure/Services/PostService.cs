using BloggingPlatform.Application.DTOs.Post;
using PersonalBloggingPlatform.Domain.Entities;
using AutoMapper;
using BloggingPlatform.Application.Interfaces.Services;
using BloggingPlatform.Application.Interfaces.Repositories;
using BloggingPlatform.Application.Exceptions;
using Microsoft.Extensions.Logging;
namespace Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<PostService> _logger;
        // Constructor injection for dependencies
        public PostService(IMapper mapper, IPostRepository repository, ICategoryRepository categoryRepository, ILogger<PostService> logger)
        {
            _mapper = mapper;
            _postRepository = repository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<Post> CreateAsync(CreatePostRequest request)
        {
            _logger.LogInformation("Creating a new post with title: {Title}", request.Title);

            var post = _mapper.Map<Post>(request);
            var created = await _postRepository.CreateAsync(post);

            _logger.LogInformation("Post created with ID: {Id}", created.Id);
            return created;
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting post with ID: {Id}", id);

            var post = await _postRepository.GetByIdAsync(id);

            if (post == null)
            {
                _logger.LogWarning("Post with ID: {Id} not found for deletion.", id);
                throw new NotFoundException($"Post with ID {id} not found.");
            }
            await _postRepository.DeleteAsync(id);

            _logger.LogInformation("Post with ID: {Id} deleted successfully.", id);
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _postRepository.GetAllAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null)
            {
                _logger.LogWarning("Post with ID {id} not found.", id);
                throw new NotFoundException($"Post with ID {id} not found.");
            }

            return post ;
        }

        public async Task<List<Post>> GetPostsByCategoryAsync(int categoryId)
        {
            _logger.LogInformation("Retrieving posts for category ID: {CategoryId}", categoryId);

            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                _logger.LogWarning("No category found for category ID {categoryId}.", categoryId);
                throw new NotFoundException($"No category found for category ID {categoryId}.");
            }

            var posts = await _postRepository.GetPostsByCategoryAsync(categoryId);

            if (posts.Count == 0)
            {
                _logger.LogWarning("No posts found for category ID {categoryId}.", categoryId);
                throw new NotFoundException($"No posts found for category ID {categoryId}.");
            }

            _logger.LogInformation("Found {Count} posts for category ID: {CategoryId}", posts.Count, categoryId);
            return posts;
        }

        public async Task<Post> UpdateAsync(UpdatePostRequest request)
        {
            _logger.LogInformation("Updating post with ID: {PostId}", request.Id);

            var existingPost = await _postRepository.GetByIdAsync(request.Id);

            if (existingPost == null)
            {
                _logger.LogWarning("Post with ID {request.Id} not found for update.",request.Id);
                throw new NotFoundException($"Post with ID {request.Id} not found.");
            }

            _mapper.Map(request, existingPost);
            existingPost.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Post with ID: {Id} updated successfully.", existingPost.Id);
            return await _postRepository.UpdateAsync(existingPost);
        }
    }
}