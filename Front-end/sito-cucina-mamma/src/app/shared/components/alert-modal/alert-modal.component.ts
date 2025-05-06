import { CommonModule } from '@angular/common';
import {
  Component,
  Input,
  Output,
  EventEmitter,
  ElementRef,
  AfterViewInit,
  HostListener,
} from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-alert-modal',
  imports: [CommonModule],
  templateUrl: './alert-modal.component.html',
  styleUrls: ['./alert-modal.component.scss'],
})
export class AlertModalComponent implements AfterViewInit {
  /* ───────────── Inputs ───────────── */
  @Input() title = 'Attenzione';
  @Input() message = 'Qualcosa è andato storto.';
  @Input() type: 'success' | 'error' | 'warning' = 'warning';
  @Input() showConfirm = false;
  /** autoclose (solo success) in ms */
  @Input() autoCloseAfterMs?: number;

  /* ───────────── Outputs ───────────── */
  @Output() close = new EventEmitter<void>();
  @Output() confirm = new EventEmitter<void>();

  constructor(private el: ElementRef) {}

  /* ═════════════ Life-cycle ═════════════ */
  ngOnInit(): void {
    if (this.type === 'success' && this.autoCloseAfterMs) {
      setTimeout(() => this.onClose(), this.autoCloseAfterMs);
    }
  }

  /** sposta il focus sul bottone di chiusura appena il modale compare */
  ngAfterViewInit(): void {
    setTimeout(() => {
      const closeButton = this.el.nativeElement.querySelector(
        '.close-button',
      ) as HTMLButtonElement | null;
      closeButton?.focus();
    });
  }

  /* ═════════════ Keyboard ═════════════ */
  /** chiusura rapida con ESC */
  @HostListener('document:keydown.escape')
  onEsc(): void {
    this.onClose();
  }

  /* ═════════════ Actions ═════════════ */
  onClose(): void {
    this.close.emit();
  }

  onConfirm(): void {
    this.confirm.emit();
  }
}
