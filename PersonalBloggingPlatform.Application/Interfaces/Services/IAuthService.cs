using BloggingPlatform.Application.DTOs.User;

namespace BloggingPlatform.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> GenerateJwtTokenAsync(string username, string password);
        Task<bool> ValidateUserAsync(string username, string password);
        Task<string> RefreshTokenAsync(string token);
        Task RevokeTokenAsync(string token);
        Task<bool> IsTokenValidAsync(string token); 
        Task LoginUserAsync(LoginRequest loginRequest);
        Task GenerateUserAsync(RegisterUserRequest registerUser);
    }
}
