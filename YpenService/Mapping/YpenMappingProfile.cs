using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using YpenService.Models.Pollinator.Persistence;
using YpenService.Models.Ypen;

namespace YpenService.Mapping
{
    public class YpenMappingProfile : Profile
    {
        public YpenMappingProfile() 
        { 
            var centers = new List<RegionCentersDto>();

            CreateMap<RegionCentersResponse, List<RegionCentersDto>>()
                .ConvertUsing(src => src.features.
                Select(f => new RegionCentersDto 
                {
                    KALCODE = f.properties.KALCODE.Remove(2),
                    Name = f.properties.EDRA,
                    Latitude = f.properties.LAT,
                    Longitude = f.properties.LON
                })
                .ToList());

            CreateMap<RegionUnitsResponse, List<RegionUnitsDto>>()
                .ConvertUsing(src => src.features.
                Select(f => new RegionUnitsDto
                {
                    KALCODE = f.properties.KALCODE,
                    area = f.properties.AREA_km2,
                    shape = CoordinatesToGeometry(f.geometry)
                })
                .ToList());

            CreateMap<RegionUnitsDto, RegionUnits>()
                .ForMember(dest => dest.unit_KALCODE, opt => opt.MapFrom(src => src.KALCODE))
                .ForMember(dest => dest.unit_Name, opt => opt.MapFrom(src => src.Region))
                .ForMember(dest => dest.unit_Center, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.unit_Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.unit_Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.unit_Shapes, opt => opt.MapFrom(src => src.shape))
                .ForMember(dest => dest.unit_Area, opt => opt.MapFrom(src => src.area));

        }
        private Geometry? CoordinatesToGeometry(UnitsGeometry geometry)
        {
            var factory = new GeometryFactory();
            const int decimalPlaces = 2;        //IMPORTANT! rounding coordinates' decimal points

            if (geometry.type == "MultiPolygon" && geometry.coordinates.Count > 0)
            {
                var polygons = geometry.coordinates.Select(polygon =>
                {
                    var rings = polygon.Select(ring =>
                        new LinearRing(ring.Select(coord =>
                            new Coordinate(
                                Math.Round(coord[0], decimalPlaces), 
                                Math.Round(coord[1], decimalPlaces)
                            )
                        ).ToArray())
                    ).ToArray();

                    return factory.CreatePolygon(rings[0], rings.Skip(1).ToArray());
                }).ToArray();

                return factory.CreateMultiPolygon(polygons);
            }

            return null;
        }
    }
}
