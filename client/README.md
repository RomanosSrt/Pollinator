# Pollinator — Angular client

A deliberately small Angular 21 app that talks to the Pollinator ASP.NET Core API.
Right now it does one thing: **register / log in** a beekeeper through `UserController`,
then show a page that explains what happened.

## Run it

1. Start the backend with the **http** launch profile (API on `http://localhost:5147`).
2. In this folder:

   ```bash
   npm install     # first time, or after package.json changes
   npm start       # = ng serve  ->  http://localhost:4200
   ```

3. Run the unit tests: `npm test`

## Folder structure

```
src/
├── index.html                 # the single HTML page; <app-root> is replaced by App
├── main.ts                    # starts Angular with App + appConfig
├── styles.css                 # global styles (colours, buttons, form fields)
├── environments/
│   └── environment.ts         # apiBaseUrl
└── app/
    ├── app.ts / .html / .css  # shell: header + <router-outlet>
    ├── app.config.ts          # providers: router, HttpClient
    ├── app.routes.ts          # URL -> page
    ├── core/                  # non-visual code shared by pages
    │   ├── models/            # TypeScript interfaces mirroring the backend DTOs
    │   ├── services/          # AuthService: the only code that calls UserController
    │   └── guards/            # authGuard: protects /session
    └── pages/                 # one folder per routed page
        ├── welcome/           # "/"        intro + login/register forms
        └── session/           # "/session" what happened during login/register
```

## Flow

`WelcomePage` (form) → `AuthService.login()/register()` → `POST /api/User/...`
→ unwrap `ApiResponse` envelope → save in signal + localStorage
→ router navigates to `/session` → `authGuard` allows it → `SessionPage` shows the details.
