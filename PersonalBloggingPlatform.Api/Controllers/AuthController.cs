using BloggingPlatform.Application.DTOs.User;
using BloggingPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BloggingPlatform.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Attempting to log in user with email: {Email}", request.Username);

            var token = await _authService.LoginUserAsync(request);

            _logger.LogInformation("User logged in successfully with email: {Email}", request.Username);

            return Ok(new { Token = token });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            _logger.LogInformation("Attempting to register user with email: {Email}", request.Email);

            var user = await _authService.RegisterUserAsync(request);

            _logger.LogInformation("User registered successfully with email: {Email}", request.Email);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
    }
}