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

            // If the username or email already exists, return 400 Bad Request
            if (user == null)
                return BadRequest("Username or Email already exists.");
            // Otherwise, return 200 OK with the created user data
            return Ok(user);
        }

        // login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var (user, token) = await _userService.LoginAsync(dto);

            // If login fails  - 401 Unauthorized
            if (user == null || token == null)
                return Unauthorized("Invalid credentials.");

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // ensures cookie is sent over HTTPS only
                SameSite = SameSiteMode.Strict, // prevents sending cookie on cross-site requests
                Expires = DateTime.UtcNow.AddHours(2) //  cookie expires in 2 hours
            });
            // Return 200 OK with the user data
            return Ok(user);
        }

        //logout
        [Authorize]// User must be authenticated
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Delete the JWT cookie to log the user out
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(new { message = "Logged out successfully." });
        }

    }
}
