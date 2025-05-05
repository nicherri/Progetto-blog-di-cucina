import { Component, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './admin-sidebar.component.html',
  styleUrl: './admin-sidebar.component.scss',
})
export class AdminSidebarComponent {
  isSidebarOpen = false;
  menuOpen: { [key: string]: boolean } = {
    ricette: false,
    ingredienti: false,
    categorie: false,
  };

  constructor(private router: Router) {}

  toggleSidebar(event?: Event) {
    if (event) {
      event.stopPropagation(); // 🔥 Evita chiusure accidentali
    }
    if (this.isSidebarOpen) {
      this.isSidebarOpen = !this.isSidebarOpen; // 🔥 Se è aperta si chiude, se è chiusa si apre
      this.resetDropdowns();
    } else {
      this.isSidebarOpen = true;
    }
  }
  resetDropdowns() {
    Object.keys(this.menuOpen).forEach((key) => {
      this.menuOpen[key] = false;
    });
  }

  toggleDropdown(menu: string, event: Event) {
    event.stopPropagation(); // 🔥 Evita che il click chiuda la sidebar

    if (this.isSidebarOpen) {
      this.menuOpen[menu] = !this.menuOpen[menu]; // 🔥 Toggle del menu
    } else {
      this.isSidebarOpen = true; // 🔥 Se la sidebar è chiusa, la apre
    }
  }

  handleClick(route: string, event: Event) {
    event.stopPropagation(); // 🔥 Evita che il click chiuda la sidebar

    if (this.isSidebarOpen) {
      this.router.navigate([route]); // 🔥 Se la sidebar è aperta, naviga
      this.isSidebarOpen = false; // 🔥 Chiude la sidebar dopo la navigazione
    } else {
      this.isSidebarOpen = true; // 🔥 Se la sidebar è chiusa, la apre invece di navigare
    }
  }

  @HostListener('document:click', ['$event'])
  closeSidebar(event: Event) {
    const clickedInside = (event.target as HTMLElement).closest('.sidebar');
    if (!clickedInside && this.isSidebarOpen) {
      this.isSidebarOpen = false;
      this.resetDropdowns();
    }
  }
}
