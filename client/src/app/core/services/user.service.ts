import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { User } from '../models/user.model';

const url = `${environment.apiBaseUrl}/User`;

// User existence lookups, called on submit only.
@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly http = inject(HttpClient);

  emailExists(email: string): Observable<boolean> {
    return this.exists('findByEmail', email);
  }

  usernameExists(username: string): Observable<boolean> {
    return this.exists('findByUsername', username);
  }

  // The API returns data: null when nothing matches.
  private exists(endpoint: string, value: string): Observable<boolean> {
    return this.http
      .get<ApiResponse<User>>(`${url}/${endpoint}/${encodeURIComponent(value)}`)
      .pipe(map((res) => !!res.data?.email));
  }
}
