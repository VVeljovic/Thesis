import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookAccommodationFormComponent } from './book-accommodation-form.component';

describe('BookAccommodationFormComponent', () => {
  let component: BookAccommodationFormComponent;
  let fixture: ComponentFixture<BookAccommodationFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookAccommodationFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookAccommodationFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
