using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using YpenService.Models.Ypen;

namespace YpenService.Models.Pollinator.Persistence
{
    public class RegionUnitsDto : RegionCentersDto
    {
        public Geometry shape { get; set; } 
        public double area { get; set; }

    }
}
