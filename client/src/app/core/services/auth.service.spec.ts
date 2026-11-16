import { provideHttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { AuthResult } from '../models/auth.model';
import { AuthService } from './auth.service';

const session = (expiresAt: string): AuthResult => ({
  token: 'jwt',
  tokenType: 'Bearer',
  expiresAt,
  userId: '1',
  email: 'a@b.com',
  userType: 'BEEKEEPER',
});

describe('AuthService', () => {
  let auth: AuthService;

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({ providers: [provideHttpClient()] });
    auth = TestBed.inject(AuthService);
  });

  it('exposes the token while the session is valid', () => {
    auth.session.set(session(new Date(Date.now() + 60_000).toISOString()));
    expect(auth.isLoggedIn()).toBe(true);
    expect(auth.token()).toBe('jwt');
  });

  it('treats an expired session as logged out', () => {
    auth.session.set(session(new Date(Date.now() - 60_000).toISOString()));
    expect(auth.isLoggedIn()).toBe(false);
    expect(auth.token()).toBeNull();
  });

  it('clears the session on logout', () => {
    auth.session.set(session(new Date(Date.now() + 60_000).toISOString()));
    auth.logout();
    expect(auth.session()).toBeNull();
  });
});
