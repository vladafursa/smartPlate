using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartPlate.Data;
using SmartPlate.DTOs.User;
using SmartPlate.Helpers;
using SmartPlate.Services.UserService;
using System.Threading.Tasks;

namespace SmartPlate.Tests.UserTests
{
    public class UserServiceTests
    {
        private readonly UserService _service;
        private readonly UserDbContext _context;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            var jwtOptions = Options.Create(new JwtSettings
            {
                Key = "ThisIsASuperLongTestSecretKey12345678901234567890",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpireMinutes = 60
            });

            _context = new UserDbContext(options);
            _service = new UserService(_context, jwtOptions);
        }

        //registration tests
        [Fact]
        public async Task Register_ShouldCreateUser()
        {
            //Arrange
            var dto = new UserRegisterDto
            {
                Name = "testuser",
                Password = "password123",
                Email = "test@example.com",
                Role = "User"
            };

            //Act
            var user = await _service.RegisterAsync(dto);

            //Assert
            Assert.NotNull(user);
            Assert.Equal("testuser", user!.UserName);
        }

        [Fact]
        public async Task Register_ShouldReturnNull_WhenUserAlreadyExists()
        {
            //Arrange
            var dto = new UserRegisterDto
            {
                Name = "duplicateuser",
                Password = "password123",
                Email = "duplicate@example.com",
                Role = "User"
            };

            //Act
            var firstUser = await _service.RegisterAsync(dto);
            var secondUser = await _service.RegisterAsync(dto);

            //Assert
            Assert.NotNull(firstUser);
            Assert.Null(secondUser);
        }

        [Fact]
        public async Task Register_ShouldHashPassword()
        {
            //Arrange
            var dto = new UserRegisterDto
            {
                Name = "secureuser",
                Password = "mypassword",
                Email = "secure@example.com",
                Role = "User"
            };

            //Act
            var userResponse = await _service.RegisterAsync(dto);
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == userResponse!.Id);

            //Assert
            Assert.NotNull(userInDb);
            Assert.NotEqual(dto.Password, userInDb!.PasswordHash);
        }

        //login tests
        [Fact]
        public async Task Login_ShouldReturnUserAndToken_WhenCredentialsAreValid()
        {
            // Arrange
            var registerDto = new UserRegisterDto
            {
                Name = "loginuser",
                Password = "password123",
                Email = "login@example.com",
                Role = "User"
            };


            var loginDto = new UserLoginDto
            {
                Email = "login@example.com",
                Password = "password123"
            };

            // Act
            await _service.RegisterAsync(registerDto);
            var (user, token) = await _service.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(user);
            Assert.Equal("loginuser", user!.UserName);
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public async Task Login_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            //Arrange
            var loginDto = new UserLoginDto
            {
                Email = "doesnotexist@example.com",
                Password = "password123"
            };

            //Act
            var (user, token) = await _service.LoginAsync(loginDto);

            //Assert
            Assert.Null(user);
            Assert.Null(token);
        }

        [Fact]
        public async Task Login_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            // Arrange
            var registerDto = new UserRegisterDto
            {
                Name = "wrongpassuser",
                Password = "correctpassword",
                Email = "wrongpass@example.com",
                Role = "User"
            };

            var loginDto = new UserLoginDto
            {
                Email = "wrongpass@example.com",
                Password = "incorrectpassword"
            };

            // Act
            await _service.RegisterAsync(registerDto);
            var (user, token) = await _service.LoginAsync(loginDto);

            // Assert
            Assert.Null(user);
            Assert.Null(token);
        }

        [Fact]
        public async Task Login_ShouldGenerateToken_WhenCredentialsAreValid()
        {
            //Arrange
            var registerDto = new UserRegisterDto
            {
                Name = "tokenuser",
                Password = "password123",
                Email = "token@example.com",
                Role = "User"
            };

            var loginDto = new UserLoginDto
            {
                Email = "token@example.com",
                Password = "password123"
            };

            //Act
            await _service.RegisterAsync(registerDto);
            var (_, token) = await _service.LoginAsync(loginDto);

            //Assert
            Assert.NotNull(token);
            Assert.StartsWith("ey", token);
        }

        //Hash function test
        [Fact]
        public void HashPassword_And_Verify_ShouldWorkTogether()
        {
            //Arrange
            var plain = "mypassword";

            //Act
            var hashed = _service.HashPassword(plain);

            //Assert
            Assert.True(_service.Verify(plain, hashed));
            Assert.False(_service.Verify("wrongpassword", hashed));
        }

    }
}
