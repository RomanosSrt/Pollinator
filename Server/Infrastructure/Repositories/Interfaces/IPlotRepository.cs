using API.Domain.Entities.PlotManagement;

namespace API.Infrastructure.Repositories.Interfaces
{
    public interface IPlotRepository
    {
        List<Plot> Add(List<Plot> plots);
    }
}
