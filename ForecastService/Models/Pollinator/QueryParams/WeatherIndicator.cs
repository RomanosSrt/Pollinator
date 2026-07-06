using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Models.Pollinator.QueryParams
{
     public enum WeatherIndicator
    {
        weather_code, 
        temperature_2m_max, 
        temperature_2m_min, 
        wind_speed_10m_max, 
        precipitation_probability_max, 
        relative_humidity_2m_max
    }
}
