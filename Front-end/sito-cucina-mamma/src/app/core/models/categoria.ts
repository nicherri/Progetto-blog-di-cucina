import { Immagine } from './immagine';
import { RicettaCategoria } from './ricetta-categoria';
import { CategoriaSottoCategoria } from './categoria-sottocategoria';

export interface Categoria {
  id: number;
  nome: string;
  descrizione?: string;
  slug: string; // ⬅️ obbligatorio
  seoTitle?: string;
  seoDescription?: string;
  immagini: Immagine[];
  ricetteCategorie: RicettaCategoria[];
  categorieSottoCategorie: CategoriaSottoCategoria[];
}
