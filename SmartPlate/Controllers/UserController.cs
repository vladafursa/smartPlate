using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPlate.DTOs.User;
using SmartPlate.Services.UserService;

namespace SmartPlate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);
            return Ok(result);
        }

        // login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var (user, token) = await _userService.LoginAsync(dto);

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(2)
            });

            return Ok(new { user, token });
        }

        //logout
        [Authorize]// user must be authenticated
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // delete the JWT cookie to log the user out
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(new { message = "Logged out successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
