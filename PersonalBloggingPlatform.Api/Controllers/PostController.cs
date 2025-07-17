using System.Security.Claims;
using BloggingPlatform.Application.DTOs.Post;
using BloggingPlatform.Application.Interfaces.Services;
using BloggingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
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
            if (createPostDto == null)
            {
                throw new ArgumentNullException(nameof(createPostDto), "Post data cannot be null");
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            createPostDto.UserId = currentUserId;

            var createdPost = await _postService.CreateAsync(createPostDto);
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest updatePostDto)
        {
            if (updatePostDto == null)
            {
                throw new ArgumentException("Invalid category ID", nameof(updatePostDto.Id));
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var existingPost = await _postService.GetByIdAsync(updatePostDto.Id);

            if (existingPost == null)
            {
                return NotFound($"Post with ID {updatePostDto.Id} not found.");
            }

            if (existingPost.UserId != currentUserId) 
            {
                return Unauthorized("You are not authorized to update this post.");
            }

            var updatedPost = await _postService.UpdateAsync(updatePostDto);
            return Ok(updatedPost);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid category ID", nameof(id));
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var post = await _postService.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound($"Post with ID {id} not found.");
            }

            if (post.UserId != currentUserId)
            {
                return Unauthorized("You are not authorized to delete this post.");
            }

            
            await _postService.DeleteAsync(id);
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
