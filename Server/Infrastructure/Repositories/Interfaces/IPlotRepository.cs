using API.Domain.Entities.PlotManagement;

namespace API.Infrastructure.Repositories.Interfaces
{
    public interface IPlotRepository
    {
        public Plot? GetByKAEK(string KAEK);
        public List<Plot> Add(List<Plot> plots);
    }
}
