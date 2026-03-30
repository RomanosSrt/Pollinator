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

        [HttpPost(Name = "addUser")]
        public ActionResult<UserDto> CreateUser([FromBody] UserDto User)
        {
            var createdUser = _userService.CreateUser(User);

            //return CreatedAtAction(nameof(createdUser), new { id = User.id }, User);
            return Ok(createdUser);
        }
    }
}
