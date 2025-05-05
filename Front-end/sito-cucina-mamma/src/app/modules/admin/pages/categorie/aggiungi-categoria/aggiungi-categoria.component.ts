import { IconWarningComponent } from './../../../../../shared/Icons/icon-warning/icon-warning.component';
import { Component, AfterViewInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CategorieService } from '../../../../../core/services/categorie.service';
import { ImmagineService } from '../../../../../core/services/immagine.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Immagine } from '../../../../../core/models/immagine';
import { GestioneImmaginiComponent } from '../../../../../shared/components/gestione-immagini/gestione-immagini.component';
import { ImmagineForm } from '../../../../../core/models/ImmagineForm';
import { AlertModalService } from '../../../../../core/services/alert-modal.service';

@Component({
  selector: 'app-aggiungi-categoria',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GestioneImmaginiComponent,
    IconWarningComponent,
  ],
  templateUrl: './aggiungi-categoria.component.html',
  styleUrls: ['./aggiungi-categoria.component.scss'],
})
export class AggiungiCategoriaComponent implements AfterViewInit {
  categoriaForm: FormGroup;
  showImmagineModal = false;
  immagineSelezionata?: Immagine;
  immagini: ImmagineForm[] = [];
  slugModificatoDallUtente = false;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private categoriaService: CategorieService,
    private immagineService: ImmagineService,
    private router: Router,
    private alertModalService: AlertModalService,
  ) {
    this.categoriaForm = this.fb.group({
      nome: ['', [Validators.required, Validators.maxLength(50)]],
      descrizione: ['', Validators.required],
      slug: ['', [Validators.required, Validators.maxLength(100)]],
      seoTitle: ['', [Validators.required, Validators.maxLength(70)]],
      seoDescription: ['', [Validators.required, Validators.maxLength(160)]],
    });

    // Slug autogenerato live
    this.categoriaForm.get('nome')?.valueChanges.subscribe((val: string) => {
      const slug = val
        .toLowerCase()
        .trim()
        .replace(/[^\w\s-]/g, '')
        .replace(/\s+/g, '-');
      this.categoriaForm.get('slug')?.setValue(slug, { emitEvent: false });
    });
  }

  get categoriaNonValidaOImmaginiIncomplete(): boolean {
    return (
      this.submitted &&
      (this.categoriaForm.invalid ||
        this.immagini.some((img) => !img.alt?.trim() || !img.title?.trim()))
    );
  }
  ngAfterViewInit() {
    this.categoriaForm.get('slug')?.valueChanges.subscribe(() => {
      this.slugModificatoDallUtente = true;
    });
  }

  apriGestioneImmagineModal() {
    this.showImmagineModal = true;
  }

  chiudiGestioneImmagineModal() {
    this.showImmagineModal = false;
  }

  salvaImmagine(immagine: Immagine) {
    this.immagineSelezionata = immagine;
    this.showImmagineModal = false;
  }

  private scrollToFirstError(): void {
    setTimeout(() => {
      const firstError = document.querySelector(
        'input.ng-invalid, textarea.ng-invalid, select.ng-invalid',
      );
      if (firstError) {
        firstError.scrollIntoView({ behavior: 'smooth', block: 'center' });
        (firstError as HTMLElement).focus();
      }
    });
  }

  submit(): void {
    this.submitted = true;
    this.categoriaForm.markAllAsTouched();
    this.categoriaForm.updateValueAndValidity();

    // Valida metadati immagini
    this.immagini = this.immagini.map((img) => ({
      ...img,
      warning: !img.alt?.trim() || !img.title?.trim(),
    }));

    const immaginiConErrori = this.immagini.some((img) => img.warning);

    this.scrollToFirstError();

    if (this.categoriaForm.invalid || immaginiConErrori) {
      this.alertModalService.showWarning(
        'Campi obbligatori mancanti',
        'Controlla i campi del form e i metadati delle immagini.',
      );
      return;
    }

    // Prepara il form data
    const formDataCategoria = new FormData();
    formDataCategoria.append('nome', this.categoriaForm.value.nome);
    formDataCategoria.append(
      'descrizione',
      this.categoriaForm.value.descrizione,
    );
    formDataCategoria.append('slug', this.categoriaForm.value.slug);
    formDataCategoria.append('seoTitle', this.categoriaForm.value.seoTitle);
    formDataCategoria.append(
      'seoDescription',
      this.categoriaForm.value.seoDescription,
    );

    // Salva la categoria
    this.categoriaService.create(formDataCategoria).subscribe({
      next: (categoria) => {
        this.alertModalService.showSuccess(
          'Categoria creata!',
          'La categoria è stata aggiunta correttamente.',
        );

        if (this.immagini.length > 0) {
          const formData = new FormData();
          this.immagini.forEach((img) => {
            formData.append('files', img.file);
          });

          const metadati = this.immagini.map((img) => ({
            alt: img.alt,
            nomeFileSeo: img.nomeFileSeo,
            title: img.title,
            caption: img.caption,
            ordine: img.ordine,
            isCover: img.isCover,
          }));

          formData.append(
            'metadati',
            new Blob([JSON.stringify(metadati)], { type: 'application/json' }),
          );

          this.immagineService
            .upload('categoria', categoria.id, formData)
            .subscribe({
              next: () => this.router.navigate(['/admin/categorie']),
              error: (err) => {
                this.alertModalService.handleHttpError(
                  err,
                  'Errore durante il caricamento delle immagini',
                );
              },
            });
        } else {
          this.router.navigate(['/admin/categorie']);
        }
      },
      error: (err) => {
        this.alertModalService.handleHttpError(
          err,
          'Errore durante la creazione della categoria. Riprova.',
        );
      },
    });
  }
}
