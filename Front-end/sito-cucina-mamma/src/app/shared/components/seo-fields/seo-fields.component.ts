// 📁 shared/components/seo-fields/seo-fields.component.ts
import { Component, Input, OnInit } from '@angular/core';
import {
  FormGroup,
  AbstractControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { IconWarningComponent } from '../../../shared/Icons/icon-warning/icon-warning.component';

@Component({
  standalone: true,
  selector: 'app-seo-fields',
  imports: [CommonModule, ReactiveFormsModule, IconWarningComponent],
  templateUrl: './seo-fields.component.html',
  styleUrls: ['./seo-fields.component.scss'],
})
export class SeoFieldsComponent implements OnInit {
  @Input() formGroup!: FormGroup;
  @Input() autoGenerateSlugFrom?: AbstractControl | null;
  @Input() submitted = false;

  slugTouchedManually = false;
  seoTitleTouchedManually = false;
  seoDescriptionTouchedManually = false;

  ngOnInit(): void {
    const nomeCtrl = this.autoGenerateSlugFrom;
    const descrizioneCtrl = this.formGroup.get('descrizione');

    // ✅ 1. Inizializzazione immediata se già compilati
    const nomeVal = nomeCtrl?.value;
    if (nomeVal) {
      if (!this.slugTouchedManually) {
        const slug = nomeVal
          .toLowerCase()
          .trim()
          .replace(/[\W_]+/g, '-')
          .replace(/^-+|-+$/g, '');
        this.formGroup.get('slug')?.patchValue(slug);
      }

      if (!this.seoTitleTouchedManually) {
        this.formGroup.get('seoTitle')?.patchValue(nomeVal.slice(0, 70));
      }
    }

    const descrizioneVal = descrizioneCtrl?.value;
    if (descrizioneVal && !this.seoDescriptionTouchedManually) {
      this.formGroup
        .get('seoDescription')
        ?.patchValue(descrizioneVal.slice(0, 160));
    }

    // ✅ 2. Live: Nome → slug e titolo SEO
    nomeCtrl?.valueChanges.subscribe((val: string) => {
      if (!this.slugTouchedManually) {
        const slug = val
          .toLowerCase()
          .trim()
          .replace(/[\W_]+/g, '-')
          .replace(/^-+|-+$/g, '');
        this.formGroup.get('slug')?.patchValue(slug);
      }

      if (!this.seoTitleTouchedManually) {
        this.formGroup.get('seoTitle')?.patchValue(val.slice(0, 70));
      }
    });

    // ✅ 3. Live: Descrizione → SEO Description
    descrizioneCtrl?.valueChanges.subscribe((val: string) => {
      if (!this.seoDescriptionTouchedManually && val) {
        this.formGroup.get('seoDescription')?.patchValue(val.slice(0, 160));
      }
    });

    // ✅ 4. Hard cap max caratteri
    this.formGroup.get('seoTitle')?.valueChanges.subscribe((val: string) => {
      if (val?.length > 70) {
        this.formGroup.get('seoTitle')?.patchValue(val.slice(0, 70));
      }
    });

    this.formGroup
      .get('seoDescription')
      ?.valueChanges.subscribe((val: string) => {
        if (val?.length > 160) {
          this.formGroup.get('seoDescription')?.patchValue(val.slice(0, 160));
        }
      });
  }

  markSlugTouched() {
    this.slugTouchedManually = true;
  }

  markSeoTitleTouched() {
    this.seoTitleTouchedManually = true;
  }

  markSeoDescriptionTouched() {
    this.seoDescriptionTouchedManually = true;
  }
}
