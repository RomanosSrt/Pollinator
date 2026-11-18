import type { ISourceOptions } from '@tsparticles/engine';

import type { AnimKind } from './metric-scales';

// What one region gets: tsParticles options, plus two optional extras for the region's div.
export interface Preset {
  options: ISourceOptions;
  blur?: number; // CSS blur in px, makes soft clouds out of hard circles
  flash?: boolean; // lightning flashes (class fx-flash in styles.css)
}

// Particles of each kind at full level in a medium-sized region. Edit to taste.
const MAX = { bees: 10, smog: 14, dust: 60, wind: 28, rain: 70, snow: 45, mist: 12 };

type Particles = Record<string, unknown>;

// Wraps the particle settings in the options every region shares.
function wrap(count: number, particles: Particles): ISourceOptions {
  return {
    fullScreen: { enable: false }, // the particles live inside the region's div, not the whole page
    detectRetina: true,
    fpsLimit: 40,
    pauseOnOutsideViewport: true, // regions scrolled off screen stop animating
    particles: {
      // density off: "count" is the real number of particles, not "per full-HD screen"
      number: { value: count, density: { enable: false } },
      ...particles,
    },
  };
}

// level: 0 to 1 (how bad the band is). size: bigger regions get more particles.
function count(kind: keyof typeof MAX, level: number, size: number): number {
  return Math.max(1, Math.round(MAX[kind] * level * size));
}

const bees = (level: number, size: number): Preset => ({
  options: wrap(count('bees', level, size), {
    paint: { color: { value: '#fbc02d' } },
    shape: { type: 'emoji', options: { emoji: { value: '🐝' } } },
    size: { value: { min: 9, max: 14 } },
    opacity: { value: 1 },
    move: { enable: true, speed: { min: 0.6, max: 1.4 }, direction: 'none', random: true, straight: false, outModes: { default: 'bounce' } },
  }),
});

// Smog (PM2.5 and PM10): slow grey blobs, blurred into clouds.
const smog = (level: number, size: number): Preset => ({
  blur: 6,
  options: wrap(count('smog', level, size), {
    paint: { color: { value: '#6f6f6f' } },
    shape: { type: 'circle' },
    size: { value: { min: 20, max: 42 } },
    opacity: { value: { min: 0.1, max: 0.3 } },
    move: { enable: true, speed: { min: 0.1, max: 0.35 }, direction: 'right', straight: true, outModes: { default: 'out' } },
  }),
});

// Mist (humidity, fog): like smog but white.
const mist = (level: number, size: number): Preset => ({
  blur: 6,
  options: wrap(count('mist', level, size), {
    paint: { color: { value: '#eceff1' } },
    shape: { type: 'circle' },
    size: { value: { min: 24, max: 50 } },
    opacity: { value: { min: 0.15, max: 0.4 } },
    move: { enable: true, speed: { min: 0.1, max: 0.3 }, direction: 'right', straight: true, outModes: { default: 'out' } },
  }),
});

// Dust: small specks blown sideways, faster when the level is higher.
const dust = (level: number, size: number): Preset => ({
  options: wrap(count('dust', level, size), {
    paint: { color: { value: '#c9a063' } },
    shape: { type: 'circle' },
    size: { value: { min: 1, max: 2.6 } },
    opacity: { value: { min: 0.4, max: 0.85 } },
    move: { enable: true, speed: { min: 2 + 3 * level, max: 4 + 5 * level }, direction: 'right', straight: true, outModes: { default: 'out' } },
  }),
});

// Wind: horizontal streaks, faster when the level is higher.
const wind = (level: number, size: number): Preset => ({
  options: wrap(count('wind', level, size), {
    paint: { color: { value: '#ffffff' }, stroke: { width: 1.5, color: { value: '#ffffff' } }, fill: { enable: false } },
    shape: { type: 'line' },
    size: { value: { min: 6, max: 14 } },
    opacity: { value: 0.7 },
    move: { enable: true, speed: { min: 3 + 4 * level, max: 6 + 8 * level }, direction: 'right', straight: true, outModes: { default: 'out' } },
  }),
});

// Rain: vertical streaks falling down.
const rain = (level: number, size: number, color = '#9fd3ff'): Preset => ({
  options: wrap(count('rain', level, size), {
    paint: { color: { value: color }, stroke: { width: 1.5, color: { value: color } }, fill: { enable: false } },
    shape: { type: 'line' },
    size: { value: { min: 5, max: 9 } },
    rotate: { value: 90 }, // the line shape is horizontal, turn it upright
    opacity: { value: 0.8 },
    move: { enable: true, speed: { min: 7 + 4 * level, max: 11 + 6 * level }, direction: 'bottom', straight: true, outModes: { default: 'out' } },
  }),
});

const snow = (level: number, size: number): Preset => ({
  options: wrap(count('snow', level, size), {
    paint: { color: { value: '#ffffff' } },
    shape: { type: 'circle' },
    size: { value: { min: 1.5, max: 4 } },
    opacity: { value: { min: 0.6, max: 0.95 } },
    move: { enable: true, speed: { min: 0.4, max: 1.2 }, direction: 'bottom', drift: { min: -0.6, max: 0.6 }, outModes: { default: 'out' } },
  }),
});

// Weather: the effect depends on the WMO group (the band label), not on a level.
// Groups missing here (Clear, Partly cloudy, Overcast) get no particles.
const WEATHER: Record<string, (size: number) => Preset> = {
  Fog: (s) => mist(0.8, s),
  Drizzle: (s) => rain(0.25, s, '#cfe9ff'),
  Rain: (s) => rain(0.6, s),
  Showers: (s) => rain(0.5, s),
  Snow: (s) => snow(0.6, s),
  'Snow showers': (s) => snow(0.8, s),
  Thunderstorm: (s) => ({ ...rain(1, s, '#b9a7e0'), flash: true }),
};

export const WEATHER_EFFECT_LABELS = Object.keys(WEATHER);

// level: 0 to 1, how bad the band is (first band 0, last band 1). label: the band's name.
// size: 0.4 to 2, how big the region is on screen. Returns null when nothing should move.
export function presetFor(anim: AnimKind, level: number, label: string, size: number): Preset | null {
  if (anim === 'weather') return WEATHER[label]?.(size) ?? null;
  if (level <= 0) return null; // the calmest band shows no particles
  switch (anim) {
    case 'bees':
      return bees(level, size);
    case 'smog':
      return smog(level, size);
    case 'dust':
      return dust(level, size);
    case 'wind':
      return wind(level, size);
    case 'rain':
      return rain(level, size);
    case 'mist':
      return mist(level, size);
    default:
      return null;
  }
}
