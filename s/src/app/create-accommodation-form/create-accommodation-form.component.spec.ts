import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAccommodationFormComponent } from './create-accommodation-form.component';

describe('CreateAccommodationFormComponent', () => {
  let component: CreateAccommodationFormComponent;
  let fixture: ComponentFixture<CreateAccommodationFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateAccommodationFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateAccommodationFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
