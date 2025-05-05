// core/services/alert-modal.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface AlertModalData {
  title: string;
  message: string;
  type: 'success' | 'error' | 'warning';
  showConfirm?: boolean;
  onConfirm?: () => void;
}

@Injectable({ providedIn: 'root' })
export class AlertModalService {
  private alertSubject = new BehaviorSubject<AlertModalData | null>(null);
  alert$ = this.alertSubject.asObservable();

  showAlert(data: AlertModalData) {
    this.alertSubject.next(data);
  }

  closeAlert() {
    this.alertSubject.next(null);
  }

  // ✅ Successo semplice
  showSuccess(title: string, message: string) {
    this.showAlert({ title, message, type: 'success' });
  }

  // ❌ Errore semplice
  showError(title: string, message: string) {
    this.showAlert({ title, message, type: 'error' });
  }

  // ⚠️ Avviso semplice
  showWarning(title: string, message: string) {
    this.showAlert({ title, message, type: 'warning' });
  }

  // ❓ Conferma con azione
  showConfirmation(title: string, message: string, onConfirm: () => void) {
    this.showAlert({
      title,
      message,
      type: 'warning',
      showConfirm: true,
      onConfirm,
    });
  }

  handleHttpError(
    error: any,
    fallbackMessage = 'Si è verificato un errore inatteso',
  ): void {
    const message = error?.error?.message || error?.message || fallbackMessage;

    this.showAlert({
      type: 'error',
      title: 'Errore',
      message,
      showConfirm: false,
    });
  }
}
