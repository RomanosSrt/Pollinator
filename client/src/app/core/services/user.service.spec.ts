import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { UserService } from './user.service';

const envelope = (email: string | null) => ({
  success: true,
  statusCode: 200,
  data: email === null ? null : { email, name: '', userType: 'BEEKEEPER' },
  message: null,
  traceId: null,
});

describe('UserService', () => {
  let users: UserService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting()] });
    users = TestBed.inject(UserService);
    http = TestBed.inject(HttpTestingController);
  });

  it('returns true when the API returns a user', () => {
    let result: boolean | undefined;
    users.emailExists('a@b.com').subscribe((r) => (result = r));
    http.expectOne((req) => req.url.endsWith('/findByEmail/a%40b.com')).flush(envelope('a@b.com'));
    expect(result).toBe(true);
  });

  it('returns false when the API returns no data', () => {
    let result: boolean | undefined;
    users.usernameExists('nobody').subscribe((r) => (result = r));
    http.expectOne((req) => req.url.endsWith('/findByUsername/nobody')).flush(envelope(null));
    expect(result).toBe(false);
  });
});
