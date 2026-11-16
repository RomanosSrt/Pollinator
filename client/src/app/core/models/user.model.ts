// Returned by the findBy* lookups; only existence matters here.
export interface User {
  email: string;
  name: string;
  userType: string;
}
