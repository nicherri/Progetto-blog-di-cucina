import { RicettaIngrediente } from './ricetta-ingrediente';
import { Immagine } from './immagine';

export interface Ingrediente {
  id: number;
  nome: string;
  ricettaIngredienti: RicettaIngrediente[];
  immagini: Immagine[];
}
