using NetTopologySuite.Geometries;
using System.Collections.ObjectModel;

namespace API.Domain.Entities.PlotManagement
{
    public class Plot
    {
        public Guid PlotId { get; set; }
        public Geometry Polygon { get; set; }
        public double Area { get; set; }
        public Collection<CropType> CropTypes { get; set; }
        public bool isClaimed { get; set; }
        public Guid FarmerId { get; set; }
    }
}
