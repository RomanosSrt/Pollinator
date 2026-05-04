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
    }
}
