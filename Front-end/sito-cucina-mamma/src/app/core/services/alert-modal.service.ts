// core/services/alert-modal.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

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

  private queue: AlertModalData[] = [];
  private isShowing = false;

  showAlert(data: AlertModalData) {
    this.queue.push(data);
    this.tryShowNext();
  }

  private tryShowNext() {
    if (this.isShowing || this.queue.length === 0) return;

    const nextAlert = this.queue.shift()!;
    this.alertSubject.next(nextAlert);
    this.isShowing = true;
  }

  closeAlert() {
    this.alertSubject.next(null);
    this.isShowing = false;

    // Mostra il prossimo dopo breve delay (per animazione uscita)
    setTimeout(() => this.tryShowNext(), 350);
  }

  // ✅ Successo semplice
  showSuccess(title: string, message: string, autoCloseMs = 3000) {
    this.showAlert({ title, message, type: 'success' });

    setTimeout(() => {
      this.closeAlert();
    }, autoCloseMs);
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
