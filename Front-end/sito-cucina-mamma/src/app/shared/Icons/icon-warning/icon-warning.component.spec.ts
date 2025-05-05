import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IconWarningComponent } from './icon-warning.component';

describe('IconWarningComponent', () => {
  let component: IconWarningComponent;
  let fixture: ComponentFixture<IconWarningComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IconWarningComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IconWarningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
