using System.Security.Authentication;
using AutoMapper;
using BloggingPlatform.Application.DTOs.User;
using BloggingPlatform.Application.Interfaces.Repositories;
using BloggingPlatform.Application.Interfaces.Security;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalBloggingPlatform.Domain.Entities;

namespace Tests.Services
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;
        private readonly Mock<IAuthRepository> _authRepositoryMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<ILogger<AuthService>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public AuthServiceTests()
        {
            _authService = new AuthService(
                _authRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _passwordHasherMock.Object,
                _tokenServiceMock.Object
            );
        }

        [Fact]
        public async Task RegisterUserAsync_WhenDataIsValid_ShouldReturnUserResponse()
        {
            // Arrange
            var registerRequest = new RegisterUserRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };

            var user = new User
            {
                Id = 1,
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                PasswordHash = "hashedPassword"
            };

            _authRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(registerRequest.Email))
                .ReturnsAsync((User?)null);

            _mapperMock
                .Setup(m => m.Map<User>(registerRequest))
                .Returns(user);

            _passwordHasherMock
                .Setup(h => h.HashPassword(registerRequest.Password))
                .Returns("hashedPassword");

            _authRepositoryMock
                .Setup(r => r.RegisterAsync(user))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.RegisterUserAsync(registerRequest);

            // Assert 
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task RegisterUserAsync_WhenPasswordsDoNotMatch_ShouldThrowArgumentException()
        {
            // Arrange
            var registerRequest = new RegisterUserRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                ConfirmPassword = "DifferentPassword123"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _authService.RegisterUserAsync(registerRequest));

            Assert.Equal("Password and Confirm Password do not match.", exception.Message);
        }

        [Fact]
        public async Task RegisterUserAsync_WhenUserAlreadyExists_ShouldThrowArgumentException()
        {
            // Arrange
            var registerRequest = new RegisterUserRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };

            var existingUser = new User
            {
                Id = 1,
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                PasswordHash = "hashedPassword"
            };

            _authRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(registerRequest.Email))
                .ReturnsAsync(existingUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _authService.RegisterUserAsync(registerRequest));

            Assert.Equal("User with this email already exists.", exception.Message);
        }

        [Fact]
        public async Task LoginUserAsync_WhenDataIsValid_ShouldReturnToken() 
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "Password123"
            };

            var user = new User
            {
                Email = loginRequest.Email,
                PasswordHash = "hashedPassword"
            };
        
            _authRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(loginRequest.Email))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(h => h.VerifyHashedPassword(loginRequest.Password, user.PasswordHash))
                .Returns(true);

            _tokenServiceMock
                .Setup(t => t.GenerateToken(user))
                .Returns("generatedToken");

            // Act
            var result = await _authService.LoginUserAsync(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("generatedToken", result);
        }

        [Fact]
        public async Task LoginUserAsync_WhenEmailIsNotFound_ShouldThrowAuthenticationException()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "Password123"
            };

            _authRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(loginRequest.Email))
                .ReturnsAsync((null as User));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AuthenticationException>(
                () => _authService.LoginUserAsync(loginRequest));
            Assert.Equal("Invalid email or password.", exception.Message);
        }

        [Fact]
        public async Task LoginUserAsync_WhenInvalidPassword_ShouldThrowAuthenticationException()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };

            var user = new User
            {
                Email = loginRequest.Email,
                PasswordHash = "hashedPassword"
            };

            _authRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(loginRequest.Email))
                .ReturnsAsync(user);
            _passwordHasherMock
                .Setup(h => h.VerifyHashedPassword(loginRequest.Password, user.PasswordHash))
                .Returns(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AuthenticationException>(
                () => _authService.LoginUserAsync(loginRequest));
            Assert.Equal("Invalid email or password.", exception.Message);
        }
    }
}