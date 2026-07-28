using API.Domain.Entities.PlotManagement;
using NetTopologySuite.Geometries;

namespace API.Application.DTOs.PlotHnadling
{
    public class PlotDto
    {
        public Guid plotId { get; set; }
        public required Geometry polygon { get; set; }
        public float area { get; set; }

        //agricultural info
        public ICollection<CropType>? cropTypes { get; set; }

        //ownership
        public bool isClaimed { get; set; } = false;
        public int? farmerId { get; set; }
    }
}
