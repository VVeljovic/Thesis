import { Component } from '@angular/core';
import { AccommodationService } from '../services/accomodation/accommodation.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
} from '@angular/forms';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-location',
  standalone: true,
  imports: [ CommonModule, ReactiveFormsModule],
  templateUrl: './location.component.html',
  styleUrl: './location.component.scss'
})
export class LocationComponent {
  accommodationForm: FormGroup;
  suggestions: string[] = [];
  constructor(
    private accommodationService: AccommodationService,
    private formBuilder: FormBuilder
  ) {
    this.accommodationForm = this.formBuilder.group({
      wareHouse: '',
      location: '',
    });
  }
  selectSuggestion(suggestion: string): void {
    console.log(suggestion);
    this.accommodationForm.get('location')?.setValue(suggestion);

    this.suggestions = [];
  }
  onChange(): void {
    const location = this.accommodationForm.value.location;
    if (location.length > 1)
      this.accommodationService.searchCities(location).subscribe(
        (data: any) => {
          data.results.forEach((element: any) => {
            console.log(element.formatted);
            this.suggestions = data.results.map(
              (element: any) => element.formatted
            );
          });
        },

        (error) => {
          console.error('Greška prilikom HTTP poziva:', error);
          this.suggestions = [];
        }
      );
    else this.suggestions = [];
  }
  getCoordinates(
    location: string
  ): Observable<{ name: string; longitude: number; latitude: number }> {
    return new Observable((observer) => {
      this.accommodationService.getCoordinates(location).subscribe(
        (data: any) => {
          console.log('Odgovor od API-ja:', data);

          if (data.results && data.results.length > 0) {
            const location = data.results[0].geometry;
            const center = {
              lat: location.lat,
              lng: location.lng,
            };
            console.log('Koordinate grada:', center);
            const result = {
              name: location,
              longitude: center.lng,
              latitude: center.lat,
            };
            observer.next(result);
          } else {
            console.error('Nije moguće dobiti koordinate za uneseni grad.');
          }
        },
        (error) => {
          console.error('Greška prilikom HTTP poziva:', error);
        },
        () => {
          observer.complete();
        }
      );
    });
  }
  async onSubmit() {
    console.log(this.accommodationForm.value.location);
    this.getCoordinates(this.accommodationForm.value.location).subscribe((respo)=>console.log(respo.latitude))
  }
  }
