using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using SmartPlate.Controllers;
using SmartPlate.DTOs.User;
using SmartPlate.Models;
using SmartPlate.Services.UserService;

namespace SmartPlate.Tests.UserTests
{
    public class UserControllerTests
    {
        private readonly UserController _userController;
        private readonly Mock<IUserService> _userServiceMock;

        public UserControllerTests()
        {
            //Dependencies
            _userServiceMock = new Mock<IUserService>();

            //SUT
            _userController = new UserController(_userServiceMock.Object);
        }

        //registration tests
        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsCreated()
        {
            // Arrange
            var dto = new UserRegisterDto
            {
                Name = "test",
                Password = "password",
                Email = "test@test.com",
                Role = "User"
            };

            var createdUserResponse = new UserResponseDto
            {
                UserName = "test",
                Email = "test@test.com",
                Role = "User"
            };

            _userServiceMock
                .Setup(s => s.RegisterAsync(dto))
                .ReturnsAsync(createdUserResponse);

            // Act
            var result = await _userController.Register(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal("test", returnedUser.UserName);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenUserIsNull()
        {
            // Arrange
            var dto = new UserRegisterDto
            {
                Name = "test",
                Password = "password",
                Email = "test@test.com",
                Role = "User"
            };

            _userServiceMock
                .Setup(s => s.RegisterAsync(dto))
                .ReturnsAsync((UserResponseDto)null);

            // Act
            var result = await _userController.Register(dto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username or Email already exists.", badRequestResult.Value);
        }

        //login tests
        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var dto = new UserLoginDto { Email = "test@test.com", Password = "password" };
            var userResponse = new UserResponseDto { UserName = "test", Email = "test@test.com", Role = "User" };
            var token = "fake-jwt-token";

            _userServiceMock
                .Setup(s => s.LoginAsync(dto))
                .ReturnsAsync((userResponse, token));

            var httpContext = new DefaultHttpContext();
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _userController.Login(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal("test", returnedUser.UserName);
            //Cookie check
            Assert.Contains("jwt=", httpContext.Response.Headers["Set-Cookie"].ToString());
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenTokenIsNull()
        {
            // Arrange
            var dto = new UserLoginDto { Email = "test@test.com", Password = "password" };
            var userResponse = new UserResponseDto { UserName = "test", Email = "test@test.com", Role = "User" };

            _userServiceMock
                .Setup(s => s.LoginAsync(dto))
                .ReturnsAsync((userResponse, null));

            var httpContext = new DefaultHttpContext();
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _userController.Login(dto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid credentials.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenTokenAndUserIsNull()
        {
            // Arrange
            var dto = new UserLoginDto { Email = "test@test.com", Password = "password" };

            _userServiceMock
                .Setup(s => s.LoginAsync(dto))
                .ReturnsAsync((null, null));

            var httpContext = new DefaultHttpContext();
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _userController.Login(dto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid credentials.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenUserIsNull()
        {
            // Arrange
            var dto = new UserLoginDto { Email = "test@test.com", Password = "password" };
            var token = "fake-jwt-token";

            _userServiceMock
                .Setup(s => s.LoginAsync(dto))
                .ReturnsAsync((null, token));

            var httpContext = new DefaultHttpContext();
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _userController.Login(dto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid credentials.", unauthorizedResult.Value);
        }

        //logout tests
        [Fact]
        public void Logout_ShouldDeleteJwtCookie_AndReturnOk()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var controller = new UserController(_userServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            // Act
            var result = controller.Logout();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseMessage = okResult.Value as dynamic;
            Assert.Equal("Logged out successfully.", (string)responseMessage.message);

            // Assert
            var setCookieHeader = httpContext.Response.Headers["Set-Cookie"].ToString();
            Assert.Contains("jwt=", setCookieHeader);
            Assert.Contains("expires=", setCookieHeader.ToLower());
        }
    }
}