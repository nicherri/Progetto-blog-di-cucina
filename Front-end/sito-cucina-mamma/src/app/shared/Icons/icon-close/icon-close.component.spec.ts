import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IconCloseComponent } from './icon-close.component';

describe('IconCloseComponent', () => {
  let component: IconCloseComponent;
  let fixture: ComponentFixture<IconCloseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IconCloseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IconCloseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
