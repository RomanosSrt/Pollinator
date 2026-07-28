using NetTopologySuite.Geometries;
using System.Collections.ObjectModel;

namespace API.Domain.Entities.PlotManagement
{
    public class Plot
    {
        public Guid plotId { get; set; }

        public required string kaek { get; set; }
        public required Geometry polygon { get; set; }
        public float area { get; set; }
        
        //agricultural info
        public ICollection<CropType>? cropTypes { get; set; }

        //ownership
        public bool isClaimed { get; set; } = false;
        public int? farmerId { get; set; }

        //availability
        public ICollection<PlotAvailability>? availabilities { get; set; }
        public ICollection<Reservation>? reservations { get; set; }

    }
}
