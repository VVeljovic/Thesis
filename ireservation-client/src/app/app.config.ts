import { APP_INITIALIZER, ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { KeycloakService } from './services/keycloak/keycloak.service';
import { provideHttpClient } from '@angular/common/http';
export function kcFactory(kcService:KeycloakService){
 return()=> kcService.init();
}
export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    // {
    //   provide: APP_INITIALIZER,
    //   deps: [KeycloakService],
    //   useFactory: kcFactory,
    //   multi: true
    // }
    provideHttpClient()]
};
