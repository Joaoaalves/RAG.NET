import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

import { environment } from './environments/environment';
import { enableProdMode } from '@angular/core';

if ((environment as any).production) {
  enableProdMode();
}
bootstrapApplication(AppComponent, appConfig).catch((err) =>
  console.error(err)
);
