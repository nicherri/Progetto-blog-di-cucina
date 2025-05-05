import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GestioneImmaginiComponent } from './gestione-immagini.component';

describe('GestioneImmaginiComponent', () => {
  let component: GestioneImmaginiComponent;
  let fixture: ComponentFixture<GestioneImmaginiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GestioneImmaginiComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GestioneImmaginiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
