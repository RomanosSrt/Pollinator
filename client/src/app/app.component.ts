import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatToolbar } from '@angular/material/toolbar';
import { Router, RouterOutlet } from '@angular/router';

import { AuthService } from './core/services/auth.service';

// App shell: toolbar plus the routed page.
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MatToolbar, MatButton],
  templateUrl: './app.component.html',
})
export class AppComponent {
  private readonly router = inject(Router);
  protected readonly auth = inject(AuthService);

  protected logout(): void {
    this.auth.logout();
    this.router.navigateByUrl('/');
  }
}
