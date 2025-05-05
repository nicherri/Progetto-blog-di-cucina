import { Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { ElencoRicetteComponent } from './pages/ricette/elenco-ricette/elenco-ricette.component';
import { AggiungiRicettaComponent } from './pages/ricette/aggiungi-ricetta/aggiungi-ricetta.component';
import { ModificaRicettaComponent } from './pages/ricette/modifica-ricetta/modifica-ricetta.component';
import { ElencoIngredientiComponent } from './pages/ingredienti/elenco-ingredienti/elenco-ingredienti.component';
import { AggiungiIngredienteComponent } from './pages/ingredienti/aggiungi-ingrediente/aggiungi-ingrediente.component';
import { ModificaIngredienteComponent } from './pages/ingredienti/modifica-ingrediente/modifica-ingrediente.component';
import { ElencoCategorieComponent } from './pages/categorie/elenco-categorie/elenco-categorie.component';
import { AggiungiCategoriaComponent } from './pages/categorie/aggiungi-categoria/aggiungi-categoria.component';
import { ModificaCategoriaComponent } from './pages/categorie/modifica-categoria/modifica-categoria.component';

export const adminRoutes: Routes = [
  { path: '', component: DashboardComponent },

  // Rotte per Ricette
  { path: 'ricette', component: ElencoRicetteComponent },
  { path: 'ricette/aggiungi', component: AggiungiRicettaComponent },
  { path: 'ricette/modifica/:id', component: ModificaRicettaComponent },

  // Rotte per Ingredienti
  { path: 'ingredienti', component: ElencoIngredientiComponent },
  { path: 'ingredienti/aggiungi', component: AggiungiIngredienteComponent },
  { path: 'ingredienti/modifica/:id', component: ModificaIngredienteComponent },

  // Rotte per Categorie
  { path: 'categorie', component: ElencoCategorieComponent },
  { path: 'categorie/aggiungi', component: AggiungiCategoriaComponent },
  { path: 'categorie/modifica/:id', component: ModificaCategoriaComponent },
];
