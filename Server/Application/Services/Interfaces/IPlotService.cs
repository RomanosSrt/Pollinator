using API.Application.DTOs.PlotHandling;
using API.Application.DTOs.PlotHnadling;
using API.Application.DTOs.UserInsertion;
using API.Domain.Entities.PlotManagement;
using API.Domain.Entities.UserManagement;

namespace API.Application.Services.Interfaces
{
    public interface IPlotService
    {
        Task<PlotDto?> GetPlot(string KAEK);
        List<Plot> AddPlots(EsriJsonRoot features);
    }
}
