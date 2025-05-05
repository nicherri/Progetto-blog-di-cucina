import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-conferma-dialog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './conferma-dialog.component.html',
  styleUrls: ['./conferma-dialog.component.scss'],
})
export class ConfermaDialogComponent {
  @Input() messaggio = 'Sei sicuro di voler procedere?';
  @Output() confermato = new EventEmitter<void>();
  @Output() annullato = new EventEmitter<void>();

  conferma() {
    this.confermato.emit();
  }

  annulla() {
    this.annullato.emit();
  }
}
