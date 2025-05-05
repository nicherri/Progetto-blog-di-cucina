// src/app/core/services/session.service.ts
import { Injectable } from '@angular/core';

/**
 * Servizio per gestire la sessione tecnica dell’utente
 * tramite un cookie 'session_id' con scadenza "sliding expiration" di 30 minuti.
 * Non richiede consenso, in quanto cookie tecnico.
 */
@Injectable({ providedIn: 'root' })
export class SessionService {
  private sessionCookieName = 'session_id';
  private sessionTimeoutMinutes = 30; // Durata in minuti

  /**
   * Ritorna l'ID di sessione se esiste, altrimenti null.
   */
  public getSessionId(): string | null {
    return this.readCookie(this.sessionCookieName);
  }

  /**
   * Crea un sessionId se non esiste già e lo salva in un cookie,
   * con scadenza di 30 minuti.
   */
  public createSessionIdIfNotExists(): string {
    const existing = this.getSessionId();
    if (existing) {
      return existing;
    }
    const newId = this.generateGuid();
    this.setSessionCookie(newId, this.sessionTimeoutMinutes);
    return newId;
  }

  /**
   * Aggiorna la scadenza del cookie (sliding expiration).
   * Da chiamare a ogni azione significativa dell'utente.
   */
  public refreshSessionExpiration(): void {
    const sid = this.getSessionId();
    if (!sid) {
      // Se non esiste, la creiamo
      this.createSessionIdIfNotExists();
      return;
    }
    // Riscrive lo stesso ID, con scadenza prorogata
    this.setSessionCookie(sid, this.sessionTimeoutMinutes);
  }

  private setSessionCookie(value: string, durationMinutes: number) {
    const date = new Date();
    date.setTime(date.getTime() + durationMinutes * 60 * 1000); // Esempio: 30 min
    // In produzione, se usi HTTPS, setta Secure=true; HttpOnly=false se vuoi leggere i cookie in JS
    const cookieValue = `${this.sessionCookieName}=${value};expires=${date.toUTCString()};path=/;HttpOnly=false;Secure=false;SameSite=Lax;`;
    document.cookie = cookieValue;
  }

  private readCookie(name: string): string | null {
    const match = document.cookie.match(
      new RegExp('(^| )' + name + '=([^;]+)'),
    );
    return match ? match[2] : null;
  }

  private generateGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
      const r = (Math.random() * 16) | 0;
      const v = c === 'x' ? r : (r & 0x3) | 0x8;
      return v.toString(16);
    });
  }
}
