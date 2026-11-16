import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AuthService } from '../services/auth.service';

// Lets signed-in users through; others go to the welcome page.
export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  if (auth.isLoggedIn()) return true;
  auth.logout(); // clear any expired session
  return inject(Router).parseUrl('/');
};
