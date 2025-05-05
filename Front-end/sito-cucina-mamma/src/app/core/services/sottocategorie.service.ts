import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SottoCategoria } from '../models/sottocategoria';

@Injectable({ providedIn: 'root' })
export class SottoCategorieService {
  private apiUrl = 'http://localhost:5279/api/sottocategoria';

  constructor(private http: HttpClient) {}

  getAll(): Observable<SottoCategoria[]> {
    return this.http.get<SottoCategoria[]>(`${this.apiUrl}`);
  }

  getById(id: number): Observable<SottoCategoria> {
    return this.http.get<SottoCategoria>(`${this.apiUrl}/${id}`);
  }

  create(sottoCategoria: SottoCategoria): Observable<SottoCategoria> {
    return this.http.post<SottoCategoria>(`${this.apiUrl}`, sottoCategoria);
  }

  update(id: number, dto: FormData): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
