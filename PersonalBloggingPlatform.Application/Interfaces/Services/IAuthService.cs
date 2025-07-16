using BloggingPlatform.Application.DTOs.User;
using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task <UserResponse> RegisterUserAsync(RegisterUserRequest registerUser);
        Task <string> LoginUserAsync(LoginRequest loginRequest);
        Task<UserResponse?> GetUserByIdAsync(int id);
    }
}