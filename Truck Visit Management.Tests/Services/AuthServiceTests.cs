using Xunit;
using NSubstitute;
using System.Threading.Tasks;
using Truck_Visit_Management.Data;
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Entities;
using Truck_Visit_Management.Services.ServiceImpl;
using Truck_Visit_Management.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Truck_Visit_Management.Repositories;

namespace Truck_Visit_Management.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public AuthServiceTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _authService = new AuthService(_userRepository, Substitute.For<IJwtUtils>());
        }

        [Fact]
        public void Authenticate_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser", PasswordHash = PasswordHasher.HashPassword("password") };
            _userRepository.GetByUsername("testuser").Returns(user);

            var loginDto = new UserLoginDto { Username = "testuser", Password = "password" };

            // Act
            var token = _authService.Authenticate(loginDto);

            // Assert
            Assert.NotNull(token);
            // Add more assertions to validate token content, if needed
        }

        [Fact]
        public void Authenticate_InvalidUsername_ReturnsNull()
        {
            // Arrange
            _userRepository.GetByUsername("nonexistentuser").Returns((User)null);

            var loginDto = new UserLoginDto { Username = "nonexistentuser", Password = "password" };

            // Act
            var token = _authService.Authenticate(loginDto);

            // Assert
            Assert.Null(token);
        }

        [Fact]
        public void Authenticate_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser", PasswordHash = PasswordHasher.HashPassword("password") };
            _userRepository.GetByUsername("testuser").Returns(user);

            var loginDto = new UserLoginDto { Username = "testuser", Password = "wrongpassword" };

            // Act
            var token = _authService.Authenticate(loginDto);

            // Assert
            Assert.Null(token);
        }

        [Fact]
        public async Task Register_ValidModel_SavesUser()
        {
            // Arrange
            var registerDto = new UserRegisterDto { Username = "newuser", Password = "newpassword", Role = "User" };

            // Act
            await _authService.Register(registerDto);

            // Assert
            await _userRepository.Received(1).AddUserAsync(Arg.Is<User>(u => u.Username == "newuser"));
            // Add more assertions if needed, e.g., check if password was hashed
        }

        [Fact]
        public async Task Register_DuplicateUsername_ThrowsException()
        {
            // Arrange
            var existingUser = new User { Username = "existinguser", PasswordHash = PasswordHasher.HashPassword("existingpassword") };
            _userRepository.UsernameExistsAsync("existinguser").Returns(true);

            var registerDto = new UserRegisterDto { Username = "existinguser", Password = "newpassword", Role = "User" };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _authService.Register(registerDto));
            Assert.Equal("Username is already taken", ex.Message);
        }

        [Fact]
        public void GetById_ValidId_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser", PasswordHash = PasswordHasher.HashPassword("password") };
            _userRepository.GetById(1).Returns(user);

            // Act
            var result = _authService.GetById(1);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public void GetById_InvalidId_ReturnsNull()
        {
            // Arrange
            _userRepository.GetById(999).Returns((User)null); // Assuming user with ID 999 doesn't exist

            // Act
            var result = _authService.GetById(999);

            // Assert
            Assert.Null(result);
        }
    }
}