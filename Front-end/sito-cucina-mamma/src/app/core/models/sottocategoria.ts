import { Immagine } from './immagine';
import { CategoriaSottoCategoria } from './categoria-sottocategoria';

export interface SottoCategoria {
  id: number;
  nome: string;
  descrizione?: string;
  immagini: Immagine[];
  categorieSottoCategorie: CategoriaSottoCategoria[];
}
