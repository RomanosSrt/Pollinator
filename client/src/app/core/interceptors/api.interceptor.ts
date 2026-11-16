import { HttpContextToken, HttpErrorResponse, HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, tap, throwError } from 'rxjs';

import { environment } from '../../../environments/environment';
import { AuthService } from '../services/auth.service';
import { ErrorService } from '../services/error.service';

// Error statuses the caller handles itself (no error dialog).
export const HANDLED_STATUSES = new HttpContextToken<number[]>(() => []);

// Adds the JWT, logs traffic, reports failures to ErrorService.
export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const errors = inject(ErrorService);
  const router = inject(Router);

  const token = auth.token();
  const isApi = req.url.startsWith(environment.apiBaseUrl);
  const outgoing = token && isApi ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req;

  const started = performance.now();
  const ms = () => Math.round(performance.now() - started);
  if (environment.logHttp) console.log(`→ ${req.method} ${req.urlWithParams}`, maskSecrets(req.body));

  return next(outgoing).pipe(
    tap((event) => {
      if (environment.logHttp && event instanceof HttpResponse) {
        console.log(`← ${event.status} ${req.method} ${req.url} (${ms()}ms)`, event.body);
      }
    }),
    catchError((err: unknown) => {
      if (err instanceof HttpErrorResponse) {
        if (environment.logHttp) console.error(`← ${err.status} ${req.method} ${req.url} (${ms()}ms)`, err.error);
        if (err.status === 401 && token) {
          auth.logout(); // token rejected: back to the welcome page
          router.navigateByUrl('/');
        }
        if (!req.context.get(HANDLED_STATUSES).includes(err.status)) errors.show(err);
      }
      return throwError(() => err);
    }),
  );
};

// Hides passwords before logging.
function maskSecrets(body: unknown): unknown {
  return body && typeof body === 'object' && 'password' in body ? { ...body, password: '***' } : body;
}
