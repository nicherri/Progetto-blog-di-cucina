import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

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

  @Output() close = new EventEmitter<void>();
  @Output() confirm = new EventEmitter<void>();

  onClose() {
    this.close.emit();
    console.log('Chiusura del modal');
  }

  onConfirm() {
    this.confirm.emit();
  }
}
