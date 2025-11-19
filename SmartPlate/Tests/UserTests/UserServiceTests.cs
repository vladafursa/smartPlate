using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartPlate.Data;
using SmartPlate.DTOs.User;
using SmartPlate.Helpers;
using SmartPlate.Services.UserService;


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
                Key = "TestSecretKey1234567890",
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
    }
}
