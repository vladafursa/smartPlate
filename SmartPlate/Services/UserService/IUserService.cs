using SmartPlate.DTOs.User;
using SmartPlate.Helpers;

namespace SmartPlate.Services.UserService
{
    public interface IUserService
    {
        Task<UserResponseDto?> RegisterAsync(UserRegisterDto dto);
        Task<(UserResponseDto? user, string? token)> LoginAsync(UserLoginDto dto);
        string HashPassword(string password);
        bool Verify(string password, string hashedPassword);
    }
}