import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { RouterOutlet } from '@angular/router';
import { SitemapService } from './core/services/sitemap.service';
import { TrackingService } from './core/services/tracking.service'; // Importa il tuo TrackingService
import { UserBehaviorService } from './core/services/user-behavior.service'; // Importa il tuo UserBehaviorService
import { Renderer2, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DOCUMENT } from '@angular/common';
import {
  AlertModalService,
  AlertModalData,
} from './core/services/alert-modal.service';
import { AlertModalComponent } from './shared/components/alert-modal/alert-modal.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AlertModalComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'LoveAndCooking';
  alert: AlertModalData | null = null;

  constructor(
    private sitemapService: SitemapService,
    private router: Router,
    private trackingService: TrackingService,
    private userBehaviorService: UserBehaviorService,
    private renderer: Renderer2,
    private alertService: AlertModalService,
    @Inject(DOCUMENT) private document: Document,
  ) {
    // Generazione sitemap (già presente nel tuo codice)
    this.sitemapService.generateSitemap().subscribe();

    // 🔔 Sottoscrizione al servizio alert
    this.alertService.alert$.subscribe((data) => {
      this.alert = data;
    });

    // Sottoscrizione agli eventi di routing
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        // L'utente ha completato la navigazione
        const pageUrl = event.urlAfterRedirects;

        // 🔥 Aggiunta dinamica classe body
        const body = this.document.body;
        this.renderer.removeClass(body, 'admin');
        this.renderer.removeClass(body, 'public');

        if (pageUrl.startsWith('/admin')) {
          this.renderer.addClass(body, 'admin');
        } else {
          this.renderer.addClass(body, 'public');
        }

        // Esempio di "PageView" con un eventCategory = "Navigation"
        this.trackingService.trackEvent('PageView', {
          eventCategory: 'Navigation',
          pageUrl,
          // Se vuoi passare altre info (es. ID ricetta), puoi aggiungere:
          additionalData: JSON.stringify({
            // recipeId: ... estrai i param dall'ActivatedRoute se necessario
          }),
        });
      }
    });
  }

  // chiude e passa all’eventuale prossimo alert
  onClose(): void {
    this.alertService.closeAlert();
  }

  // esegue la callback (se c’è) e poi chiude
  onConfirm(): void {
    this.alert?.onConfirm?.();
    this.alertService.closeAlert();
  }
}
