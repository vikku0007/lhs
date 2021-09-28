// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { Environment } from './base.environment';

export const environment: Environment = {
  production: false,
  // baseUrl: 'http://localhost:5001/',
  baseUrl: 'http://localhost:62431/',
  // baseUrl: 'https://roster-webapi.azurewebsites.net/'
  //  baseUrl: 'http://52.25.96.244:7031/',    
  //docUrl: 'http://localhost:5001/Docs/',
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
