using BloggingPlatform.Application.DTOs.Post;
using BloggingPlatform.Application.Interfaces.Services;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid post ID");
            }
            var post = await _postService.GetByIdAsync(id);
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest createPostDto)
        {
            if (createPostDto == null)
            {
                return BadRequest("Post data is null");
            }

            var createdPost = await _postService.CreateAsync(createPostDto);
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest updatePostDto)
        {
            if (updatePostDto == null)
            {
                return BadRequest("Post data is null");
            }

            var updatedPost = await _postService.UpdateAsync(updatePostDto);
            return Ok(updatedPost);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid post ID");
            }
            await _postService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetPostsByCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                return BadRequest("Invalid category ID");
            }
            var posts = await _postService.GetPostsByCategoryAsync(categoryId);
            return Ok(posts);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllAsync();
            return Ok(posts);
        }
    }
}
