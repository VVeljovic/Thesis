import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UploadImagesComponent } from '../create-accommodation/upload-images/upload-images.component';
import { debounceTime, Observable, switchMap } from 'rxjs';
import { AccommodationService } from '../services/accommodation.service';
import { Accommodation } from '../models/accommodation';
import { Amenity } from '../models/amenity';

@Component({
  selector: 'app-create-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, UploadImagesComponent, FormsModule],
  templateUrl: './create-form.component.html',
  styleUrl: './create-form.component.scss'
})
export class CreateFormComponent {
  bookingForm!: FormGroup;
  selectedBedding: string = 'Bedding';
  showDropdown = false; showCities: boolean = true;
  selectedImages: (string | ArrayBuffer | null)[] = Array(8).fill(null);
  longitude:number=-1;
  latitude:number=-1;
  selectCity(city: string): void {
    this.bookingForm.get('location')?.setValue(city);
    console.log('a');
    this.showDropdown = false;
    this.showCities = false;
  }
  onImagesSelected(images: (string | ArrayBuffer | null)[]): void {
    this.selectedImages = images;
    console.log('Selected images:', this.selectedImages);
  }

  cities: string[] = [];
  constructor(private fb: FormBuilder, private accommodationService: AccommodationService) {
    this.bookingForm = this.fb.group({
      name: ['', Validators.required],
      petsAllowed: [undefined, Validators.required],
      roomService: [undefined, Validators.required],
      pricePerNight: ['', Validators.required],
      location: ['', Validators.required],
      nonSmokingRooms: [undefined, Validators.required],
      spa: [undefined, Validators.required],
      numberOfGuests: ['', Validators.required],
      parking: [undefined, Validators.required],
      arrive: ['', Validators.required],
      depart: ['', Validators.required],
      wifi: [undefined, Validators.required],
      fitnessCentre: [undefined, Validators.required],
      swimmingPool: [undefined, Validators.required],
      airConditioning: [undefined, Validators.required],
      balcony: [undefined, Validators.required],
      television: [undefined, Validators.required],
      comments: ['']
    });
  }

  ngOnInit(): void {
    this.bookingForm.get('location')?.valueChanges.pipe(
      debounceTime(300),
      switchMap(query => this.searchCities(query))
    ).subscribe((data: any) => {
      this.cities = data.results.map((result: any) => result.formatted);
    });
  }
  searchCities(query: string): Observable<any> {
    if (!query) {
      return new Observable();
    }
    return this.accommodationService.searchCities(query);
  }
  onSubmit(): void {
    this.accommodationService.getCoordinates(this.bookingForm.get('location')?.value).subscribe((response)=>{
      console.log(response)
      this.longitude = response.results[0].geometry['lng']
      this.latitude=response.results[0].geometry['lat']
      
    
        const amenity : Amenity = {
          parking: this.bookingForm.get('parking')?.value,
          wifi: this.bookingForm.get('wifi')?.value,
          fitnessCentre: this.bookingForm.get('fitnessCentre')?.value,
          swimmingPool: this.bookingForm.get('swimmingPool')?.value,
          airConditioning: this.bookingForm.get('airConditioning')?.value,
          balcony: this.bookingForm.get('balcony')?.value,
          television: this.bookingForm.get('television')?.value,
          spa: this.bookingForm.get('spa')?.value,
          nonSmokingRooms: this.bookingForm.get('nonSmokingRooms')?.value,
          roomService: this.bookingForm.get('roomService')?.value,
          petsAllowed: this.bookingForm.get('petsAllowed')?.value,

        };
  
        console.log('Amenity object:', amenity);
  
       
        const accommodation : Accommodation= {
          propertyName: this.bookingForm.get('name')?.value,
          pricePerNight: this.bookingForm.get('pricePerNight')?.value,
          address: this.bookingForm.get('location')?.value,
          numberOfGuests: this.bookingForm.get('numberOfGuests')?.value,
          availableFrom: this.bookingForm.get('arrive')?.value,
          availableTo: this.bookingForm.get('depart')?.value,
          description: this.bookingForm.get('comments')?.value,
          photos: this.selectedImages, 
          amenity: amenity, 
          latitude:this.latitude,
          longitude:this.longitude,
          userId:'d00fb6cb-5498-4f78-b6f7-9c52904b6ae5'
        };
        this.accommodationService.createAccomodation(accommodation);
        console.log('Accommodation object:', accommodation);
      
    })
  }
}