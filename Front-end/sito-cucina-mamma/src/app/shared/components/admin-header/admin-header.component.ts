import { Component, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-header',
  standalone: true, // ✅ Questo lo rende standalone
  imports: [CommonModule],
  templateUrl: './admin-header.component.html',
  styleUrl: './admin-header.component.scss',
})
export class AdminHeaderComponent {
  isProfileMenuOpen = false;
  username: string | null = null; // 🔹 Di default nessun nome

  constructor(private router: Router) {}

  toggleProfileMenu() {
    this.isProfileMenuOpen = !this.isProfileMenuOpen;
  }

  goToSettings() {
    this.router.navigate(['/admin/settings']);
    this.isProfileMenuOpen = false;
  }

  logout() {
    console.log('Utente disconnesso');
    this.isProfileMenuOpen = false;
  }
  loginUser(name: string) {
    this.username = name; // 🔥 Imposta nome utente al login
  }

  @HostListener('document:click', ['$event'])
  closeProfileMenu(event: Event) {
    const clickedInside = (event.target as HTMLElement).closest(
      '.profile-container',
    );
    if (!clickedInside && this.isProfileMenuOpen) {
      this.isProfileMenuOpen = false;
    }
  }
}
