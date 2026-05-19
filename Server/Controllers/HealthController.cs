using API.Application.Services.Interfaces;
using API.Application.DTOs.UserInsertion;
using API.Domain.Entities.UserManagement;
using Microsoft.AspNetCore.Mvc;
using API.Infrastructure.Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly AppDbContext _appDbContext;

        public HealthController(ILogger<UserController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }


        [HttpGet()]
        public async Task<IActionResult> HealthCheck()
        {
            try
            {
                bool dbCheck = await _appDbContext.Database.CanConnectAsync();
                if (!dbCheck)
                {
                    _logger.LogError("Database connection failed during health check.");
                    throw new Exception("Database connection failed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection failed during health check.");
                throw new Exception("Database connection failed");
            }
            return Ok(new { status = "Pollinator is healthy!"});
        }
    }
}
