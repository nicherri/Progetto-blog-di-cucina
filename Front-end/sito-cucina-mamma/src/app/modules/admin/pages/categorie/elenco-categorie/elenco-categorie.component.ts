import { Component, OnInit } from '@angular/core';
import { CategorieService } from '../../../../../core/services/categorie.service';
import { Categoria } from '../../../../../core/models/categoria';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { trigger, transition, style, animate } from '@angular/animations';
import { ConfermaDialogComponent } from '../../../../../shared/components/conferma-dialog/conferma-dialog.component';
import { TabellaGenericaComponent } from '../../../../../shared/components/tabella-generica/tabella-generica.component'; // ✅ importa la tabella generica

@Component({
  selector: 'app-elenco-categorie',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ConfermaDialogComponent,
    TabellaGenericaComponent,
  ],
  templateUrl: './elenco-categorie.component.html',
  styleUrls: ['./elenco-categorie.component.scss'],
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(20px)' }),
        animate(
          '400ms ease-out',
          style({ opacity: 1, transform: 'translateY(0)' }),
        ),
      ]),
    ]),
  ],
})
export class ElencoCategorieComponent implements OnInit {
  categorie: Categoria[] = [];
  loading = true;
  errore: string = '';
  mostraModale = false;
  categoriaDaEliminare: Categoria | null = null;

  // ✅ colonne dinamiche per la tabella generica
  colonne = [
    { campo: 'coverImgHtml', intestazione: 'Immagine Cover' },
    { campo: 'nome', intestazione: 'Nome' },
    { campo: 'descrizioneTroncata', intestazione: 'Descrizione' },
    { campo: 'azioniHtml', intestazione: 'Azione' },
  ];

  constructor(
    private categoriaService: CategorieService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.caricaCategorie();

    // ✅ listener per click su pulsante elimina nella tabella
    document.addEventListener('eliminaCategoria', (e: any) => {
      const id = e.detail;
      const categoria = this.categorie.find((c) => c.id === id);
      if (categoria) this.confermaEliminazione(categoria);
    });
  }

  caricaCategorie(): void {
    this.loading = true;
    this.categoriaService.getAll().subscribe({
      next: (categorie) => {
        this.categorie = categorie;
        this.loading = false;
      },
      error: (err) => {
        this.errore = 'Errore nel caricamento delle categorie';
        console.error(err);
        this.loading = false;
      },
    });
  }

  get categorieConMetadati() {
    return this.categorie.map((c) => {
      const descrizione = c.descrizione ?? '';
      const coverUrl = this.getCoverUrl(c);
      const descrizioneTroncata =
        descrizione.length > 100
          ? `${descrizione.slice(0, 100)}... <span class='mostra-altro'>Mostra altro</span>`
          : descrizione;

      return {
        ...c,
        coverImgHtml: coverUrl
          ? `<img src="${coverUrl}" alt="cover" class="cover-img" />`
          : '',
        descrizioneTroncata,
        azioniHtml: `
          <button
            class="btn-action delete modern"
            title="Elimina categoria"
            onclick="event.stopPropagation(); document.dispatchEvent(new CustomEvent('eliminaCategoria', { detail: ${c.id} }))"
          >
            <svg class="icon-trash" xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="none"
                 viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                    d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6M1 7h22M10 3h4a1 1 0 011 1v1H9V4a1 1 0 011-1z"/>
            </svg>
          </button>
        `,
      };
    });
  }

  getCoverUrl(categoria: Categoria): string | undefined {
    return categoria.immagini?.find((img) => img.isCover)?.url;
  }

  confermaEliminazione(categoria: Categoria) {
    this.mostraModale = true;
    this.categoriaDaEliminare = categoria;
  }

  eliminaCategoriaSeConfermato() {
    if (!this.categoriaDaEliminare) return;

    const id = this.categoriaDaEliminare.id;

    this.categoriaService.delete(id).subscribe({
      next: () => {
        this.categorie = this.categorie.filter((c) => c.id !== id);
        this.categoriaDaEliminare = null;
        this.mostraModale = false;
      },
      error: (err) => {
        console.error('❌ Errore durante l’eliminazione:', err);
        this.errore = 'Errore durante l’eliminazione';
        this.mostraModale = false;
        this.categoriaDaEliminare = null;
      },
    });
  }

  annullaEliminazione() {
    this.mostraModale = false;
    this.categoriaDaEliminare = null;
  }

  vaiAggiungiCategoria(): void {
    this.router.navigate(['/admin/categorie/aggiungi']);
  }

  vaiModificaCategoria(id: number): void {
    this.router.navigate(['/admin/categorie/modifica', id]);
  }
}
