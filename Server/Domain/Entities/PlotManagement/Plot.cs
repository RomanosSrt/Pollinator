using NetTopologySuite.Geometries;
using System.Collections.ObjectModel;

namespace API.Domain.Entities.PlotManagement
{
    public class Plot
    {
        public Guid PlotId { get; set; }

        //geospatial
        public Geometry Polygon { get; set; }
        public double Area { get; set; }
        
        //agricultural info
        public ICollection<CropType>? CropTypes { get; set; }

        //ownership
        public bool isClaimed { get; set; } = false;
        public Guid? FarmerId { get; set; }

        //availability
        public ICollection<PlotAvailability>? Availabilities { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }

/*        Plot()
        {
            if (!isClaimed)
            {
                CropTypes = null;
                Availabilities = null;
                Reservations = null;
            }

        }*/
    }
}
