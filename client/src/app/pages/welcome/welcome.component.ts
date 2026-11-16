import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard, MatCardContent } from '@angular/material/card';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatTab, MatTabGroup } from '@angular/material/tabs';
import { Router } from '@angular/router';
import { EMPTY, Observable, finalize, map, of, switchMap } from 'rxjs';

import { environment } from '../../../environments/environment'; // REMOVE-BEFORE-PROD
import { DEV_ADMIN_SESSION } from '../../core/dev/dev-admin'; // REMOVE-BEFORE-PROD
import { AuthService } from '../../core/services/auth.service';
import { UserService } from '../../core/services/user.service';

const MIN_PASSWORD_LENGTH = 6; // matches the API's Identity rule

// Empty, non-nullable text control.
const field = (...validators: ValidatorFn[]) => new FormControl('', { nonNullable: true, validators });

// Sign in / register page.
@Component({
  selector: 'app-welcome',
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatCardContent,
    MatTabGroup,
    MatTab,
    MatFormField,
    MatLabel,
    MatError,
    MatInput,
    MatButton,
    MatProgressBar,
  ],
  templateUrl: './welcome.component.html',
})
export class WelcomeComponent {
  private readonly auth = inject(AuthService);
  private readonly users = inject(UserService);
  private readonly router = inject(Router);

  protected readonly busy = signal(false);
  protected readonly showDevAdmin = environment.devAdminLogin; // REMOVE-BEFORE-PROD
  protected readonly minPasswordLength = MIN_PASSWORD_LENGTH;

  protected readonly loginForm = new FormGroup({
    email: field(Validators.required, Validators.email),
    password: field(Validators.required),
  });

  protected readonly registerForm = new FormGroup({
    name: field(Validators.required),
    email: field(Validators.required, Validators.email),
    password: field(Validators.required, Validators.minLength(MIN_PASSWORD_LENGTH)),
  });

  constructor() {
    if (this.auth.isLoggedIn()) this.router.navigateByUrl('/map');
  }

  // Checks the account exists (1 call), then signs in.
  protected login(): void {
    const form = this.loginForm;
    if (form.invalid) {
      form.markAllAsTouched();
      return;
    }
    const { email, password } = form.getRawValue();

    this.run(
      this.users.emailExists(email).pipe(
        switchMap((exists) => {
          if (exists) return this.auth.login({ email, password });
          form.controls.email.setErrors({ notFound: true });
          return EMPTY;
        }),
      ),
      // Account exists, so a 500 from login means wrong password.
      () => form.controls.password.setErrors({ incorrect: true }),
    );
  }

  // Checks e-mail, then username (2 calls max), then registers.
  protected register(): void {
    const form = this.registerForm;
    if (form.invalid) {
      form.markAllAsTouched();
      return;
    }
    const { name, email, password } = form.getRawValue();
    const username = email.split('@')[0]; // the API derives the username this way

    this.run(
      this.users.emailExists(email).pipe(
        switchMap((taken) => (taken ? of('emailTaken') : this.users.usernameExists(username).pipe(map((t) => (t ? 'usernameTaken' : null))))),
        switchMap((conflict) => {
          if (!conflict) return this.auth.register({ name, email, password, userType: 'BEEKEEPER' });
          form.controls.email.setErrors({ [conflict]: true });
          return EMPTY;
        }),
      ),
    );
  }

  // REMOVE-BEFORE-PROD: signs in as the test admin without the API.
  protected loginAsAdmin(): void {
    this.auth.startSession(DEV_ADMIN_SESSION);
    this.router.navigateByUrl('/map');
  }

  // Runs a request with the busy flag, then opens /map.
  private run(request: Observable<unknown>, onServerError?: () => void): void {
    this.busy.set(true);
    request.pipe(finalize(() => this.busy.set(false))).subscribe({
      next: () => this.router.navigateByUrl('/map'),
      error: (err: unknown) => {
        if (err instanceof HttpErrorResponse && err.status === 500) onServerError?.();
      },
    });
  }
}
