using API.Application.DTOs.PlotInsertion;
using API.Domain.Entities.PlotManagement;
using AutoMapper;
using NetTopologySuite.Geometries;

namespace API.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var factory = new GeometryFactory(new PrecisionModel(), 4326);
            
            CreateMap<EsriFeature, Plot>()
                .ForMember(dest => dest.PlotId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.KAEK, opt => opt.MapFrom(src => src.attributes.KAEK))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.attributes.Area))
                .ForMember(dest => dest.Polygon, opt => opt.MapFrom(src => factory.CreatePolygon(factory.CreateLinearRing(src.geometry.rings[0]
                              .Select(p => new Coordinate(p[0], p[1])).ToArray())
                )))
                .ForMember(dest => dest.isClaimed, opt => opt.Ignore())
                .ForMember(dest => dest.FarmerId, opt => opt.MapFrom(src => (Guid?)null));
        }
    }
}
