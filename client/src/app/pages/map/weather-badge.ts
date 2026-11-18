import * as L from 'leaflet';
// "lottie_light" is the SVG-only build: smaller, and enough for the Meteocons icons.
import lottie, { type AnimationItem } from 'lottie-web/build/player/lottie_light';

export interface BadgeCenter {
  kalcode: string;
  latitude: number;
  longitude: number;
}

const SIZE = 44; // px

// WMO weather code -> Meteocons file in public/weather/ (without .json). null = no icon.
export function weatherFile(code: number): string | null {
  if (code === 0) return 'clear-day';
  if (code === 1 || code === 2) return 'partly-cloudy-day';
  if (code === 3) return 'overcast-day';
  if (code === 45 || code === 48) return 'fog-day';
  if (code >= 51 && code <= 57) return 'drizzle';
  if ((code >= 61 && code <= 67) || (code >= 80 && code <= 82)) return 'rain';
  if ((code >= 71 && code <= 77) || code === 85 || code === 86) return 'snow';
  if (code >= 95 && code <= 99) return 'thunderstorms-day-rain';
  return null;
}

// Each icon file is downloaded once, then shared by every region that needs it.
const downloads = new Map<string, Promise<object | null>>();
function download(file: string): Promise<object | null> {
  let json = downloads.get(file);
  if (!json) {
    json = fetch(`weather/${file}.json`)
      .then((r) => (r.ok ? r.json() : null))
      .catch(() => null);
    downloads.set(file, json);
  }
  return json;
}

interface Badge {
  latlng: L.LatLng;
  el: HTMLElement; // the Lottie animation is drawn inside this
  wanted: string | null; // file for the region's current weather code
  file: string | null; // file currently loaded (or loading)
  anim?: AnimationItem;
}

// An animated Meteocons weather icon at the centre of each region.
// Only meant for the Weather metric: the map component shows it with setVisible(true) there.
export class WeatherBadges {
  private readonly layer = L.layerGroup();
  private readonly badges = new Map<string, Badge>();
  private readonly reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)');
  private visible = false;

  private readonly onMoveEnd = () => this.refresh();

  constructor(
    private readonly map: L.Map,
    centers: BadgeCenter[],
  ) {
    for (const c of centers) {
      const el = document.createElement('div');
      el.style.cssText = `width:${SIZE}px;height:${SIZE}px`;
      el.setAttribute('aria-hidden', 'true');
      const icon = L.divIcon({ className: 'wx-badge', html: el, iconSize: [SIZE, SIZE] });
      const latlng = L.latLng(c.latitude, c.longitude);
      L.marker(latlng, { icon, interactive: false, keyboard: false }).addTo(this.layer);
      this.badges.set(c.kalcode, { latlng, el, wanted: null, file: null });
    }
    map.on('moveend', this.onMoveEnd);
  }

  // Weather code per region (kalcode) for the chosen day.
  setCodes(codes: Map<string, number>): void {
    for (const [kalcode, badge] of this.badges) {
      const code = codes.get(kalcode);
      badge.wanted = code === undefined ? null : weatherFile(code);
    }
    this.refresh();
  }

  setVisible(visible: boolean): void {
    this.visible = visible;
    if (visible) this.layer.addTo(this.map);
    else this.layer.remove();
    this.refresh();
  }

  destroy(): void {
    this.map.off('moveend', this.onMoveEnd);
    this.layer.remove();
    for (const badge of this.badges.values()) badge.anim?.destroy();
    this.badges.clear();
  }

  // Brings every badge in line with the current state. Cheap to call often.
  private refresh(): void {
    for (const badge of this.badges.values()) {
      const file = this.visible ? badge.wanted : null; // nothing is loaded while the badges are hidden
      if (file !== badge.file) {
        badge.file = file;
        void this.load(badge, file);
      } else {
        this.playback(badge);
      }
    }
  }

  private async load(badge: Badge, file: string | null): Promise<void> {
    badge.anim?.destroy();
    badge.anim = undefined;
    if (!file) return;

    const json = await download(file);
    if (!json || badge.file !== file) return; // failed, or the region's weather changed meanwhile
    badge.anim = lottie.loadAnimation({
      container: badge.el,
      renderer: 'svg',
      loop: true,
      autoplay: false,
      animationData: structuredClone(json),
    });
    this.playback(badge);
  }

  // Animate only badges in view (and only if the OS allows motion); the others hold still.
  private playback(badge: Badge): void {
    if (!badge.anim) return;
    const inView = this.map.getBounds().pad(0.1).contains(badge.latlng);
    if (inView && !this.reducedMotion.matches) badge.anim.play();
    else badge.anim.pause();
  }
}
