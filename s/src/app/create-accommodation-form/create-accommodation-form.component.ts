import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AccommodationModel } from '../models/keycloak/accomodation.model';
import { AccommodationService } from '../services/accomodation/accommodation.service';

@Component({
  selector: 'app-create-accommodation-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-accommodation-form.component.html',
  styleUrl: './create-accommodation-form.component.css'
})
export class CreateAccommodationFormComponent implements OnInit {
  accommodationForm!: FormGroup;
  selectedFiles: FileList | undefined;
  filesBase64: string [] = [];
  onFileSelected(event:any):void{
    const files = event.target.files;
    console.log('upao');
    for(let i = 0 ; i < files.length; i ++ )
    {
      const file = files[i];
      const reader = new FileReader();

      reader.onload = ( e : any)=>{
        const base64String = e.target.result.split(',')[1];
        this.filesBase64.push(base64String);
        console.log(this.filesBase64.length)
      }
      reader.readAsDataURL(file);
      console.log(this.filesBase64.length)
    }
  }
  constructor(private fb: FormBuilder, private accommodationService: AccommodationService) {}
  preventScroll(event: Event): void {
    event.preventDefault();
  }
  ngOnInit(): void {
    this.accommodationForm = this.fb.group({
      propertyName: ['', Validators.required],
      description: ['', Validators.required],
      address: ['', Validators.required],
      pricePerNight: ['', [Validators.required, Validators.min(0)]],
      availableFrom: ['', Validators.required],
      availableTo: ['', Validators.required],
      userId: ['', Validators.required],
      photos: [null] 
    });
  }

  onSubmit(): void {
    if (this.accommodationForm?.valid) {
      let formData: AccommodationModel = this.accommodationForm?.value;
      formData.photos = this.filesBase64;
      console.log('Form Data:', formData);
     
    }
  }

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.selectedFiles = input.files;
     
    }
  }
  handleFileUpload() {
    console.log('a');const formData: AccommodationModel = this.accommodationForm?.value;
    formData.photos = this.filesBase64;
    console.log(formData);
    this.accommodationService.createAccomodation(formData);
  }
}
