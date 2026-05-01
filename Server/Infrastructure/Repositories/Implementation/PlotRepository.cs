using API.Domain.Entities.PlotManagement;
using API.Domain.Entities.UserManagement;
using API.Infrastructure.Persistence;
using API.Infrastructure.Repositories.Interfaces;

namespace API.Infrastructure.Repositories.Implementation
{
    public class PlotRepository : IPlotRepository
    {
        private readonly AppDbContext _context;
    
        public PlotRepository(AppDbContext context)
        {
            _context = context;
        }
    
        public Plot? GetByKAEK(string KAEK)
        {
            return _context.Plots.FirstOrDefault(p => p.kaek == KAEK);
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
