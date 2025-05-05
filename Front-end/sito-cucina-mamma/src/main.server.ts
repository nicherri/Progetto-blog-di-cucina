import { AppComponent } from './app/app.component';
import { appRoutes } from './app/app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideClientHydration } from '@angular/platform-browser';
import { provideServerRendering } from '@angular/platform-server';
import { provideRouter } from '@angular/router';
import { importProvidersFrom } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { ApplicationRef } from '@angular/core';

export default (): Promise<ApplicationRef> => {
  return bootstrapApplication(AppComponent, {
    providers: [
      provideHttpClient(),
      provideAnimations(),
      provideClientHydration(),
      provideServerRendering(),
      provideRouter(appRoutes),
      importProvidersFrom(), // ← rimuovilo se non stai importando niente
    ],
  });
};
