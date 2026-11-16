import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { Observable, map, tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import { HANDLED_STATUSES } from '../interceptors/api.interceptor';
import { ApiResponse, unwrap } from '../models/api-response.model';
import { AuthResult, LoginRequest, RegisterRequest } from '../models/auth.model';

const STORAGE_KEY = 'pollinator.auth';
const url = `${environment.apiBaseUrl}/User`;

// Login, register and the stored session (JWT).
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);

  readonly session = signal<AuthResult | null>(readStored());

  // The API reports bad credentials as 500; the caller handles it.
  login(body: LoginRequest): Observable<AuthResult> {
    const context = new HttpContext().set(HANDLED_STATUSES, [500]);
    return this.save(this.http.post<ApiResponse<AuthResult>>(`${url}/login`, body, { context }));
  }

  register(body: RegisterRequest): Observable<AuthResult> {
    return this.save(this.http.post<ApiResponse<AuthResult>>(`${url}/register`, body));
  }

  logout(): void {
    this.session.set(null);
    localStorage.removeItem(STORAGE_KEY);
  }

  // Token, or null when missing/expired.
  token(): string | null {
    return this.isLoggedIn() ? this.session()!.token : null;
  }

  isLoggedIn(): boolean {
    const s = this.session();
    return !!s && Date.parse(s.expiresAt) > Date.now();
  }

  // Stores the session in memory and localStorage.
  startSession(result: AuthResult): void {
    this.session.set(result);
    localStorage.setItem(STORAGE_KEY, JSON.stringify(result));
  }

  private save(request: Observable<ApiResponse<AuthResult>>): Observable<AuthResult> {
    return request.pipe(map(unwrap), tap((result) => this.startSession(result)));
  }
}

// Restores the session after a page refresh.
function readStored(): AuthResult | null {
  try {
    return JSON.parse(localStorage.getItem(STORAGE_KEY) ?? 'null');
  } catch {
    return null;
  }
}
