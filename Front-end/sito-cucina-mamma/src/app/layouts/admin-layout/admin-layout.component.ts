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
  ],
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss'],
})
export class AdminLayoutComponent {
  isSidebarOpen = false; // ✅ Definito correttamente

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }
}
