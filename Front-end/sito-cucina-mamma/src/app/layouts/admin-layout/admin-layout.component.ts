import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AdminHeaderComponent } from '../../shared/components/admin-header/admin-header.component';
import { AdminSidebarComponent } from '../admin-sidebar/admin-sidebar.component';
import { AlertModalComponent } from '../../shared/components/alert-modal/alert-modal.component';
import {
  AlertModalService,
  AlertModalData,
} from '../../core/services/alert-modal.service';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    AdminHeaderComponent,
    AdminSidebarComponent,
    AlertModalComponent, // 👈 IMPORTA IL MODALE
  ],
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss'],
})
export class AdminLayoutComponent {
  alert: AlertModalData | null = null;
  isSidebarOpen = false; // ✅ Definito correttamente

  constructor(private alertService: AlertModalService) {
    // ✅ Qui sottoscrivi il servizio per mostrare il modale!
    this.alertService.alert$.subscribe((data) => {
      this.alert = data;
    });
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  onClose() {
    this.alert = null;
  }

  onConfirm() {
    if (this.alert?.onConfirm) {
      this.alert.onConfirm();
    }
    this.alert = null;
  }
}
