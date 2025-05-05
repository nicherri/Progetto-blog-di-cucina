import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminHeaderComponent } from './components/admin-header/admin-header.component';
import { PublicHeaderComponent } from './components/public-header/public-header.component';
import { FooterComponent } from './components/footer/footer.component';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FooterComponent,
    PublicHeaderComponent,
    AdminHeaderComponent,
  ],
  exports: [AdminHeaderComponent, PublicHeaderComponent, FooterComponent],
})
export class SharedModule {}
