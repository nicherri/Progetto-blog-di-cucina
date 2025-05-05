import { Immagine } from './immagine';

export interface PassaggioPreparazione {
  id: number;
  ordine: number;
  descrizione: string;
  ricettaId: number;
  immagini: Immagine[];
}
