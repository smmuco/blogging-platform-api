using BloggingPlatform.Application.DTOs.User;
using BloggingPlatform.Application.Interfaces.Services;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        public Task<string> GenerateJwtTokenAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task GenerateUserAsync(RegisterUserRequest registerUser)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsTokenValidAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task LoginUserAsync(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }

        public Task<string> RefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task RevokeTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
