import { Routes } from '@angular/router';
import { PublicLayoutComponent } from './layouts/public-layout/public-layout.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { HomeComponent } from './modules/public/pages/home/home.component';

export const appRoutes: Routes = [
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      { path: '', component: HomeComponent },
      {
        path: 'ricette',
        loadChildren: () =>
          import('./modules/public/pages/ricette/ricette.routes').then(
            (m) => m.ricetteRoutes,
          ),
      },
      {
        path: 'blog',
        loadChildren: () =>
          import('./modules/public/pages/blog/blog.routes').then(
            (m) => m.blogRoutes,
          ),
      },
    ],
  },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    children: [
      {
        path: '',
        loadChildren: () =>
          import('./modules/admin/admin.routes').then((m) => m.adminRoutes),
      },
    ],
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./modules/auth/auth.routes').then((m) => m.authRoutes),
  },
  { path: '**', redirectTo: '' },
];
