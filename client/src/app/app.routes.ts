import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    title: 'Pollinator',
    loadComponent: () => import('./pages/welcome/welcome.component').then((m) => m.WelcomeComponent),
  },
  {
    path: 'map',
    title: 'Pollinator · Map',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/map/map.component').then((m) => m.MapComponent),
  },
  {
    path: '**',
    title: 'Pollinator · Not found',
    loadComponent: () => import('./pages/error/error.component').then((m) => m.ErrorComponent),
  },
];
