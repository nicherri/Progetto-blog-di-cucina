export interface Immagine {
  id: number;
  url: string;
  isCover: boolean;
  alt: string;
  nomeFileSeo: string;
  title: string;
  caption: string;
  ordine: number;
  file?: File;
  ricettaId?: number;
  ingredienteId?: number;
  passaggioPreparazioneId?: number;
  categoriaId?: number;
  sottoCategoriaId?: number;
  fatteDaVoiId?: number;
  anteprima?: string;
}
