import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map, shareReplay } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse, unwrap } from '../models/api-response.model';
import { AirQualityForecast, WeatherForecast } from '../models/forecast.model';

const url = `${environment.apiBaseUrl}/Forecast`;

// Forecasts for all regions and days, cached for the session.
@Injectable({ providedIn: 'root' })
export class ForecastService {
  private readonly http = inject(HttpClient);

  readonly weather$: Observable<WeatherForecast[]> = this.http
    .get<ApiResponse<WeatherForecast[]>>(`${url}/weatherIndexes`)
    .pipe(map(unwrap), shareReplay({ bufferSize: 1, refCount: false }));

  readonly airQuality$: Observable<AirQualityForecast[]> = this.http
    .get<ApiResponse<AirQualityForecast[]>>(`${url}/airQualityIndexes`)
    .pipe(map(unwrap), shareReplay({ bufferSize: 1, refCount: false }));
}
