// ✅ elenco-categorie.component.ts aggiornato

import { Component, OnInit } from '@angular/core';
import { CategorieService } from '../../../../../core/services/categorie.service';
import { Categoria } from '../../../../../core/models/categoria';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { trigger, transition, style, animate } from '@angular/animations';
import { ConfermaDialogComponent } from '../../../../../shared/components/conferma-dialog/conferma-dialog.component';

@Component({
  selector: 'app-elenco-categorie',
  standalone: true,
  imports: [CommonModule, RouterModule, ConfermaDialogComponent],
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

  constructor(
    private categoriaService: CategorieService,
    private router: Router, // ⬅️ DEVE ESSERE INIETTATO QUI!
  ) {
    console.log('🚀 Router:', this.router);
  }

  ngOnInit(): void {
    this.caricaCategorie();
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
        console.log(`✅ Categoria con ID ${id} eliminata con successo.`);
        // 🔄 Rimuovi la categoria dalla lista senza ricaricare tutto
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
    console.log('Navigazione verso aggiunta categoria');
    this.router.navigate(['/admin/categorie/aggiungi']);
  }

  vaiModificaCategoria(id: number): void {
    console.log('Navigazione verso modifica categoria', id);
    this.router.navigate(['/admin/categorie/modifica', id]);
  }
}
