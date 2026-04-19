using API.Application.DTOs.PlotInsertion;
using API.Application.Services.Implementation;
using API.Application.Services.Interfaces;
using API.Domain.Entities.PlotManagement;
using API.Domain.Entities.UserManagement;
using API.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;
using System.Text.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CadastralController : ControllerBase
    {
        private readonly ILogger<CadastralController> _logger;
        private readonly IPlotService _plotservice;

        public CadastralController(ILogger<CadastralController> logger, IPlotService plotService)
        {
            _logger = logger;
            _plotservice = plotService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCadastralData(IFormFile file)
        {
            if (file == null || file.Length == 0 || file.ContentType != "application/json")
            {
                return BadRequest("No file uploaded or invalid file type.");
            }
            try
            {
                file.OpenReadStream().Position = 0;
                using var reader = new StreamReader(file.OpenReadStream());   
                var json = await reader.ReadToEndAsync();                                                                                          
                Console.WriteLine($"First 20 lines: {json.Substring(0, Math.Min(1000, json.Length))}");
                var data = JsonSerializer.Deserialize<EsriJsonRoot>(json);                                                                        
  
                if (data?.features == null || data.features.Count == 0)                                                                                  
                   return BadRequest("No features in file");                                                                                         
                                                                                                                                               
                var plots = _plotservice.AddPlots(data);

                // Transform 2100 → 4326                                                                                                           
                /*var transformed = await _dbContext.Database.ExecuteSqlRawAsync(
                    @"UPDATE ""Plots"" SET ""Polygon"" = ST_Transform(ST_SetSRID(""Polygon"", 2100), 4326)");
 */
                return Ok(new { Imported = plots.Count});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing cadastral data.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}