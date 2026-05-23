using API.Application.DTOs.HealtCheck;
using API.Application.DTOs.UserInsertion;
using API.Application.Services.Interfaces;
using API.Domain.Entities.UserManagement;
using API.Infrastructure.Persistence;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(typeof(ApiResponse<IsHealthy>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IsHealthy> HealthCheck()
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
            return new IsHealthy();
        }
    }
}
