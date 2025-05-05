import { Injectable } from '@angular/core';
import { TrackingService } from './tracking.service';
import { CookieConsentService } from './CookieConsentService';

@Injectable({ providedIn: 'root' })
export class UserBehaviorService {
  // ========== MOUSEMOVE REPLAY CHUNK ==========
  private mouseMovesBuffer: Array<{ x: number; y: number; t: number }> = [];
  private bufferFlushInterval = 5000; // Ogni 5s inviamo un chunk di movimenti
  private mouseMoveSamplingRate = 0.1; // 10% di sampling

  // ========== SCROLL REPLAY CHUNK (avanzato) ==========
  private scrollMovesBuffer: Array<{ x: number; y: number; t: number }> = [];
  private scrollFlushInterval = 3000; // flush scroll ogni 3s
  private scrollThrottlingMs = 200;
  private lastScrollTimestamp = 0;

  // ========== MUTATION OBSERVER (Session Replay Pro) ==========
  private mutationObserver: MutationObserver | null = null;
  private mutationsBuffer: any[] = [];
  private mutationFlushInterval = 5000;

  constructor(
    private trackingService: TrackingService,
    private cookieConsentService: CookieConsentService,
  ) {
    // Avvia i listener solo se l’utente ha accettato i cookie di analytics
    if (this.cookieConsentService.isAnalyticsConsentGiven()) {
      this.initializeClickTracking();
      this.initializeMouseTracking();
      this.initializeScrollTracking(); // base (profondità)
      this.initializeContinuousScrollTracking(); // se vuoi lo scroll "step by step"
      this.initializeMutationObserver(); // replay DOM
      this.startMutationFlush(); // flush delle mutazioni
      this.initializeInputTracking();
      this.startPeriodicFlushMouseMoves();
      this.startPeriodicFlushScrollMoves(); // se usi scroll “replay”
    }
  }

  // ----------------------------------------------------------------
  // 1) CLICK TRACKING (Heatmap Avanzata)
  // ----------------------------------------------------------------
  private initializeClickTracking() {
    document.addEventListener('click', (evt) => {
      const e = evt as MouseEvent;
      const targetEl = e.target as HTMLElement;

      // bounding box e coordinate
      const rect = targetEl.getBoundingClientRect();
      const scrollX = window.scrollX;
      const scrollY = window.scrollY;
      const viewportWidth = window.innerWidth;
      const viewportHeight = window.innerHeight;

      const data = {
        mouseX: e.clientX,
        mouseY: e.clientY,
        scrollX,
        scrollY,
        viewportWidth,
        viewportHeight,
        elementId: targetEl.id,
        elementRect: {
          left: rect.left,
          top: rect.top,
          width: rect.width,
          height: rect.height,
        },
      };

      // Tracciamo l'evento "Click" con un category = "Heatmap"
      this.trackingService.trackEvent('Click', {
        eventCategory: 'Heatmap',
        additionalData: JSON.stringify(data),
      });
    });
  }

  // ----------------------------------------------------------------
  // 2) MOUSE MOVE (Session Replay base, con sampling)
  // ----------------------------------------------------------------
  private initializeMouseTracking() {
    document.addEventListener('mousemove', (evt) => {
      if (Math.random() <= this.mouseMoveSamplingRate) {
        const e = evt as MouseEvent;
        this.mouseMovesBuffer.push({
          x: e.clientX,
          y: e.clientY,
          t: Date.now(),
        });
      }
    });
  }

  private startPeriodicFlushMouseMoves() {
    setInterval(() => {
      if (this.mouseMovesBuffer.length > 0) {
        const batch = [...this.mouseMovesBuffer];
        this.mouseMovesBuffer = [];
        // Usiamo "SessionReplayChunk" con eventCategory = "Replay"
        this.trackingService.trackEvent('SessionReplayChunk', {
          eventCategory: 'Replay',
          additionalData: JSON.stringify({
            type: 'mouse',
            moves: batch,
          }),
        });
      }
    }, this.bufferFlushInterval);
  }

  // ----------------------------------------------------------------
  // 3) SCROLL TRACKING (base: profondità massima)
  // ----------------------------------------------------------------
  private initializeScrollTracking() {
    let maxScrollPercentage = 0;
    window.addEventListener('scroll', () => {
      const scrollTop = window.scrollY;
      const docHeight =
        document.documentElement.scrollHeight - window.innerHeight;
      if (docHeight <= 0) return;

      const scrollPercentage = Math.round((scrollTop / docHeight) * 100);
      if (scrollPercentage > maxScrollPercentage) {
        maxScrollPercentage = scrollPercentage;

        this.trackingService.trackEvent('ScrollDepth', {
          eventCategory: 'Heatmap',
          scrollDepthPercentage: scrollPercentage,
          additionalData: JSON.stringify({ scrollTop, scrollPercentage }),
        });
      }
    });
  }

  // ----------------------------------------------------------------
  // 3.bis) SCROLL TRACKING CONTINUO (Session Replay) - OPZIONALE
  // ----------------------------------------------------------------
  // Se vuoi catturare TUTTI i movimenti di scroll (come un replay video),
  // puoi usare questo metodo invece (o in aggiunta) al "depth" base.
  private initializeContinuousScrollTracking() {
    window.addEventListener('scroll', () => {
      const now = Date.now();
      if (now - this.lastScrollTimestamp < this.scrollThrottlingMs) {
        return; // throttling per non spammare
      }
      this.lastScrollTimestamp = now;

      const scrollX = window.scrollX;
      const scrollY = window.scrollY;
      this.scrollMovesBuffer.push({ x: scrollX, y: scrollY, t: now });
    });
  }

  private startPeriodicFlushScrollMoves() {
    setInterval(() => {
      if (this.scrollMovesBuffer.length > 0) {
        const batch = [...this.scrollMovesBuffer];
        this.scrollMovesBuffer = [];
        this.trackingService.trackEvent('SessionReplayChunk', {
          eventCategory: 'Replay',
          additionalData: JSON.stringify({
            type: 'scroll',
            moves: batch,
            viewportWidth: window.innerWidth,
            viewportHeight: window.innerHeight,
          }),
        });
      }
    }, this.scrollFlushInterval);
  }

  // ----------------------------------------------------------------
  // 4) MUTATION OBSERVER (Session Replay Pro)
  // ----------------------------------------------------------------
  private initializeMutationObserver() {
    this.mutationObserver = new MutationObserver((mutations) => {
      // accumulo i mutation record
      const changes = mutations.map((m) => {
        return {
          type: m.type,
          target: (m.target as HTMLElement).tagName,
          addedNodes: Array.from(m.addedNodes).map(
            (n) => (n as HTMLElement).tagName,
          ),
          removedNodes: Array.from(m.removedNodes).map(
            (n) => (n as HTMLElement).tagName,
          ),
          // Attenzione a non includere .outerHTML completo (può essere enorme)
        };
      });
      this.mutationsBuffer.push(...changes);
    });

    this.mutationObserver.observe(document.body, {
      attributes: true,
      childList: true,
      subtree: true,
    });
  }

  private startMutationFlush() {
    setInterval(() => {
      if (this.mutationsBuffer.length > 0) {
        const batch = [...this.mutationsBuffer];
        this.mutationsBuffer = [];

        this.trackingService.trackEvent('SessionReplayChunk', {
          eventCategory: 'Replay',
          additionalData: JSON.stringify({
            type: 'mutation',
            changes: batch,
          }),
        });
      }
    }, this.mutationFlushInterval);
  }

  // ----------------------------------------------------------------
  // 5) TRACCIAMENTO INPUT (globale)
  // ----------------------------------------------------------------
  private initializeInputTracking() {
    document.addEventListener('input', (evt) => {
      const target = evt.target as HTMLInputElement;
      if (!target) return;

      // Evita campi sensibili
      if (target.type === 'password') return;

      const data = {
        fieldId: target.id,
        fieldName: target.name,
        fieldType: target.type,
        value: target.value.slice(0, 20), // mascheramento
      };

      this.trackingService.trackEvent('InputChange', {
        eventCategory: 'FormInteraction',
        additionalData: JSON.stringify(data),
      });
    });
  }

  // ----------------------------------------------------------------
  // 6) FUNNEL STEP
  // ----------------------------------------------------------------
  // Quando vuoi tracciare un passaggio di funnel (es: "AddToCart", "Payment")
  public trackFunnelStep(stepName: string, extra?: any) {
    this.trackingService.trackEvent('FunnelStep', {
      eventCategory: 'Funnel',
      eventLabel: stepName,
      additionalData: extra ? JSON.stringify(extra) : undefined,
    });
  }
}
