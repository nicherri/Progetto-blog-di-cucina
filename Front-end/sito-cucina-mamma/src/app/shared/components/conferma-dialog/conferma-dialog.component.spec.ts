import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfermaDialogComponent } from './conferma-dialog.component';

describe('ConfermaDialogComponent', () => {
  let component: ConfermaDialogComponent;
  let fixture: ComponentFixture<ConfermaDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfermaDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfermaDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
