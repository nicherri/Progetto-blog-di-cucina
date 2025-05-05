import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Ricetta } from '../core/models/ricetta';

@Injectable({
  providedIn: 'root',
})
export class RicetteService {
  private apiUrl = 'http://localhost:5279/api/Ricetta'; // Sostituisci con la tua API

  constructor(private http: HttpClient) {}

  getRicettaBySlug(slug: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${slug}`);
  }

  getAllRicette(): Observable<Ricetta[]> {
    return this.http.get<Ricetta[]>(this.apiUrl);
  }

  getRicettaById(id: number): Observable<Ricetta> {
    return this.http.get<Ricetta>(`${this.apiUrl}/${id}`);
  }
}
