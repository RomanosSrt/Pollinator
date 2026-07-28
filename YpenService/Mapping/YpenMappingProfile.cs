using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using YpenService.Models.Pollinator.Business;
using YpenService.Models.Pollinator.Persistence;
using YpenService.Models.Ypen;


namespace YpenService.Mapping
{
    public class YpenMappingProfile : Profile
    {
        public YpenMappingProfile() 
        { 
            CreateMap<RegionCentersResponse, List<RegionCenterDto>>()
                .ConvertUsing(src => src.features.
                Select(f => new RegionCenterDto 
                {
                    KALCODE = f.properties.KALCODE.Remove(2),
                    Name = f.properties.EDRA,
                    Region = "Π.Ε. " + f.properties.PE_ENOTHTA,
                    Latitude = (float)Math.Round(f.properties.LAT, 3),
                    Longitude = (float)Math.Round(f.properties.LON, 3)
                })
                .ToList());

            CreateMap<RegionUnitsResponse, List<RegionUnitDto>>()
                .ConvertUsing(src => src.features.
                Select(f => new RegionUnitDto
                {
                    KALCODE = f.properties.KALCODE,
                    //regionCheck = f.properties.LEKTIKO,   //test
                    area = f.properties.AREA_km2,
                    shape = CoordinatesToGeometry(f.geometry)
                })
                .ToList());

            CreateMap<RegionUnitDto, RegionUnit>()
                .ForMember(dest => dest.unit_KALCODE, opt => opt.MapFrom(src => src.KALCODE))
                //.ForMember(dest => dest.unit_NameCheck, opt => opt.MapFrom(src => src.regionCheck))   //region-center validity check
                .ForMember(dest => dest.unit_Name, opt => opt.MapFrom(src => src.Region))
                .ForMember(dest => dest.unit_Center, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.unit_Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.unit_Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.unit_Shapes, opt => opt.MapFrom(src => src.shape))
                .ForMember(dest => dest.unit_Area, opt => opt.MapFrom(src => src.area))
                .ReverseMap();

            CreateMap<RegionUnit, RegionCenter>()
                .ForMember(dest => dest.KALCODE, opt => opt.MapFrom(src => src.unit_KALCODE))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.unit_Center))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.unit_Name))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.unit_Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.unit_Longitude));

            CreateMap<RegionCenter, RegionCenterDto>()
                .ForMember(dest => dest.KALCODE, opt => opt.MapFrom(src => src.KALCODE))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));

        }
        private Geometry? CoordinatesToGeometry(UnitsGeometry geometry)
        {
            var factory = new GeometryFactory();
            const int decimalPlaces = 3;        //IMPORTANT! rounding coordinates' decimal points

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
