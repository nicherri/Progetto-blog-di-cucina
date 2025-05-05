import { RicettaIngrediente } from './ricetta-ingrediente';
import { Immagine } from './immagine'; // Add this line to import the Immagine type
import { RicettaCategoria } from './ricetta-categoria'; // Add this line to import the RicettaCategoria type
import { PassaggioPreparazione } from './passaggio-preparazione'; // Add this line to import the PassaggioPreparazione type

export interface Ricetta {
  id: number;
  titolo: string;
  descrizione: string;
  tempoPreparazione: number;
  difficolta: 'Facile' | 'Media' | 'Difficile';
  slug: string;
  published: boolean;
  metaDescription: string;
  dataCreazione: Date;
  ricettaIngredienti: RicettaIngrediente[];
  immagini: Immagine[];
  ricetteCategorie: RicettaCategoria[];
  passaggiPreparazione: PassaggioPreparazione[];
  utenteId?: number;
}
