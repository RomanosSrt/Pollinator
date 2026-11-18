import { Injectable } from '@angular/core';
import * as L from 'leaflet';

import { METRIC_SCALES, bandOf } from './metric-scales';

// Region with no value for the chosen metric and day.
const NO_DATA: L.PathOptions = { color: '#888', weight: 1, fillColor: '#ccc', fillOpacity: 0.3 };

// One radial gradient in the map template's hidden <svg><defs>.
export interface Gradient {
  id: string;
  color: string;
  dur: number; // seconds per pulse
}

// One line of the click card: a metric with its value and band.
export interface CardRow {
  group: string; // section heading, e.g. "Air quality"
  label: string;
  text: string; // value with unit, or "no data"
  color: string; // band colour for the dot ('' = none)
  band: string;
}

// Turns a metric value into how a region is drawn on the map.
@Injectable({ providedIn: 'root' })
export class MapVisualsService {
  // Animations are skipped when the OS asks for less motion.
  readonly reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

  // Leaflet style for one region: a solid band colour, or the region's gradient for 'pulse' metrics.
  styleFor(key: string, kalcode: string, value: number | undefined): L.PathOptions {
    const found = bandOf(key, value);
    if (!found) return NO_DATA;
    if (METRIC_SCALES[key].fill === 'pulse') {
      return { color: '#555', weight: 1, fillColor: `url(#${this.gradientId(kalcode)})`, fillOpacity: 1 };
    }
    return { color: '#555', weight: 1, fillColor: found.band.color, fillOpacity: 0.6 };
  }

  // Band name for the tooltip, e.g. "Poor". Empty when there is no value.
  bandLabel(key: string, value: number | undefined): string {
    return bandOf(key, value)?.band.label ?? '';
  }

  // True when the value is bad enough for the red alarm border (see `alarmFrom` in metric-scales.ts).
  isAlarm(key: string, value: number | undefined): boolean {
    const alarmFrom = METRIC_SCALES[key]?.alarmFrom;
    const found = bandOf(key, value);
    return alarmFrom !== undefined && found !== undefined && found.index >= alarmFrom;
  }

  // The card shown when a region is clicked: every metric with a coloured dot for its band.
  // Built with the DOM (not an HTML string); styled by the .region-card classes in styles.css.
  card(title: string, rows: CardRow[]): HTMLElement {
    const card = document.createElement('div');
    card.className = 'region-card';

    const heading = document.createElement('div');
    heading.className = 'region-card-title';
    heading.textContent = title;
    card.appendChild(heading);

    let group = '';
    for (const row of rows) {
      if (row.group !== group) {
        group = row.group;
        const h = document.createElement('div');
        h.className = 'region-card-group';
        h.textContent = group;
        card.appendChild(h);
      }
      const line = document.createElement('div');
      line.className = 'region-card-row';

      const dot = document.createElement('span');
      dot.className = 'region-card-dot';
      if (row.color) dot.style.background = row.color;

      const label = document.createElement('span');
      label.className = 'region-card-label';
      label.textContent = row.label;

      const value = document.createElement('span');
      value.textContent = row.band ? `${row.text} · ${row.band}` : row.text;

      line.append(dot, label, value);
      card.appendChild(line);
    }
    return card;
  }

  // One gradient per region that has a value; empty unless the metric pulses.
  gradients(key: string, values: Map<string, number>): Gradient[] {
    if (METRIC_SCALES[key]?.fill !== 'pulse') return [];
    const result: Gradient[] = [];
    for (const [kalcode, value] of values) {
      const found = bandOf(key, value);
      if (found) result.push({ id: this.gradientId(kalcode), color: found.band.color, dur: this.pulseSeconds(found.index) });
    }
    return result;
  }

  private gradientId(kalcode: string): string {
    return `g-${kalcode}`;
  }

  // Worse band (higher index) = faster pulse: 4 s for the first band, down to 1.2 s.
  private pulseSeconds(bandIndex: number): number {
    return Math.max(1.2, 4 - bandIndex * 0.6);
  }
}
