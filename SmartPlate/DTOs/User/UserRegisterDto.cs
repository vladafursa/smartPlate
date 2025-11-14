using System.ComponentModel.DataAnnotations;


namespace SmartPlate.DTOs.User
{
    public record class UserRegisterDto
    {
        [Required][StringLength(50)] string name;
        [Required][StringLength(20)] string password;
        [Required][StringLength(20)][EmailAddress] string email;
        [StringLength(20)] string role = "User";
    }
}