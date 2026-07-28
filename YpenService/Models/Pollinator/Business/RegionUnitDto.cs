using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using YpenService.Models.Ypen;

namespace YpenService.Models.Pollinator.Business
{
    public class RegionUnitDto : RegionCenterDto
    {
        public Geometry shape { get; set; }
        public float area { get; set; } = 0;
        //public string regionCheck { get; set; }

    }
}
