import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-alert-modal',
  imports: [CommonModule],
  templateUrl: './alert-modal.component.html',
  styleUrls: ['./alert-modal.component.scss'],
})
export class AlertModalComponent {
  @Input() title: string = 'Attenzione';
  @Input() message: string = 'Qualcosa è andato storto.';
  @Input() type: 'success' | 'error' | 'warning' = 'warning';
  @Input() showConfirm: boolean = false;
  @Input() autoCloseAfterMs?: number = undefined; // ⏱️ opzionale

  @Output() close = new EventEmitter<void>();
  @Output() confirm = new EventEmitter<void>();

  ngOnInit() {
    if (this.type === 'success' && this.autoCloseAfterMs) {
      setTimeout(() => {
        this.onClose();
      }, this.autoCloseAfterMs);
    }
  }

  onClose() {
    this.close.emit();
  }

  onConfirm() {
    this.confirm.emit();
  }
}
