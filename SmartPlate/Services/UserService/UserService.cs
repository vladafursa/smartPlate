using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SmartPlate.Data;
using SmartPlate.DTOs.User;
using SmartPlate.Helpers;
using SmartPlate.Models;
using BCrypt.Net;

namespace SmartPlate.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwt;

        // Constructor with dependency injection
        public UserService(AppDbContext context, IOptions<JwtSettings> jwtOptions)
        {
            _context = context;
            _jwt = jwtOptions.Value; // Get JwtSettings values from IOptions
        }

        // User registration
        public async Task<UserResponseDto?> RegisterAsync(UserRegisterDto dto)
        {
            // Check if a user with the same username or email already exists
            if (await _context.Users.AnyAsync(u => u.UserName == dto.Name || u.Email == dto.Email))
                return null;

            string hashedPassword = HashPassword(dto.Password);

            var user = User.Create(Guid.NewGuid(), dto.Name, hashedPassword, dto.Email, dto.Role);

            // Add the user to the context and save changes to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // Return a DTO with user info (without password)
            return new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }

        // Find the user by username
        public async Task<(UserResponseDto? user, string? token)> LoginAsync(UserLoginDto dto)
        {
            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return (null, null);

            // Verify the password
            bool isValidPassword = Verify(dto.Password, user.PasswordHash);
            if (!isValidPassword) return (null, null);

            var token = GenerateJwtToken(user);

            // Map User to UserResponseDto
            var userDto = new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };

            return (userDto, token);
        }

        // Hash a plain password using BCrypt
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verify a plain password against a hashed password
        public bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private string GenerateJwtToken(User user)
        {
            // Define claims (user info stored in the token)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Create a symmetric security key from the secret
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),// Token valid for 2 hours
                signingCredentials: creds
            );

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Role = u.Role
                })
                .ToListAsync();
        }
    }
}
