import { Component, DestroyRef, ElementRef, afterNextRender, computed, effect, inject, signal, viewChild } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonToggle, MatButtonToggleGroup } from '@angular/material/button-toggle';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatOptgroup, MatOption, MatSelect } from '@angular/material/select';
import * as L from 'leaflet';
import { forkJoin } from 'rxjs';

import { AirQualityForecast, WeatherForecast } from '../../core/models/forecast.model';
import { METRICS } from '../../core/models/metric.model';
import { ForecastService } from '../../core/services/forecast.service';
import { RegionService } from '../../core/services/region.service';

// Forecast record read by metric key.
interface Row {
  kalcode: string;
  time: string;
  [key: string]: string | number;
}

const GREECE: L.LatLngExpression = [38.5, 23.8];
const RAMP = ['#ffffb2', '#fd8d3c', '#800026']; // low -> high
const NO_DATA: L.PathOptions = { color: '#888', weight: 1, fillColor: '#ccc', fillOpacity: 0.3 };

// Map page: regional units coloured by the chosen metric and day.
@Component({
  selector: 'app-map',
  imports: [MatProgressBar, MatFormField, MatLabel, MatSelect, MatOption, MatOptgroup, MatButtonToggleGroup, MatButtonToggle],
  templateUrl: './map.component.html',
})
export class MapComponent {
  private readonly regions = inject(RegionService);
  private readonly forecasts = inject(ForecastService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly mapEl = viewChild.required<ElementRef<HTMLElement>>('map');

  protected readonly groups = (['Weather', 'Air quality', 'Pollen'] as const).map((name) => ({
    name,
    metrics: METRICS.filter((m) => m.group === name),
  }));
  protected readonly gradient = `linear-gradient(to right, ${RAMP.join(', ')})`;

  protected readonly loading = signal(true); // map layers
  protected readonly forecastLoading = signal(true);
  protected readonly metricKey = signal('temperatureMax');
  protected readonly day = signal('');

  private readonly weather = signal<WeatherForecast[]>([]);
  private readonly air = signal<AirQualityForecast[]>([]);
  private readonly unitLayer = signal<L.GeoJSON | null>(null);

  protected readonly metric = computed(() => METRICS.find((m) => m.key === this.metricKey())!);

  // Records of the chosen metric's source.
  private readonly rows = computed(
    () => (this.metric().source === 'weather' ? this.weather() : this.air()) as unknown as Row[],
  );

  // Days available for the chosen metric.
  protected readonly days = computed(() => [...new Set(this.rows().map((r) => r.time))].sort());

  // Value per region for the chosen day.
  private readonly values = computed(() => {
    const key = this.metricKey();
    const day = this.day();
    return new Map(this.rows().filter((r) => r.time === day).map((r) => [r.kalcode, Number(r[key])]));
  });

  // Min/max over all days, so colours stay comparable when the day changes.
  protected readonly range = computed(() => {
    const key = this.metricKey();
    const all = this.rows().map((r) => Number(r[key]));
    return all.length ? { min: Math.min(...all), max: Math.max(...all) } : { min: 0, max: 0 };
  });

  // First forecast day, when it is already in the past.
  protected readonly staleSince = computed(() => {
    const first = this.days()[0];
    return first && first < new Date().toISOString().slice(0, 10) ? first : null;
  });

  constructor() {
    afterNextRender(() => this.initMap());

    // Recolour the units when metric, day or data change.
    effect(() => this.unitLayer()?.setStyle((f) => this.styleFor(f?.properties?.['kalcode'])));

    forkJoin({ weather: this.forecasts.weather$, air: this.forecasts.airQuality$ })
      .pipe(takeUntilDestroyed())
      .subscribe({
        next: ({ weather, air }) => {
          this.weather.set(weather);
          this.air.set(air);
          this.day.set(this.days()[0] ?? '');
          this.forecastLoading.set(false);
        },
        error: () => this.forecastLoading.set(false), // ErrorService already informed the user
      });
  }

  protected selectMetric(key: string): void {
    this.metricKey.set(key);
    const days = this.days();
    if (!days.includes(this.day())) this.day.set(days[0] ?? ''); // keep the day if the new metric has it
  }

  protected dayLabel(day: string): string {
    return new Date(`${day}T00:00:00`).toLocaleDateString('en-GB', { weekday: 'short', day: 'numeric', month: 'short' });
  }

  protected format(value: number): string {
    return Math.abs(value) < 10 ? value.toFixed(2) : value.toFixed(1);
  }

  private styleFor(kalcode: string): L.PathOptions {
    const value = this.values().get(kalcode);
    if (value === undefined) return NO_DATA;
    const { min, max } = this.range();
    const t = max > min ? (value - min) / (max - min) : 0.5;
    return { color: '#555', weight: 1, fillColor: colorAt(t), fillOpacity: 0.75 };
  }

  private tooltip(kalcode: string, name: string): HTMLElement {
    const value = this.values().get(kalcode);
    const { label, unit } = this.metric();
    const el = document.createElement('div');
    el.textContent = value === undefined ? `${name}: no data` : `${name} · ${label}: ${this.format(value)} ${unit}`;
    return el;
  }

  private initMap(): void {
    const map = L.map(this.mapEl().nativeElement, { preferCanvas: true }).setView(GREECE, 6);
    // Re-measure when the layout changes (controls wrapping, window resize).
    const resize = new ResizeObserver(() => map.invalidateSize());
    resize.observe(this.mapEl().nativeElement);
    this.destroyRef.onDestroy(() => {
      resize.disconnect();
      map.remove();
    });

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 18,
      attribution: '&copy; OpenStreetMap contributors',
    }).addTo(map);

    forkJoin({ units: this.regions.units$, centers: this.regions.centers$ })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: ({ units, centers }) => {
          const unitLayer = L.geoJSON(
            units.map((u) => ({
              type: 'Feature' as const,
              geometry: u.shape,
              properties: { kalcode: u.kalcode, name: u.name },
            })),
            {
              style: (f) => this.styleFor(f?.properties?.['kalcode']),
              onEachFeature: (feature, layer) => {
                layer.bindTooltip(() => this.tooltip(feature.properties['kalcode'], feature.properties['name']), { sticky: true });
                layer.on({
                  mouseover: (e) => (e.target as L.Path).setStyle({ weight: 3, color: '#222' }),
                  mouseout: () => unitLayer.resetStyle(layer as L.Path),
                });
              },
            },
          );

          const centerLayer = L.layerGroup(
            centers.map((c) =>
              L.circleMarker([c.latitude, c.longitude], { radius: 3, color: '#222', fillColor: '#222', fillOpacity: 1 }).bindTooltip(c.name),
            ),
          );

          unitLayer.addTo(map);
          centerLayer.addTo(map);
          L.control.layers(undefined, { 'Regional units': unitLayer, Centers: centerLayer }).addTo(map);
          map.invalidateSize();
          map.fitBounds(unitLayer.getBounds());
          this.unitLayer.set(unitLayer);
          this.loading.set(false);
        },
        error: () => this.loading.set(false), // ErrorService already informed the user
      });
  }
}

// Picks a colour along the ramp for t between 0 and 1.
function colorAt(t: number): string {
  const scaled = Math.min(Math.max(t, 0), 1) * (RAMP.length - 1);
  const i = Math.min(Math.floor(scaled), RAMP.length - 2);
  const [a, b] = [RAMP[i], RAMP[i + 1]].map((hex) => [1, 3, 5].map((p) => parseInt(hex.slice(p, p + 2), 16)));
  return `rgb(${a.map((c, k) => Math.round(c + (b[k] - c) * (scaled - i))).join(',')})`;
}
