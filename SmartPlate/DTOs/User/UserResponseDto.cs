namespace SmartPlate.DTOs.User
{
    public record class UserResponseDto
    {
        public Guid Id { get; init; }
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Role { get; init; } = null!;
    }
}
