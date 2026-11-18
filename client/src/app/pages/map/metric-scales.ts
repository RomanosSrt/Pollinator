// How each metric looks on the map. This file is DATA ONLY: to change a colour,
// a threshold or an animation, edit it here and nothing else.

// 'solid' = flat colour. 'pulse' = radial gradient that breathes.
export type FillKind = 'solid' | 'pulse';

// What moves inside the region ('none' = colour only).
export type AnimKind = 'none' | 'smog' | 'dust' | 'bees' | 'weather' | 'wind' | 'rain' | 'mist';

// One step of a scale. A value belongs to the first band whose `max` is greater than it,
// so "<20" is { max: 20 }. The last band uses Infinity.
export interface Band {
  label: string;
  max: number;
  color: string;
}

export interface MetricScale {
  fill: FillKind;
  anim: AnimKind;
  bands: Band[];
  alarmFrom?: number; // red pulsing border from this band index upwards (0 = first band)
}

// Official European Air Quality Index colours (EEA).
const EAQI: Band[] = [
  { label: 'Good', max: 20, color: '#50f0e6' },
  { label: 'Fair', max: 40, color: '#50ccaa' },
  { label: 'Moderate', max: 60, color: '#f0e641' },
  { label: 'Poor', max: 80, color: '#ff5050' },
  { label: 'Very poor', max: 100, color: '#960032' },
  { label: 'Extremely poor', max: Infinity, color: '#7d2181' },
];

const TEMPERATURE: Band[] = [
  { label: 'Freezing', max: 0, color: '#313695' },
  { label: 'Cold', max: 10, color: '#74add1' },
  { label: 'Mild', max: 20, color: '#fee090' },
  { label: 'Warm', max: 28, color: '#fdae61' },
  { label: 'Hot', max: 35, color: '#f46d43' },
  { label: 'Extreme heat', max: Infinity, color: '#a50026' },
];

// Pollen: same four colours for every species, only the limits differ.
const POLLEN_COLORS = ['#a6d96a', '#fee08b', '#fdae61', '#d73027'];
const POLLEN_LABELS = ['Low', 'Moderate', 'High', 'Very high'];

// `limits` are the three upper limits, in grains/m³.
function pollen(limits: [number, number, number]): Band[] {
  return POLLEN_LABELS.map((label, i) => ({ label, max: i < 3 ? limits[i] : Infinity, color: POLLEN_COLORS[i] }));
}

const POLLEN_TREE = pollen([10, 100, 500]); // alder, birch, olive
const POLLEN_GRASS_RAGWEED = pollen([10, 30, 100]);
const POLLEN_MUGWORT = pollen([10, 50, 100]);

export const METRIC_SCALES: Record<string, MetricScale> = {
  // Air quality (European AQI, 0 to 100+)
  aqi: { fill: 'solid', anim: 'none', bands: EAQI },
  pM2_5: { fill: 'solid', anim: 'smog', bands: EAQI, alarmFrom: 3 },
  pM10: { fill: 'solid', anim: 'smog', bands: EAQI, alarmFrom: 3 }, // same as PM2.5
  o3: { fill: 'solid', anim: 'none', bands: EAQI, alarmFrom: 3 },
  nO2: { fill: 'solid', anim: 'none', bands: EAQI, alarmFrom: 3 },
  dust: {
    fill: 'solid',
    anim: 'dust',
    alarmFrom: 3,
    bands: [ // µg/m³, demo bands (no official scale)
      { label: 'Very low', max: 20, color: '#fff3c4' },
      { label: 'Low', max: 50, color: '#f6d58e' },
      { label: 'Moderate', max: 100, color: '#e6a950' },
      { label: 'High', max: 200, color: '#c06a1f' },
      { label: 'Very high', max: Infinity, color: '#7a3b0c' },
    ],
  },

  // Pollen (grains/m³)
  alderPollen: { fill: 'solid', anim: 'bees', bands: POLLEN_TREE },
  birchPollen: { fill: 'solid', anim: 'bees', bands: POLLEN_TREE },
  olivePollen: { fill: 'solid', anim: 'bees', bands: POLLEN_TREE },
  grassPollen: { fill: 'solid', anim: 'bees', bands: POLLEN_GRASS_RAGWEED },
  ragweedPollen: { fill: 'solid', anim: 'bees', bands: POLLEN_GRASS_RAGWEED },
  mugwortPollen: { fill: 'solid', anim: 'bees', bands: POLLEN_MUGWORT },

  // Weather. The WMO code groups are the bands; the animation is picked from the band.
  wmoCode: {
    fill: 'solid',
    anim: 'weather',
    bands: [
      { label: 'Clear', max: 1, color: '#ffd54f' },
      { label: 'Partly cloudy', max: 3, color: '#ffe9a8' },
      { label: 'Overcast', max: 4, color: '#b0bec5' },
      { label: 'Fog', max: 51, color: '#cfd8dc' },
      { label: 'Drizzle', max: 61, color: '#81d4fa' },
      { label: 'Rain', max: 71, color: '#4fa3e0' },
      { label: 'Snow', max: 80, color: '#e3f2fd' },
      { label: 'Showers', max: 83, color: '#5c9bd5' },
      { label: 'Snow showers', max: 95, color: '#b3d4f5' },
      { label: 'Thunderstorm', max: Infinity, color: '#6a4c93' },
    ],
  },
  temperatureMax: { fill: 'pulse', anim: 'none', bands: TEMPERATURE }, // °C
  temperatureMin: { fill: 'pulse', anim: 'none', bands: TEMPERATURE },
  windSpeed: {
    fill: 'solid',
    anim: 'wind',
    bands: [ // km/h, roughly Beaufort groups
      { label: 'Calm', max: 6, color: '#e0f2f1' },
      { label: 'Light', max: 20, color: '#a5d6d1' },
      { label: 'Moderate', max: 39, color: '#5fb3b3' },
      { label: 'Strong', max: 62, color: '#2e7d8c' },
      { label: 'Gale', max: Infinity, color: '#1a3d6b' },
    ],
  },
  precipitationPct: {
    fill: 'solid',
    anim: 'rain',
    bands: [ // %
      { label: 'Unlikely', max: 20, color: '#e3f2fd' },
      { label: 'Possible', max: 50, color: '#90caf9' },
      { label: 'Likely', max: 80, color: '#42a5f5' },
      { label: 'Very likely', max: Infinity, color: '#1565c0' },
    ],
  },
  humidity: {
    fill: 'solid',
    anim: 'mist',
    bands: [ // %
      { label: 'Dry', max: 30, color: '#fff3e0' },
      { label: 'Comfortable', max: 60, color: '#c8e6c9' },
      { label: 'Humid', max: 80, color: '#80cbc4' },
      { label: 'Very humid', max: Infinity, color: '#00796b' },
    ],
  },
};

// Which band a value falls in, or undefined when the value is missing/invalid.
export function bandOf(key: string, value: number | undefined): { band: Band; index: number } | undefined {
  const scale = METRIC_SCALES[key];
  if (!scale || value === undefined || !Number.isFinite(value)) return undefined;
  const index = scale.bands.findIndex((b) => value < b.max);
  return { band: scale.bands[index], index };
}
