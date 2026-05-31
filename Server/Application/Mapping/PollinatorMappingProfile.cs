using API.Application.DTOs.PlotHandling;
using API.Application.DTOs.PlotHnadling;
using API.Application.DTOs.UserInsertion;
using API.Domain.Entities.PlotManagement;
using API.Domain.Entities.UserManagement;
using AutoMapper;
using NetTopologySuite.Geometries;

namespace API.Application.Mapping
{
    public class PollinatorMappingProfile : Profile
    {
        public PollinatorMappingProfile()
        {
            var factory = new GeometryFactory(new PrecisionModel(), 4326);
            
            CreateMap<EsriFeature, Plot>()
                .ForMember(dest => dest.plotId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.kaek, opt => opt.MapFrom(src => src.attributes.KAEK))
                .ForMember(dest => dest.area, opt => opt.MapFrom(src => src.attributes.Area))
                .ForMember(dest => dest.polygon, opt => opt.MapFrom(src => factory.CreatePolygon(factory.CreateLinearRing(src.geometry.rings[0]
                              .Select(p => new Coordinate(p[0], p[1])).ToArray())
                )))
                .ForMember(dest => dest.isClaimed, opt => opt.Ignore())
                .ForMember(dest => dest.farmerId, opt => opt.MapFrom(src => (Guid?)null));

            CreateMap<Plot, PlotDto>()
                .ForMember(dest => dest.plotId, opt => opt.MapFrom(src => src.plotId))
                .ForMember(dest => dest.polygon, opt => opt.MapFrom(src => src.polygon))
                .ForMember(dest => dest.area, opt => opt.MapFrom(src => src.area))
                .ForMember(dest => dest.cropTypes, opt => opt.MapFrom(src => src.cropTypes))
                .ForMember(dest => dest.isClaimed, opt => opt.MapFrom(src => src.isClaimed))
                .ForMember(dest => dest.farmerId, opt => opt.MapFrom(src => src.farmerId));

            CreateMap<UserDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.email.Remove(src.email.IndexOf('@'))))
                .ForMember(dest => dest.fullName, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                .ForMember(dest => dest.userType, opt => opt.MapFrom(src => src.userType));

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.fullName))
                .ForMember(dest => dest.password, opt => opt.Ignore())
                .ForMember(dest => dest.userType, opt => opt.MapFrom(src => src.userType));
        }
    }
}
