import { Component, OnInit } from '@angular/core';
import { CookieConsentService } from '../../../../app/core/services/CookieConsentService';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cookie-consent-banner',
  imports: [CommonModule],
  templateUrl: './cookie-consent-banner.component.html',
  styleUrl: './cookie-consent-banner.component.scss',
})
export class CookieConsentBannerComponent implements OnInit {
  showBanner = false;

  constructor(private cookieConsentService: CookieConsentService) {}

  ngOnInit(): void {
    // Se non è stato ancora impostato (né true né false), mostriamo il banner
    this.showBanner = this.cookieConsentService.isAnalyticsConsentNotSet();
  }

  acceptAnalytics(): void {
    this.cookieConsentService.setAnalyticsConsent(true);
    this.showBanner = false;
  }

  denyAnalytics(): void {
    this.cookieConsentService.setAnalyticsConsent(false);
    this.showBanner = false;
  }
}
