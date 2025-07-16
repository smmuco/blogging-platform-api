using AutoMapper;
using Azure.Core;
using BloggingPlatform.Application.DTOs.User;
using BloggingPlatform.Application.Exceptions;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Application.Interfaces.Security;
using BloggingPlatform.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using PersonalBloggingPlatform.Domain.Entities;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UserService> _logger;
        // Constructor injection for dependencies
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with id {id} not found.");
                throw new NotFoundException($"User with id {id} not found.");
            }
            return user;
        }

        //public async Task<User> CreateAsync(RegisterUserRequest request)
        //{
        //    _logger.LogInformation("Creating a new user with email: {Email}", request.Email);
            
        //    var user = _mapper.Map<User>(request);
        //    user.PasswordHash = _passwordHasher.HashPassword(request.Password);
        //    var created = await _userRepository.CreateAsync(user);

        //    _logger.LogInformation("User created with ID: {Id}", created.Id);
        //    return created;
        //}

        public async Task<User> UpdateAsync(UpdateUserRequest request)
        {
            _logger.LogInformation("Updating user with ID: {UserId}", request.UserId);

            var existingUser = await _userRepository.GetByIdAsync(request.UserId);

            if (existingUser == null)
            {
                _logger.LogWarning("User with ID: {UserId} not found for update.", request.UserId);
                throw new NotFoundException($"User with id {request.UserId} not found.");
            }

            _mapper.Map(request ,existingUser);

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
            existingUser.PasswordHash =_passwordHasher.HashPassword(request.NewPassword);
            }

            _logger.LogInformation("User with ID: {Id} updated successfully.", existingUser.Id);

            return await _userRepository.UpdateAsync(existingUser);
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting user with ID: {Id}", id);

            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User with ID: {Id} not found for deletion.", id);
                throw new NotFoundException($"User with id {id} not found.");
            }

            await _userRepository.DeleteAsync(id);
            
            _logger.LogInformation("User with ID: {Id} deleted successfully.", id);
        }
    }
}
