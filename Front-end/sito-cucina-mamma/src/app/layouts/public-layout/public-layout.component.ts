import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PublicHeaderComponent } from '../../shared/components/public-header/public-header.component'; // ✅ Import diretto
import { FooterComponent } from '../../shared/components/footer/footer.component'; // ✅ Import diretto
import { CookieConsentBannerComponent } from '../../shared/components/cookie-consent-banner/cookie-consent-banner.component';
import { TrackingService } from '../../core/services/tracking.service';

@Component({
  selector: 'app-public-layout',
  standalone: true, // ✅ Deve essere standalone
  imports: [
    CommonModule,
    RouterModule,
    PublicHeaderComponent,
    CookieConsentBannerComponent,
    FooterComponent,
  ], // ✅ Importa i componenti
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.scss',
})
export class PublicLayoutComponent {
  constructor(
    @Inject(TrackingService) private trackingService: TrackingService,
  ) {}

  ngOnInit(): void {
    // Subito dopo l'inizializzazione,
    // tracciamo un evento "PageView" (se analytics è abilitato)
    this.trackingService.trackEvent('PageView', {
      eventCategory: 'Navigation',
      pageUrl: window.location.href,
    });
  }
}
