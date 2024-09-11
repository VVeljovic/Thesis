import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UploadImagesComponent } from '../create-accommodation/upload-images/upload-images.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-book-accommodation-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, UploadImagesComponent],
  templateUrl: './book-accommodation-form.component.html',
  styleUrl: './book-accommodation-form.component.scss'
})
export class BookAccommodationFormComponent {
  bookingForm!: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.bookingForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      street: ['', Validators.required],
      streetNumber: ['', Validators.required],
      city: ['', Validators.required],
      postCode: ['', Validators.required],
      country: ['', Validators.required],
      arrive: ['', Validators.required],
      depart: ['', Validators.required],
      people: ['', Validators.required],
      room: ['', Validators.required],
      bedding: ['', Validators.required],
      comments: ['']
    });
  }

  onSubmit(): void {
    {
      console.log(this.bookingForm.value);
    }  {
      console.log('Form is invalid');
    }
  }
}