using API.Application.DTOs.PlotHandling;
using API.Application.DTOs.PlotHnadling;
using API.Application.Services.Interfaces;
using API.Domain.Entities.PlotManagement;
using API.Infrastructure.Persistence;
using API.Infrastructure.Repositories.Interfaces;
using AutoMapper;

namespace API.Application.Services.Implementation
{
    public class PlotService : IPlotService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PlotService> _logger;
        private readonly IPlotRepository _plotrepository;

        public PlotService(IMapper mapper, ILogger<PlotService> logger, IPlotRepository plotRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _plotrepository = plotRepository;
        }

        public async Task<PlotDto?> GetPlot(string KAEK)
        {
            var plot = await _plotrepository.GetByKAEK(KAEK);
            if (plot == null)
            {
                _logger.LogWarning("No plot found with KAEK: {KAEK}", KAEK);
                return null;            // TODO: Throw an exception or return a Result type to better handle this case.
            }
            return _mapper.Map<PlotDto>(plot);
        }

        public async Task<List<PlotDto?>> GetPlotsbyClaim(bool isClaimed)
        {
            var claimedPlots = await _plotrepository.GetByClaim(isClaimed);
            if (claimedPlots == null || !claimedPlots.Any())
            {
                _logger.LogInformation("No plots found with isClaimed: {isClaimed}", isClaimed);
                throw new NullReferenceException($"No plots found with isClaimed: {isClaimed}");
            }
            return _mapper.Map<List<PlotDto?>>(claimedPlots);
        }

        public async Task<List<PlotDto?>> GetPlotsbyCropType(List<CropType> cropTypes)
        {
            var selectedPlots = await _plotrepository.GetByCropType(cropTypes);
            if (selectedPlots == null || !selectedPlots.Any())
            {
                _logger.LogInformation("No plots found with crop types: {cropTypes}", string.Join(", ", cropTypes));
                throw new NullReferenceException($"No plots found with crop types: {string.Join(", ", cropTypes)}");
            }
            return _mapper.Map<List<PlotDto?>>(selectedPlots);
        }


        #region InitPlots
        public List<Plot> AddPlots(EsriJsonRoot data)
        {
            var plots = _mapper.Map<List<Plot>>(data.features);
            var random = new Random();

            foreach (var plot in plots)
            {
                if (random.Next(2) == 0) 
                { 
                    plot.isClaimed = true;
                    plot.farmerId = random.Next(0, 49);
                }
            }
            return _plotrepository.Add(plots);
        }
        #endregion
    }
}
