import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map, shareReplay } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse, unwrap } from '../models/api-response.model';
import { RegionCenter, RegionUnit } from '../models/region.model';

const url = `${environment.apiBaseUrl}/Ypen`;

// Regional unit boundaries and centres, cached for the session.
@Injectable({ providedIn: 'root' })
export class RegionService {
  private readonly http = inject(HttpClient);

  // The boundaries payload is large, so it is fetched only once.
  readonly units$: Observable<RegionUnit[]> = this.http
    .get<ApiResponse<RegionUnit[]>>(`${url}/RegionUnitsData`)
    .pipe(map(unwrap), shareReplay({ bufferSize: 1, refCount: false }));

  readonly centers$: Observable<RegionCenter[]> = this.http
    .get<ApiResponse<RegionCenter[]>>(`${url}/RegionCentersData`)
    .pipe(map(unwrap), shareReplay({ bufferSize: 1, refCount: false }));
}
