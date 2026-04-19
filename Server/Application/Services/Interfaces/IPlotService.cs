using API.Application.DTOs.PlotInsertion;
using API.Application.DTOs.UserInsertion;
using API.Domain.Entities.PlotManagement;
using API.Domain.Entities.UserManagement;

namespace API.Application.Services.Interfaces
{
    public interface IPlotService
    {
        List<Plot> AddPlots(EsriJsonRoot features);
    }
}
