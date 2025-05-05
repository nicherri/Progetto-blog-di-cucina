export interface ImmagineForm {
  file: File;
  anteprima: string;
  ordine: number;
  alt: string;
  title: string;
  caption: string;
  nomeFileSeo: string;
  isCover: boolean;
  id?: number;
  url?: string;
  coloreWarning?: string;
  warning?: boolean;
}
