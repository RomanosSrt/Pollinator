using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Models.OpenMeteo.Responses
{
    public class PollenIndexesResponse
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public double elevation { get; set; }
        public Current current { get; set; }
        public HourlyUnits hourly_units { get; set; }
        public Hourly hourly { get; set; }
        public int? location_id { get; set; }


    }

    public class Current
    {
        public string time { get; set; }
        public int interval { get; set; }
        public double alder_pollen { get; set; }
        public double birch_pollen { get; set; }
        public double grass_pollen { get; set; }
        public double mugwort_pollen { get; set; }
        public double olive_pollen { get; set; }
        public double ragweed_pollen { get; set; }
    }
    public class Hourly
    {
        public List<string> time { get; set; }
        public List<double> alder_pollen { get; set; }
        public List<double> birch_pollen { get; set; }
        public List<double> grass_pollen { get; set; }
        public List<double> mugwort_pollen { get; set; }
        public List<double> olive_pollen { get; set; }
        public List<double> ragweed_pollen { get; set; }
    }

    public class HourlyUnits
    {
        public string time { get; set; }
        public string alder_pollen { get; set; }
        public string birch_pollen { get; set; }
        public string grass_pollen { get; set; }
        public string mugwort_pollen { get; set; }
        public string olive_pollen { get; set; }
        public string ragweed_pollen { get; set; }
    }
}
