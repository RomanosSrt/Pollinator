import { type Container, tsParticles } from '@tsparticles/engine';
import { loadSlim } from '@tsparticles/slim';
import type { MultiPolygon } from 'geojson';
import * as L from 'leaflet';

import { presetFor } from './fx-presets';
import { METRIC_SCALES, bandOf } from './metric-scales';

export interface FxRegion {
  kalcode: string;
  shape: MultiPolygon;
}

const MIN_SIDE = 24; // px; regions smaller than this on screen get no particles
const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)');

// The tsParticles engine only needs loading once.
let engineReady: Promise<void> | undefined;
function ensureEngine(): Promise<void> {
  engineReady ??= (async () => {
    await loadSlim(tsParticles);
    await tsParticles.init();
  })();
  return engineReady;
}

// Animated particles inside each region of the map.
// Every region gets its own <div> in a custom Leaflet pane, cut to the region's outline with CSS
// clip-path, and a tsParticles container is created inside that div.
export class FxLayer {
  private readonly pane: HTMLElement;
  private regions: FxRegion[] = [];
  private metricKey = '';
  private values = new Map<string, number>();
  private enabled = true;

  private divs: HTMLElement[] = [];
  private containers: Container[] = [];
  private generation = 0; // grows with every rebuild, so an older, slower rebuild can tell it is out of date
  private rebuildQueued = false;

  private readonly onZoomStart = () => (this.pane.style.display = 'none'); // positions are wrong mid-zoom
  private readonly onZoomEnd = () => {
    this.pane.style.display = '';
    this.queueRebuild();
  };

  constructor(private readonly map: L.Map) {
    this.pane = map.getPane('fx') ?? map.createPane('fx');
    this.pane.style.zIndex = '450'; // above the regions (400), below markers (600) and tooltips (650)
    this.pane.style.pointerEvents = 'none'; // the mouse still reaches the regions underneath

    map.on('zoomstart', this.onZoomStart);
    map.on('zoomend', this.onZoomEnd);
  }

  setRegions(regions: FxRegion[]): void {
    this.regions = regions;
    this.queueRebuild();
  }

  // The metric to show and its value per region (kalcode).
  setState(metricKey: string, values: Map<string, number>): void {
    this.metricKey = metricKey;
    this.values = values;
    this.queueRebuild();
  }

  setEnabled(enabled: boolean): void {
    this.enabled = enabled;
    this.queueRebuild();
  }

  destroy(): void {
    this.generation++;
    this.clear();
    this.map.off('zoomstart', this.onZoomStart);
    this.map.off('zoomend', this.onZoomEnd);
    this.pane.remove();
  }

  // Several changes often arrive together (metric, day, toggle): rebuild once, on the next frame.
  private queueRebuild(): void {
    if (this.rebuildQueued) return;
    this.rebuildQueued = true;
    requestAnimationFrame(() => {
      this.rebuildQueued = false;
      void this.rebuild();
    });
  }

  private clear(): void {
    for (const c of this.containers) c.destroy();
    for (const d of this.divs) d.remove();
    this.containers = [];
    this.divs = [];
  }

  private async rebuild(): Promise<void> {
    const generation = ++this.generation;
    this.clear();

    const scale = METRIC_SCALES[this.metricKey];
    if (!this.enabled || reducedMotion.matches || !scale || scale.anim === 'none' || !this.regions.length) return;
    if (!this.map.getContainer().isConnected) return;

    await ensureEngine();
    if (generation !== this.generation) return; // a newer rebuild started while we waited

    const lastBand = scale.bands.length - 1;
    await Promise.all(
      this.regions.map(async (region) => {
        const found = bandOf(this.metricKey, this.values.get(region.kalcode));
        if (!found) return;
        const box = this.outline(region.shape);
        if (!box) return;

        const size = Math.min(Math.max(Math.sqrt(box.w * box.h) / 150, 0.4), 2);
        const preset = presetFor(scale.anim, found.index / lastBand, found.band.label, size);
        if (!preset) return;

        const div = document.createElement('div');
        div.id = `fx-${region.kalcode}`;
        div.style.cssText = `position:absolute;left:${box.x}px;top:${box.y}px;width:${box.w}px;height:${box.h}px;clip-path:${box.clip}`;
        if (preset.blur) div.style.filter = `blur(${preset.blur}px)`;
        if (preset.flash) {
          div.classList.add('fx-flash');
          div.style.animationDelay = `${(Math.random() * 4).toFixed(1)}s`; // storms do not flash in sync
        }
        this.pane.appendChild(div);
        this.divs.push(div);

        const container = await tsParticles.load({ id: div.id, element: div, options: preset.options });
        if (!container) return;
        if (generation !== this.generation) {
          container.destroy(); // too late, the map has moved on
          return;
        }
        this.containers.push(container);
      }),
    );
  }

  // Bounding box of the region on screen, and a clip-path that cuts the div to the region's outline.
  private outline(shape: MultiPolygon): { x: number; y: number; w: number; h: number; clip: string } | null {
    const rings: L.Point[][] = [];
    let x0 = Infinity;
    let y0 = Infinity;
    let x1 = -Infinity;
    let y1 = -Infinity;

    for (const polygon of shape.coordinates) {
      for (const ring of polygon) {
        const points: L.Point[] = [];
        for (const [lng, lat] of ring) {
          const p = this.map.latLngToLayerPoint([lat, lng]);
          const last = points[points.length - 1];
          if (last && Math.abs(p.x - last.x) + Math.abs(p.y - last.y) < 1) continue; // skip tiny steps
          points.push(p);
          x0 = Math.min(x0, p.x);
          y0 = Math.min(y0, p.y);
          x1 = Math.max(x1, p.x);
          y1 = Math.max(y1, p.y);
        }
        if (points.length > 2) rings.push(points);
      }
    }

    const w = x1 - x0;
    const h = y1 - y0;
    if (!(w >= MIN_SIDE) || !(h >= MIN_SIDE)) return null;

    // One path with a sub-path per ring (islands, holes); evenodd leaves the holes empty.
    const d = rings
      .map((ring) => ring.map((p, i) => `${i ? 'L' : 'M'}${(p.x - x0).toFixed(1)} ${(p.y - y0).toFixed(1)}`).join('') + 'Z')
      .join('');
    return { x: x0, y: y0, w, h, clip: `path(evenodd, '${d}')` };
  }
}
