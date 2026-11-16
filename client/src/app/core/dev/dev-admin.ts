// REMOVE-BEFORE-PROD: test-only admin session. Delete this whole folder before production.
import { AuthResult } from '../models/auth.model';

// Paste a fresh token here; expiresAt is read from the token itself.
const TOKEN =
  'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5YzRjNTdhOS0wZTYyLTQ4MzMtODNkYy05OWU5MzI5MTU4ZTkiLCJlbWFpbCI6ImFkbWluQG1haWwuY29tIiwianRpIjoiNTliZTQ2NTAtZmRjYy00MWU5LTkxZjctZjhjM2Q2Zjg3YTZmIiwiZXhwIjoxNzk4Mjg0MjYzLCJpc3MiOiJodHRwczovL3lvdXJhcGkuY29tIiwiYXVkIjoiaHR0cHM6Ly95b3VyY2xpZW50LmNvbSJ9.TZbddA8Iyj5uGcW1ORap01qqHAvFBAqdKXK_eJQHnFw';

// Expiry (the JWT "exp" claim, in seconds) as an ISO date.
const expiresAt = new Date(JSON.parse(atob(TOKEN.split('.')[1])).exp * 1000).toISOString();

export const DEV_ADMIN_SESSION: AuthResult = {
  token: TOKEN,
  tokenType: 'Bearer',
  expiresAt,
  userId: '9c4c57a9-0e62-4833-83dc-99e9329158e9',
  email: 'admin@mail.com',
  userType: 'BEEKEEPER',
};
