using API.Domain.Entities.PlotManagement;
using NetTopologySuite.Geometries;
using System.Collections.ObjectModel;

namespace API.Domain.Entities.UserManagement
{
    public interface Farmer
    {
        public Collection<Plot> Fields { get; set; }

    }
}
