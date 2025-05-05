import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { AlertModalService } from '../services/alert-modal.service';

interface ApiProblem {
  status: number;
  title: string;
  userMessage?: string;
  severity?: 'Info' | 'Warning' | 'Error' | 'Fatal';
  errors?: Record<string, string[]>;
  retryAfter?: number;
}

@Injectable({ providedIn: 'root' })
export class ProblemDetailsInterceptor implements HttpInterceptor {
  constructor(private alerts: AlertModalService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.error && err.error.status) {
          const p = err.error as ApiProblem;

          // 1. Mostra il modale (mapping severity→type)
          const type = mapSeverity(p.severity, p.status);
          const message = p.userMessage || p.title || 'Errore';

          this.alerts.showAlert({ title: p.title, message, type });

          // 2. Propaga l’oggetto al caller (es. per i form)
          return throwError(() => p);
        }

        // fallback: errore non formattato
        this.alerts.showError('Errore', err.message);
        return throwError(() => err);
      }),
    );
  }
}

function mapSeverity(
  sev: ApiProblem['severity'],
  status: number,
): 'success' | 'warning' | 'error' {
  if (status >= 500 || sev === 'Fatal' || sev === 'Error') return 'error';
  if (status === 400 || sev === 'Warning') return 'warning';
  return 'success';
}
