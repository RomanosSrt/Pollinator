using API.Domain.Entities.PlotManagement;

namespace API.Infrastructure.Repositories.Interfaces
{
    public interface IPlotRepository
    {
        Task<Plot?> GetByKAEK(string KAEK);
        Task<List<Plot>?> GetByClaim(bool isClaimed);
        Task<List<Plot>?> GetByCropType(List<CropType> cropTypes);
        List<Plot> Add(List<Plot> plots);
    }
}
