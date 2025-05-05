import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Immagine } from '../models/immagine';

@Injectable({ providedIn: 'root' })
export class ImmagineService {
  private apiUrl = 'http://localhost:5279/api/immagine';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Immagine[]> {
    return this.http.get<Immagine[]>(this.apiUrl);
  }

  getById(id: number): Observable<Immagine> {
    return this.http.get<Immagine>(`${this.apiUrl}/${id}`);
  }

  updateMetadati(id: number, immagine: Immagine): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, immagine);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  upload(tipo: string, entitaId: number, formData: FormData): Observable<any> {
    return this.http.post<any>(
      `${this.apiUrl}/upload?tipo=${tipo}&entitaId=${entitaId}`,
      formData,
    );
  }
}
