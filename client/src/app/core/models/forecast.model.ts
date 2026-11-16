// One region and day from Forecast/weatherIndexes.
export interface WeatherForecast {
  kalcode: string;
  wmoCode: number;
  temperatureMax: number;
  temperatureMin: number;
  windSpeed: number;
  precipitationPct: number;
  humidity: number;
  time: string; // yyyy-MM-dd
}

// One region and day from Forecast/airQualityIndexes.
export interface AirQualityForecast {
  kalcode: string;
  dust: number;
  alderPollen: number;
  birchPollen: number;
  grassPollen: number;
  mugwortPollen: number;
  olivePollen: number;
  ragweedPollen: number;
  pM10: number;
  pM2_5: number;
  aqi: number;
  o3: number;
  nO2: number;
  time: string; // yyyy-MM-dd
}
