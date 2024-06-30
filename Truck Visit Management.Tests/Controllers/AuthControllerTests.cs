using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truck_Visit_Management.Controllers;
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Services.ServiceImpl;

namespace Truck_Visit_Management.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly IAuthService _authService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authService = Substitute.For<IAuthService>();
            _controller = new AuthController(_authService);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var loginDto = new UserLoginDto { Username = "testuser", Password = "password" };
            var expectedToken = "mocked.jwt.token";
            _authService.Authenticate(loginDto).Returns(expectedToken);

            // Act
            var result = _controller.Login(loginDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
           // Assert.Equal(expectedToken, result.Value);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new UserLoginDto { Username = "testuser", Password = "wrongpassword" };
            _authService.Authenticate(loginDto).Returns((string)null); // Simulate authentication failure

            // Act
            var result = _controller.Login(loginDto) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
         //   Assert.Equal("Username or password is incorrect", result.Value);
        }


        [Fact]
        public async Task Register_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var registerDto = new UserRegisterDto { Username = "newuser", Password = "password", Role = "Admin" };

            // Act
            var result = await _controller.Register(registerDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
           // Assert.Equal("User registered successfully", result.Value);
        }

        [Fact]
        public async Task Register_DuplicateUsername_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new UserRegisterDto { Username = "existinguser", Password = "password", Role = "User" };
            _authService.Register(registerDto).Returns(Task.FromException<UserRegisterDto>(new Exception("Username is already taken")));

            // Act
            var result = await _controller.Register(registerDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
         //   Assert.Equal("Username is already taken",result.Value );
        }

    }
}
