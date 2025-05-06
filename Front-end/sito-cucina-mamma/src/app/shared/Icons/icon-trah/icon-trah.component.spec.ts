import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IconTrahComponent } from './icon-trah.component';

describe('IconTrahComponent', () => {
  let component: IconTrahComponent;
  let fixture: ComponentFixture<IconTrahComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IconTrahComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IconTrahComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
