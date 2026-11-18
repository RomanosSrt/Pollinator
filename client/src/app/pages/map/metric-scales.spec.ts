import { METRICS } from '../../core/models/metric.model';
import { METRIC_SCALES, bandOf } from './metric-scales';

describe('metric scales', () => {
  it('has a scale for every metric', () => {
    for (const m of METRICS) expect(METRIC_SCALES[m.key], m.key).toBeDefined();
  });

  it('ends every scale with an open band and keeps limits ascending', () => {
    for (const [key, scale] of Object.entries(METRIC_SCALES)) {
      const maxes = scale.bands.map((b) => b.max);
      expect(maxes.at(-1), key).toBe(Infinity);
      for (let i = 1; i < maxes.length; i++) expect(maxes[i], key).toBeGreaterThan(maxes[i - 1]);
    }
  });

  it('picks the band: a value equal to a limit belongs to the next band', () => {
    expect(bandOf('aqi', 19.9)?.band.label).toBe('Good');
    expect(bandOf('aqi', 20)?.band.label).toBe('Fair');
    expect(bandOf('aqi', 250)?.band.label).toBe('Extremely poor');
    expect(bandOf('temperatureMax', -3)?.band.label).toBe('Freezing');
    expect(bandOf('grassPollen', 29)?.index).toBe(1);
    expect(bandOf('grassPollen', 30)?.index).toBe(2);
  });

  it('maps WMO codes to their groups', () => {
    const label = (code: number) => bandOf('wmoCode', code)?.band.label;
    expect(label(0)).toBe('Clear');
    expect(label(2)).toBe('Partly cloudy');
    expect(label(3)).toBe('Overcast');
    expect(label(45)).toBe('Fog');
    expect(label(53)).toBe('Drizzle');
    expect(label(65)).toBe('Rain');
    expect(label(73)).toBe('Snow');
    expect(label(81)).toBe('Showers');
    expect(label(86)).toBe('Snow showers');
    expect(label(95)).toBe('Thunderstorm');
  });

  it('returns undefined for missing or invalid values', () => {
    expect(bandOf('aqi', undefined)).toBeUndefined();
    expect(bandOf('aqi', NaN)).toBeUndefined();
    expect(bandOf('nope', 5)).toBeUndefined();
  });
});
