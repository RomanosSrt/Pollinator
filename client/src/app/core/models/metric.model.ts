export type MetricGroup = 'Weather' | 'Air quality' | 'Pollen';

// A forecast value the map can display.
export interface Metric {
  key: string; // field name in the forecast record
  label: string;
  unit: string;
  group: MetricGroup;
  source: 'weather' | 'air';
}

export const METRICS: Metric[] = [
  { key: 'temperatureMax', label: 'Max temperature', unit: '°C', group: 'Weather', source: 'weather' },
  { key: 'temperatureMin', label: 'Min temperature', unit: '°C', group: 'Weather', source: 'weather' },
  { key: 'windSpeed', label: 'Wind speed', unit: 'km/h', group: 'Weather', source: 'weather' },
  { key: 'precipitationPct', label: 'Precipitation chance', unit: '%', group: 'Weather', source: 'weather' },
  { key: 'humidity', label: 'Humidity', unit: '%', group: 'Weather', source: 'weather' },
  { key: 'aqi', label: 'Air quality index', unit: 'AQI', group: 'Air quality', source: 'air' },
  { key: 'pM10', label: 'PM10', unit: 'µg/m³', group: 'Air quality', source: 'air' },
  { key: 'pM2_5', label: 'PM2.5', unit: 'µg/m³', group: 'Air quality', source: 'air' },
  { key: 'o3', label: 'Ozone (O₃)', unit: 'µg/m³', group: 'Air quality', source: 'air' },
  { key: 'nO2', label: 'Nitrogen dioxide (NO₂)', unit: 'µg/m³', group: 'Air quality', source: 'air' },
  { key: 'dust', label: 'Dust', unit: 'µg/m³', group: 'Air quality', source: 'air' },
  { key: 'grassPollen', label: 'Grass pollen', unit: 'grains/m³', group: 'Pollen', source: 'air' },
  { key: 'olivePollen', label: 'Olive pollen', unit: 'grains/m³', group: 'Pollen', source: 'air' },
  { key: 'birchPollen', label: 'Birch pollen', unit: 'grains/m³', group: 'Pollen', source: 'air' },
  { key: 'alderPollen', label: 'Alder pollen', unit: 'grains/m³', group: 'Pollen', source: 'air' },
  { key: 'mugwortPollen', label: 'Mugwort pollen', unit: 'grains/m³', group: 'Pollen', source: 'air' },
  { key: 'ragweedPollen', label: 'Ragweed pollen', unit: 'grains/m³', group: 'Pollen', source: 'air' },
];
