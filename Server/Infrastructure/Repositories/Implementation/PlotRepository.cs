using API.Domain.Entities.PlotManagement;
using API.Domain.Entities.UserManagement;
using API.Infrastructure.Persistence;
using API.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Repositories.Implementation
{
    public class PlotRepository : IPlotRepository
    {
        private readonly AppDbContext _context;
    
        public PlotRepository(AppDbContext context)
        {
            _context = context;
        }
    
        public async Task<Plot?> GetByKAEK(string KAEK)
        {
            return await _context.Plots.SingleOrDefaultAsync(p => p.kaek == KAEK);
        }

        public List<Plot> Add(List<Plot> plots)
        {
            foreach (var plot in plots)
            {
                _context.Plots.Add(plot);
            }
            _context.SaveChanges();
            return plots;
        }

        public async Task<List<Plot>?> GetByClaim(bool isClaimed)
        {
            var plots = await _context.Plots.Where(p => p.isClaimed == isClaimed).ToListAsync();
            return plots;
        }

        public async Task<List<Plot>?> GetByCropType(List<CropType> cropTypes)
        {
            var plots = await _context.Plots.Where(p => p.cropTypes != null).ToListAsync();
            var filteredPlots = plots.Where(p => p.cropTypes != null && p.cropTypes.All(ct => cropTypes.Contains(ct))).ToList();
            return filteredPlots;
        }
    }
}
