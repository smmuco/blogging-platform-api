using System.Security.Claims;
using BloggingPlatform.Application.DTOs.User;
using BloggingPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService,ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid category ID", nameof(id));
            }
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest updateUserDto)
        {
            _logger.LogInformation("Updating user with data: {@UpdateUserDto}", updateUserDto);
            if (updateUserDto == null)
            {
                _logger.LogError("User data is null when trying to update user.");
                return BadRequest("User data is null");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                _logger.LogError("User ID claim is not valid: {UserIdClaim}", userIdClaim);
                return Unauthorized("You are not authorized.");
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Admin" && currentUserId != updateUserDto.UserId)
            {
                _logger.LogWarning("User with ID {UserId} is not authorized to update user with ID {UpdateUserId}.", currentUserId, updateUserDto.UserId);
                return Unauthorized("You are not authorized to update this user");
            }

            var updatedUser = await _userService.UpdateAsync(updateUserDto);
            _logger.LogInformation("User with ID {Id} updated successfully.", updatedUser.Id);

            return Ok(updatedUser);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("Attempting to delete user with ID: {Id}", id);
            if (id <= 0)
            {
                _logger.LogError("Invalid user ID: {Id}", id);
                throw new ArgumentException("Invalid user ID", nameof(id));
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                _logger.LogError("User ID claim is not valid: {UserIdClaim}", userIdClaim);
                return Unauthorized("You are not authorized.");
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Admin" && currentUserId != id)
            {
                _logger.LogWarning("User with ID {UserId} is not authorized to delete user with ID {DeleteUserId}.", currentUserId, id);
                return Unauthorized("You are not authorized to update this user");
            }

            await _userService.DeleteAsync(id);
            _logger.LogInformation("User with ID {Id} deleted successfully.", id);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }
    }
}
