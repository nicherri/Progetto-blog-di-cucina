// src/app/core/services/tracking.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EventTracking } from '../models/event-tracking.model';
import { SessionService } from './session.service';
import { CookieConsentService } from './/CookieConsentService';
import { catchError, retryWhen, delay, take, tap } from 'rxjs/operators';
import { EMPTY } from 'rxjs';

/**
 * Servizio che invia eventi di tracciamento a CucinaMammaAPI,
 * il quale li inoltra a RabbitMQ per il microservizio di analytics.
 */
@Injectable({ providedIn: 'root' })
export class TrackingService {
  private trackingApiUrl = 'http://localhost:5279/api/Tracking/log-event';

  constructor(
    private http: HttpClient,
    private cookieConsentService: CookieConsentService,
    private sessionService: SessionService,
  ) {}

  /**
   * Traccia un evento di analytics (es: "PageView", "RecipeClick").
   * Se l’utente ha negato o non scelto i cookie di analisi, non fa nulla.
   */
  public trackEvent(eventName: string, data?: Partial<EventTracking>) {
    // Rinnova la scadenza della sessione tecnica
    this.sessionService.refreshSessionExpiration();

    // Se l’utente rifiuta i cookie analytics o non ha ancora scelto, interrompiamo
    if (this.cookieConsentService.isAnalyticsConsentDenied()) return;
    if (this.cookieConsentService.isAnalyticsConsentNotSet()) return;

    // Se l’utente ha accettato analytics
    const sessionId = this.sessionService.createSessionIdIfNotExists();

    // Prepariamo il payload
    const eventPayload: EventTracking = {
      sessionId,
      eventName,
      eventCategory: data?.eventCategory ?? 'N/A',
      eventLabel: data?.eventLabel,
      eventValue: data?.eventValue,
      pageUrl: data?.pageUrl ?? window.location.href,
      referrer: data?.referrer ?? document.referrer,
      timestampUtc: new Date().toISOString(),
      timeSpentSeconds: data?.timeSpentSeconds,
      scrollDepthPercentage: data?.scrollDepthPercentage,
      additionalData: data?.additionalData,
      userId: data?.userId,
      optOut: false,

      mouseX: data?.mouseX,
      mouseY: data?.mouseY,
      scrollX: data?.scrollX,
      scrollY: data?.scrollY,
      viewportWidth: data?.viewportWidth,
      viewportHeight: data?.viewportHeight,
      elementLeft: data?.elementLeft,
      elementTop: data?.elementTop,
      elementWidth: data?.elementWidth,
      elementHeight: data?.elementHeight,
      replayChunkData: data?.replayChunkData,
      replayChunkType: data?.replayChunkType,
      funnelData: data?.funnelData,
    };

    // Invio asincrono + retry (max 3 volte)
    this.http
      .post(this.trackingApiUrl, eventPayload)
      .pipe(
        retryWhen((errors) =>
          errors.pipe(
            take(3),
            delay(1000),
            tap(() => console.warn('Retrying tracking event...')),
          ),
        ),
        catchError((err) => {
          console.error('Final error in tracking event after retries:', err);
          return EMPTY; // Non bloccare l’app
        }),
      )
      .subscribe({
        next: () => {
          // Evento inviato con successo (potresti loggare in console se vuoi)
        },
      });
  }
}
