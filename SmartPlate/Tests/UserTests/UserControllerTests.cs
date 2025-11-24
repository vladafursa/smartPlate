using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartPlate.Controllers;
using SmartPlate.DTOs.User;
using SmartPlate.Models;
using SmartPlate.Helpers;
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
        public async Task Register_ShouldThrowException_WhenUserAlreadyExists()
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
                .ThrowsAsync(new ArgumentException("User already exists"));

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userController.Register(dto));

            // Assert
            Assert.Equal("User already exists", exception.Message);
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

            var result = await _userController.Login(dto);

            // Act
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;

            var userProp = value.GetType().GetProperty("user");
            var tokenProp = value.GetType().GetProperty("token");

            var returnedUser = (UserResponseDto)userProp!.GetValue(value)!;
            var returnedToken = (string)tokenProp!.GetValue(value)!;

            // Assert
            Assert.Equal("test", returnedUser.UserName);
            Assert.Equal("fake-jwt-token", returnedToken);
        }

        [Fact]
        public async Task Login_ShouldThrowUnauthorizedException_WhenUserDoesntExist()
        {
            // Arrange
            var dto = new UserLoginDto { Email = "nonexistent@test.com", Password = "password" };

            _userServiceMock
                .Setup(s => s.LoginAsync(dto))
                .ThrowsAsync(new UserNotFoundException());

            // Act 
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(() => _userController.Login(dto));

            //Assert
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async Task Login_ShouldThrowUnauthorizedException_WhenPasswordIsInvalid()
        {
            // Arrange
            var dto = new UserLoginDto { Email = "yser@test.com", Password = "invalid" };

            _userServiceMock
                .Setup(s => s.LoginAsync(dto))
                .ThrowsAsync(new InvalidPasswordException());

            // Act 
            var exception = await Assert.ThrowsAsync<InvalidPasswordException>(() => _userController.Login(dto));

            //Assert
            Assert.Equal("Invalid password.", exception.Message);
        }

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
            Assert.NotNull(okResult.Value);

            var messageProp = okResult.Value.GetType().GetProperty("message");
            Assert.NotNull(messageProp);
            var messageValue = messageProp.GetValue(okResult.Value) as string;
            Assert.Equal("Logged out successfully.", messageValue);

            var setCookieHeader = httpContext.Response.Headers["Set-Cookie"].ToString();
            Assert.Contains("jwt=", setCookieHeader);
            Assert.Contains("expires=", setCookieHeader.ToLower());
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkWithDtos()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(s => s.GetAllUsersAsync())
                .ReturnsAsync(new List<UserResponseDto>
                {
            new UserResponseDto
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Email = "admin@test.com",
                Role = "Admin"
            }
                });

            var controller = new UserController(mockService.Object);

            // Act
            var result = await controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsAssignableFrom<List<UserResponseDto>>(okResult.Value);
            Assert.Single(users);
            Assert.Equal("admin", users[0].UserName);
        }

    }
}