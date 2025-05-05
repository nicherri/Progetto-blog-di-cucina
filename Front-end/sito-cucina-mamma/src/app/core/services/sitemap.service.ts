import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SitemapService {
  constructor(private http: HttpClient) {}

  generateSitemap(): Observable<string> {
    return this.http.get<string>('https://api.tuosito.com/generate-sitemap', { responseType: 'text' as 'json' });
  }
}
