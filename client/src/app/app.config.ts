import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideNgxStripe } from 'ngx-stripe';

import { routes } from './app.routes';
import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { JwtInterceptor } from './interceptors/jwt-interceptor';
import { provideCharts, withDefaultRegisterables } from 'ng2-charts';
import { environment } from 'src/environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideHttpClient(withInterceptorsFromDi()),
    provideRouter(routes),
    provideNgxStripe(environment.stripePublishableKey),
    provideCharts(withDefaultRegisterables()),
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
};
