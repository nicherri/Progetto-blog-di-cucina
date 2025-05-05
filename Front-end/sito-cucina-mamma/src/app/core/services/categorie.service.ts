import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Categoria } from '../models/categoria';
import { SottoCategoria } from '../models/sottocategoria';

@Injectable({ providedIn: 'root' })
export class CategorieService {
  private apiUrl = 'http://localhost:5279/api/Categoria';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Categoria[]> {
    return this.http.get<Categoria[]>(this.apiUrl);
  }

  getById(id: number): Observable<Categoria> {
    return this.http.get<Categoria>(`${this.apiUrl}/${id}`);
  }

  create(formData: FormData): Observable<Categoria> {
    return this.http.post<Categoria>(this.apiUrl, formData);
  }

  update(id: number, dto: FormData): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getSottoCategorie(id: number): Observable<SottoCategoria[]> {
    return this.http.get<SottoCategoria[]>(
      `${this.apiUrl}/${id}/sottocategorie`,
    );
  }

  addSottoCategoria(id: number, sottoCategoriaId: number): Observable<void> {
    return this.http.post<void>(
      `${this.apiUrl}/${id}/sottocategorie/${sottoCategoriaId}`,
      {},
    );
  }

  removeSottoCategoria(id: number, sottoCategoriaId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}/sottocategorie/${sottoCategoriaId}`,
    );
  }

  getCategorieBySottoCategoria(
    sottoCategoriaId: number,
  ): Observable<Categoria[]> {
    return this.http.get<Categoria[]>(
      `${this.apiUrl}/sottocategorie/${sottoCategoriaId}/categorie`,
    );
  }
}
