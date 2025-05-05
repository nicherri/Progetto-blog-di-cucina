import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Ingrediente } from '../core/models/ingrediente';

@Injectable({
  providedIn: 'root',
})
export class IngredientiService {
  private apiUrl = 'http://localhost:5279/api/Ingredienti';

  constructor(private http: HttpClient) {}

  getAllIngredienti(): Observable<Ingrediente[]> {
    return this.http.get<Ingrediente[]>(this.apiUrl);
  }
}
