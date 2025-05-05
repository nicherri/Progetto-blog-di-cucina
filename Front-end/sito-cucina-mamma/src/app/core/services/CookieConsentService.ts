// src/app/core/services/cookie-consent.service.ts
import { Injectable } from '@angular/core';

/**
 * Servizio per la gestione del consenso ai cookie di analytics.
 * - Salva e legge un cookie 'cookie_consent_analytics' con valore 'true' / 'false'.
 * - Determina se mostrare il banner di consenso.
 */
@Injectable({ providedIn: 'root' })
export class CookieConsentService {
  private analyticsCookieName = 'cookie_consent_analytics'; // 'true' / 'false'
  private cookieExpiryDays = 365; // es. 1 anno

  /**
   * Restituisce true se l'utente ha già accettato i cookie di analisi.
   */
  public isAnalyticsConsentGiven(): boolean {
    return this.readCookie(this.analyticsCookieName) === 'true';
  }

  /**
   * Restituisce true se l'utente ha espressamente rifiutato.
   */
  public isAnalyticsConsentDenied(): boolean {
    return this.readCookie(this.analyticsCookieName) === 'false';
  }

  /**
   * Restituisce true se non è stato ancora impostato (né 'true' né 'false').
   */
  public isAnalyticsConsentNotSet(): boolean {
    const val = this.readCookie(this.analyticsCookieName);
    return val !== 'true' && val !== 'false';
  }

  /**
   * Imposta il consenso per i cookie di analisi (true o false).
   */
  public setAnalyticsConsent(consent: boolean) {
    const date = new Date();
    date.setDate(date.getDate() + this.cookieExpiryDays); // scadenza in giorni
    const cookieValue = `${this.analyticsCookieName}=${consent};expires=${date.toUTCString()};path=/;Secure=false;HttpOnly=false;SameSite=Lax;`;
    document.cookie = cookieValue;
  }

  /**
   * Legge un cookie generico (utility).
   */
  private readCookie(name: string): string | null {
    const match = document.cookie.match(
      new RegExp('(^| )' + name + '=([^;]+)'),
    );
    return match ? match[2] : null;
  }
}
