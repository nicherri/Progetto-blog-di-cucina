import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImmagineMetadatiModalComponent } from './immagine-metadati-modal.component';

describe('ImmagineMetadatiModalComponent', () => {
  let component: ImmagineMetadatiModalComponent;
  let fixture: ComponentFixture<ImmagineMetadatiModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImmagineMetadatiModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ImmagineMetadatiModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
