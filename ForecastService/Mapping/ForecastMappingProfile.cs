using AutoMapper;
using ForecastService.Models.OpenMeteo.Responses;
using ForecastService.Models.Pollinator.DAOs;
using ForecastService.Models.Pollinator.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Mapping
{
    public class ForecastMappingProfile : Profile
    {
        public ForecastMappingProfile()
        {
            CreateMap<AirQualityResponse, AirQualityDTO>()
                .ForMember(dest => dest.Dust, opt => opt.MapFrom(src => src.hourly.dust))
                .ForMember(dest => dest.AlderPollen, opt => opt.MapFrom(src => src.hourly.alder_pollen))
                .ForMember(dest => dest.BirchPollen, opt => opt.MapFrom(src => src.hourly.birch_pollen))
                .ForMember(dest => dest.GrassPollen, opt => opt.MapFrom(src => src.hourly.grass_pollen))
                .ForMember(dest => dest.MugwortPollen, opt => opt.MapFrom(src => src.hourly.mugwort_pollen))
                .ForMember(dest => dest.OlivePollen, opt => opt.MapFrom(src => src.hourly.olive_pollen))
                .ForMember(dest => dest.RagweedPollen, opt => opt.MapFrom(src => src.hourly.ragweed_pollen))
                .ForMember(dest => dest.PM10, opt => opt.MapFrom(src => src.hourly.pm10))
                .ForMember(dest => dest.PM2_5, opt => opt.MapFrom(src => src.hourly.pm2_5))
                .ForMember(dest => dest.AQI, opt => opt.MapFrom(src => src.hourly.european_aqi))
                .ForMember(dest => dest.O3, opt => opt.MapFrom(src => src.hourly.ozone))
                .ForMember(dest => dest.NO2, opt => opt.MapFrom(src => src.hourly.nitrogen_dioxide))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.hourly.time));
            
            CreateMap<WeatherForecastResponse, WeatherDTO>()
                .ForMember(dest => dest.WmoCode, opt => opt.MapFrom(src => src.daily.weather_code))
                .ForMember(dest => dest.TemperatureMax, opt => opt.MapFrom(src => src.daily.temperature_2m_max))
                .ForMember(dest => dest.TemperatureMin, opt => opt.MapFrom(src => src.daily.temperature_2m_min))
                .ForMember(dest => dest.WindSpeed, opt => opt.MapFrom(src => src.daily.wind_speed_10m_max))
                .ForMember(dest => dest.PrecipitationPct, opt => opt.MapFrom(src => src.daily.precipitation_probability_max))
                .ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => src.daily.relative_humidity_2m_max))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.daily.time));

            CreateMap<WeatherDAO, WeatherDTO>()
                .ForMember(dest => dest.WmoCode, opt => opt.MapFrom(src => src.WmoCode))
                .ForMember(dest => dest.TemperatureMax, opt => opt.MapFrom(src => src.TemperatureMax))
                .ForMember(dest => dest.TemperatureMin, opt => opt.MapFrom(src => src.TemperatureMin))
                .ForMember(dest => dest.WindSpeed, opt => opt.MapFrom(src => src.WindSpeed))
                .ForMember(dest => dest.PrecipitationPct, opt => opt.MapFrom(src => src.PrecipitationPct))
                .ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => src.Humidity))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time))
                .ReverseMap();
        }
    }
}
