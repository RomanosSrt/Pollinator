import { weatherFile } from './weather-badge';

describe('weatherFile', () => {
  it('maps WMO codes to Meteocons files', () => {
    expect(weatherFile(0)).toBe('clear-day');
    expect(weatherFile(2)).toBe('partly-cloudy-day');
    expect(weatherFile(3)).toBe('overcast-day');
    expect(weatherFile(48)).toBe('fog-day');
    expect(weatherFile(53)).toBe('drizzle');
    expect(weatherFile(65)).toBe('rain');
    expect(weatherFile(81)).toBe('rain');
    expect(weatherFile(73)).toBe('snow');
    expect(weatherFile(86)).toBe('snow');
    expect(weatherFile(95)).toBe('thunderstorms-day-rain');
  });

  it('has no icon for unknown codes', () => {
    expect(weatherFile(6090)).toBeNull();
    expect(weatherFile(-1)).toBeNull();
  });
});
