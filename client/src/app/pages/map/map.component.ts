import { Component, DestroyRef, ElementRef, afterNextRender, computed, effect, inject, signal, viewChild } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonToggle, MatButtonToggleGroup } from '@angular/material/button-toggle';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatOptgroup, MatOption, MatSelect } from '@angular/material/select';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import * as L from 'leaflet';
import { forkJoin } from 'rxjs';

import { AirQualityForecast, WeatherForecast } from '../../core/models/forecast.model';
import { METRICS } from '../../core/models/metric.model';
import { ForecastService } from '../../core/services/forecast.service';
import { RegionService } from '../../core/services/region.service';
import { BandLegendComponent } from './band-legend/band-legend.component';
import { FxLayer } from './fx-layer';
import { CardRow, MapVisualsService } from './map-visuals.service';
import { bandOf } from './metric-scales';
import { WeatherBadges } from './weather-badge';

// Forecast record read by metric key.
interface Row {
  kalcode: string;
  time: string;
  [key: string]: string | number;
}

const GREECE: L.LatLngExpression = [38.5, 23.8];
const BADGE_MIN_ZOOM = 7; // below this the weather badges crowd the map

// Map page: regional units coloured by the chosen metric and day, with an animated effect showing its scale.
@Component({
  selector: 'app-map',
  imports: [
    MatProgressBar,
    MatFormField,
    MatLabel,
    MatSelect,
    MatOption,
    MatOptgroup,
    MatButtonToggleGroup,
    MatButtonToggle,
    MatSlideToggle,
    BandLegendComponent,
  ],
  templateUrl: './map.component.html',
})
export class MapComponent {
  private readonly regions = inject(RegionService);
  private readonly forecasts = inject(ForecastService);
  private readonly mapVisuals = inject(MapVisualsService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly mapEl = viewChild.required<ElementRef<HTMLElement>>('map');

  protected readonly groups = (['Weather', 'Air quality', 'Pollen'] as const).map((name) => ({
    name,
    metrics: METRICS.filter((m) => m.group === name),
  }));

  protected readonly loading = signal(true); // map layers
  protected readonly forecastLoading = signal(true);
  protected readonly metricKey = signal('temperatureMax');
  protected readonly day = signal('');
  protected readonly effectsOn = signal(true);

  private readonly weather = signal<WeatherForecast[]>([]);
  private readonly air = signal<AirQualityForecast[]>([]);
  private readonly unitLayer = signal<L.GeoJSON | null>(null);
  private readonly overlay = signal<FxLayer | null>(null);
  private readonly badges = signal<WeatherBadges | null>(null);
  private readonly zoom = signal(0);

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

  // Weather condition code per region for the chosen day.
  private readonly conditions = computed(
    () => new Map(this.weather().filter((r) => r.time === this.day()).map((r) => [r.kalcode, r.wmoCode])),
  );

  // Gradients used by the pulsing fills (empty for other metrics).
  protected readonly gradients = computed(() => this.mapVisuals.gradients(this.metricKey(), this.values()));
  protected readonly reducedMotion = this.mapVisuals.reducedMotion;

  // First forecast day, when it is already in the past.
  protected readonly staleSince = computed(() => {
    const first = this.days()[0];
    return first && first < new Date().toISOString().slice(0, 10) ? first : null;
  });

  constructor() {
    afterNextRender(() => this.initMap());

    // Recolour the units when metric, day or data change.
    effect(() => {
      const layer = this.unitLayer();
      if (!layer) return;
      layer.setStyle((f) => this.styleFor(f?.properties?.['kalcode']));
      // The alarm border is a CSS class (styles.css), switched on for regions with a bad value.
      const key = this.metricKey();
      const values = this.values();
      layer.eachLayer((l) => {
        const kalcode = (l as L.Polygon).feature?.properties?.['kalcode'];
        (l as L.Path).getElement()?.classList.toggle('fx-alarm', this.mapVisuals.isAlarm(key, values.get(kalcode)));
      });
    });

    // Redraw the effect when metric, day or data change, or the toggle flips.
    effect(() => {
      const overlay = this.overlay();
      if (!overlay) return;
      overlay.setState(this.metricKey(), this.values());
      overlay.setEnabled(this.effectsOn() && !this.forecastLoading());
    });

    // Weather badges: only for the Weather (WMO code) metric, and only when zoomed in.
    effect(() => {
      const badges = this.badges();
      if (!badges) return;
      badges.setCodes(this.conditions());
      badges.setVisible(this.metricKey() === 'wmoCode' && this.zoom() >= BADGE_MIN_ZOOM);
    });

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
    return this.mapVisuals.styleFor(this.metricKey(), kalcode, this.values().get(kalcode));
  }

  private tooltip(kalcode: string, name: string): HTMLElement {
    const value = this.values().get(kalcode);
    const { label, unit } = this.metric();
    const band = this.mapVisuals.bandLabel(this.metricKey(), value);
    const el = document.createElement('div');
    if (value === undefined) el.textContent = `${name}: no data`;
    else if (this.metricKey() === 'wmoCode') el.textContent = `${name} · ${band} (code ${value})`;
    else el.textContent = `${name} · ${label}: ${this.format(value)} ${unit} · ${band}`;
    return el;
  }

  // Click card: every metric of the region for the chosen day, each with its band colour.
  private card(kalcode: string, name: string): HTMLElement {
    const day = this.day();
    const weather = this.weather().find((r) => r.kalcode === kalcode && r.time === day) as unknown as Row | undefined;
    const air = this.air().find((r) => r.kalcode === kalcode && r.time === day) as unknown as Row | undefined;

    const rows: CardRow[] = this.groups.flatMap((group) =>
      group.metrics.map((m) => {
        const raw = (m.source === 'weather' ? weather : air)?.[m.key];
        const value = raw === undefined ? undefined : Number(raw);
        const found = bandOf(m.key, value);
        const text =
          value === undefined ? 'no data' : m.key === 'wmoCode' ? `code ${value}` : `${this.format(value)} ${m.unit}`;
        return { group: group.name, label: m.label, text, color: found?.band.color ?? '', band: found?.band.label ?? '' };
      }),
    );
    return this.mapVisuals.card(`${name} · ${this.dayLabel(day)}`, rows);
  }

  private initMap(): void {
    // No preferCanvas: the SVG renderer is needed for gradient fills and CSS classes on the regions.
    const map = L.map(this.mapEl().nativeElement).setView(GREECE, 6);
    this.zoom.set(map.getZoom());
    map.on('zoomend', () => this.zoom.set(map.getZoom()));
    // Re-measure when the layout changes (controls wrapping, window resize).
    const resize = new ResizeObserver(() => map.invalidateSize());
    resize.observe(this.mapEl().nativeElement);
    this.destroyRef.onDestroy(() => {
      resize.disconnect();
      this.overlay()?.destroy();
      this.badges()?.destroy();
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
                layer.bindPopup(() => this.card(feature.properties['kalcode'], feature.properties['name']), { maxHeight: 360 });
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

          const overlay = new FxLayer(map);
          overlay.setRegions(units.map((u) => ({ kalcode: u.kalcode, shape: u.shape })));

          this.badges.set(new WeatherBadges(map, centers));

          this.unitLayer.set(unitLayer);
          this.overlay.set(overlay);
          this.loading.set(false);
        },
        error: () => this.loading.set(false), // ErrorService already informed the user
      });
  }
}
