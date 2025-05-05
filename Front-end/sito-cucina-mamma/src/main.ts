import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import {
  provideHttpClient,
  withInterceptorsFromDi, // 👈 importa questo
} from '@angular/common/http';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ProblemDetailsInterceptor } from './app/core/interceptors/problem-details.interceptor';
import { AppComponent } from './app/app.component';
import { appRoutes } from './app/app.routes';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(appRoutes),

    // prende tutti gli interceptor dichiarati nel DI
    provideHttpClient(withInterceptorsFromDi()),

    // registra la tua classe come interceptor
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ProblemDetailsInterceptor,
      multi: true,
    },

    provideAnimations(),
  ],
}).catch(console.error);
