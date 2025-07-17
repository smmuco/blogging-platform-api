using System.Security.Claims;
using BloggingPlatform.Application.DTOs.Post;
using BloggingPlatform.Application.Interfaces.Services;
using BloggingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid category ID", nameof(id));
            }
            var post = await _postService.GetByIdAsync(id);
            return Ok(post);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest createPostDto)
        {
            _logger.LogInformation("Creating a new post with data: {@CreatePostDto}", createPostDto);
            if (createPostDto == null)
            {
                _logger.LogError("Post data is null when trying to create a new post.");
                throw new ArgumentNullException(nameof(createPostDto), "Post data cannot be null");
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            createPostDto.UserId = currentUserId;

            var createdPost = await _postService.CreateAsync(createPostDto);
            _logger.LogInformation("Post created with ID: {Id}", createdPost.Id);
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest updatePostDto)
        {
            _logger.LogInformation("Updating post with ID: {Id}", updatePostDto?.Id);

            if (updatePostDto == null)
            {
                _logger.LogError("Post data is null when trying to update a post.");
                throw new ArgumentException("Invalid category ID", nameof(updatePostDto.Id));
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                _logger.LogWarning("UserId claim is missing or invalid.");
                return Unauthorized("You are not authorized to update this post.");
            }

            var existingPost = await _postService.GetByIdAsync(updatePostDto.Id);

            if (existingPost == null)
            {
                _logger.LogWarning("Post with ID {Id} not found for update.", updatePostDto.Id);
                return NotFound($"Post with ID {updatePostDto.Id} not found.");
            }

            if (existingPost.UserId != currentUserId) 
            {
                _logger.LogWarning("User with ID {UserId} is not authorized to update post with ID {PostId}.", currentUserId, updatePostDto.Id);
                return Unauthorized("You are not authorized to update this post.");
            }

            var updatedPost = await _postService.UpdateAsync(updatePostDto);
            _logger.LogInformation("Post with ID {Id} updated successfully.", updatedPost.Id);
            return Ok(updatedPost);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            _logger.LogInformation("Attempting to delete post with ID: {Id}", id);
            if (id <= 0)
            {
                _logger.LogError("Invalid post ID: {Id}", id);
                throw new ArgumentException("Invalid category ID", nameof(id));
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                _logger.LogWarning("UserId claim is missing or invalid.");
                return Unauthorized("You are not authorized to delete this post.");
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var post = await _postService.GetByIdAsync(id);
            if (post == null)
            {
                _logger.LogWarning("Post with ID {Id} not found for deletion.", id);
                return NotFound($"Post with ID {id} not found.");
            }

            if (userRole != "Admin" && post.UserId != currentUserId)
            {
                _logger.LogWarning("User with ID {UserId} is not authorized to delete post with ID {PostId}.", currentUserId, id);
                return Unauthorized("You are not authorized to delete this post.");
            }

            
            await _postService.DeleteAsync(id);
            _logger.LogInformation("Post with ID {Id} deleted successfully.", id);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetPostsByCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                throw new ArgumentException("Invalid category ID", nameof(categoryId));
            }
            var posts = await _postService.GetPostsByCategoryAsync(categoryId);
            return Ok(posts);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllAsync();
            return Ok(posts);
        }
    }
}
