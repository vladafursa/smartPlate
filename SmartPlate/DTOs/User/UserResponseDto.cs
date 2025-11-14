using System.ComponentModel.DataAnnotations;

namespace SmartPlate.DTOs.User
{
    public record class UserResponseDto
    {
        public Guid Id { get; init; }
        [Required] public string UserName { get; init; }
        [Required] public string Email { get; init; }
        [Required] public string Role { get; init; }
    }
}
