using BloggingPlatform.Application.DTOs.Post;
using PersonalBloggingPlatform.Domain.Entities;
using AutoMapper;
using BloggingPlatform.Application.Interfaces.Services;
using BloggingPlatform.Application.Interfaces.Repositories;
using BloggingPlatform.Application.Exceptions;
namespace Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        // Constructor injection for dependencies
        public PostService(IMapper mapper, IPostRepository repository, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _postRepository = repository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Post> CreateAsync(CreatePostRequest request)
        {
            var post = _mapper.Map<Post>(request);
            return await _postRepository.CreateAsync(post);
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                throw new NotFoundException($"Post with ID {id} not found.");
            }
            await _postRepository.DeleteAsync(id);
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
                throw new NotFoundException($"Post with ID {id} not found.");
            }
            return post ;
        }

        public async Task<List<Post>> GetPostsByCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if(category == null)
                throw new NotFoundException($"No category found for category ID {categoryId}.");
            var posts = await _postRepository.GetPostsByCategoryAsync(categoryId);
            if (posts.Count == 0)
                throw new NotFoundException($"No posts found for category ID {categoryId}.");
            return posts;
        }

        public async Task<Post> UpdateAsync(UpdatePostRequest request)
        {
            var existingPost = await _postRepository.GetByIdAsync(request.Id);
            if (existingPost == null)
            {
                throw new NotFoundException($"Post with ID {request.Id} not found.");
            }
            _mapper.Map(request, existingPost);
            existingPost.UpdatedAt = DateTime.UtcNow;
            
            return await _postRepository.UpdateAsync(existingPost);
        }
    }
}
