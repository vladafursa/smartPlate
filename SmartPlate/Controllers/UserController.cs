using Microsoft.AspNetCore.Mvc;
using SmartPlate.DTOs.User;
using SmartPlate.Services.UserService;
using Microsoft.AspNetCore.Authorization;

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
            var user = await _userService.RegisterAsync(dto);

            if (user == null)
                return BadRequest("Username or Email already exists.");

            return Ok(user);
        }

        // login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var (user, token) = await _userService.LoginAsync(dto);

            if (user == null || token == null)
                return Unauthorized("Invalid credentials.");

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(2)
            });

            return Ok(user);
        }

        //logout
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(new { message = "Logged out successfully." });
        }

        //test
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                User = User.Identity?.Name
            });
        }
    }
}
