using AutoMapper;
using BloggingPlatform.Application.DTOs.User;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Application.Interfaces.Security;
using BloggingPlatform.Application.Interfaces.Services;
using PersonalBloggingPlatform.Domain.Entities;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        // Constructor injection for dependencies
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(id));
            }

            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> CreateAsync(RegisterUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            user.PasswordHash = _passwordHasher.HashPassword(request.Password);
            return await _userRepository.CreateAsync(user);
        }

        public async Task<User> UpdateAsync(UpdateUserRequest request)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId);
            if (existingUser == null)
            {
                throw new ArgumentException("User not found", nameof(request.UserId));
            }

            _mapper.Map(request ,existingUser);

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
            existingUser.PasswordHash =_passwordHasher.HashPassword(request.NewPassword);
            }
            return await _userRepository.UpdateAsync(existingUser);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(id));
            }
            return await _userRepository.DeleteAsync(id);
        }
    }
}
