import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IconDotsComponent } from './icon-dots.component';

describe('IconDotsComponent', () => {
  let component: IconDotsComponent;
  let fixture: ComponentFixture<IconDotsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IconDotsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IconDotsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
