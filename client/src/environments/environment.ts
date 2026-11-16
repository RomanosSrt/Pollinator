// Dev settings; swapped for environment.prod.ts in production builds.
export const environment = {
  apiBaseUrl: 'https://pollinator-api-btcrdvandwfygkfa.italynorth-01.azurewebsites.net/api',
  logHttp: true, // log requests/responses to the console
  devAdminLogin: true, // REMOVE-BEFORE-PROD: shows the test admin button
};
