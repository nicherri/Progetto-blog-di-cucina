import { Component, EventEmitter, Output, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImmagineForm } from '../../../core/models//ImmagineForm';
import { FormsModule } from '@angular/forms';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';

import { IconCloseComponent } from '../../Icons/icon-close/icon-close.component';
import { IconWarningComponent } from '../../Icons/icon-warning/icon-warning.component';

@Component({
  selector: 'app-immagine-metadati-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    IconCloseComponent,
    IconWarningComponent,
  ],
  templateUrl: './immagine-metadati-modal.component.html',
  styleUrls: ['./immagine-metadati-modal.component.scss'],
})
export class ImmagineMetadatiModalComponent {
  @Input() immagine?: ImmagineForm;
  @Output() salva = new EventEmitter<ImmagineForm>();
  @Output() chiudi = new EventEmitter<void>();

  form!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.form = this.fb.group({
      alt: [this.immagine?.alt || '', Validators.required],
      title: [this.immagine?.title || '', Validators.required],
      caption: [this.immagine?.caption || '', Validators.required],
      nomeFileSeo: [this.immagine?.nomeFileSeo || '', Validators.required],
      ordine: [this.immagine?.ordine ?? 0, Validators.required],
      isCover: [this.immagine?.isCover ?? false],
    });
  }

  onSubmit() {
    if (!this.form.valid) {
      this.form.markAllAsTouched(); // <- fondamentale per attivare le classi
      return;
    }

    if (this.immagine) {
      const dati = { ...this.immagine, ...this.form.value };
      this.salva.emit(dati);
    }
  }

  onChiudi() {
    this.chiudi.emit();
  }
}
