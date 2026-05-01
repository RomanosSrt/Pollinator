using API.Application.Services.Interfaces;
using API.Application.DTOs.UserInsertion;
using API.Domain.Entities.UserManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto User)
        {
            _logger.LogInformation("Registering user with email: {Email}", User.email);
            var user = await _userService.CreateUser(User);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation("Logging in user with email: {Email}", loginDto.Email);
            var token = await _userService.LoginUser(loginDto);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Invalid credentials");
            }
            return Ok(new { token = token });
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("No user found with email: {Email}", email);
                return NotFound("No user exists with the provided email");
            }
            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                _logger.LogWarning("No user found with username: {Username}", username);
                return NotFound("No user exists with the provided username");
            }
            return Ok(user);
        }
    }
}
