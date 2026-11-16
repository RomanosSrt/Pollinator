import type { MultiPolygon } from 'geojson';

// Regional unit centre point (Ypen/RegionCentersData).
export interface RegionCenter {
  kalcode: string;
  name: string;
  region: string;
  latitude: number;
  longitude: number;
}

// Regional unit with its boundary (Ypen/RegionUnitsData).
export interface RegionUnit extends RegionCenter {
  shape: MultiPolygon;
  area: number;
}
