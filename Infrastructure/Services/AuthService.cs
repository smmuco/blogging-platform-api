using System.Security.Authentication;
using AutoMapper;
using BloggingPlatform.Application.DTOs.User;
using BloggingPlatform.Application.Exceptions;
using BloggingPlatform.Application.Interfaces.Repositories;
using BloggingPlatform.Application.Interfaces.Security;
using BloggingPlatform.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using PersonalBloggingPlatform.Domain.Entities;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        public AuthService(IAuthRepository authRepository, IMapper mapper, ILogger<AuthService> logger, IPasswordHasher hasher, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = hasher;
            _tokenService = tokenService;
        }

        public async Task<UserResponse> RegisterUserAsync(RegisterUserRequest registerUser)
        {
            _logger.LogInformation("Registering user with email: {Email}", registerUser.Email);

            if (registerUser.Password != registerUser.ConfirmPassword)
            {
                _logger.LogWarning("Password and Confirm Password do not match for email: {Email}", registerUser.Email);
                throw new ArgumentException("Password and Confirm Password do not match.");
            }

            if (await _authRepository.GetUserByEmailAsync(registerUser.Email) != null)
            {
                _logger.LogWarning("User with email {Email} already exists", registerUser.Email);
                throw new AuthenticationException("User with this email already exists.");
            }

            var newUser = _mapper.Map<User>(registerUser);
            newUser.PasswordHash = _passwordHasher.HashPassword(registerUser.Password);
            await _authRepository.RegisterAsync(newUser);

            _logger.LogInformation("User registered with ID: {Id}", newUser.Id);
            return _mapper.Map<UserResponse>(newUser);
        }

        public async Task<string> LoginUserAsync(LoginRequest loginRequest)
        {
            _logger.LogInformation("Logging in user with email: {Email}", loginRequest.Username);

            var user = await _authRepository.GetUserByEmailAsync(loginRequest.Username);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found", loginRequest.Username);
                throw new AuthenticationException("Invalid email or password.");

            }

            var passwordchech = _passwordHasher.VerifyHashedPassword(loginRequest.Password, user.PasswordHash);

            if (!passwordchech)
            {
                _logger.LogWarning("Invalid password for user with email {Email}", loginRequest.Email);
                throw new AuthenticationException("Invalid email or password.");
            }
            _logger.LogInformation("User logged in with ID: {Id}", user.Id);
            return _tokenService.GenerateToken(user);
        }
        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving user with ID: {Id}", id);
            var user = await _authRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found", id);
                throw new NotFoundException ($"User with ID {id} not found.");
            }
            return _mapper.Map<UserResponse>(user);
        }
    }
}
