import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeoFieldsComponent } from './seo-fields.component';

describe('SeoFieldsComponent', () => {
  let component: SeoFieldsComponent;
  let fixture: ComponentFixture<SeoFieldsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SeoFieldsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SeoFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
