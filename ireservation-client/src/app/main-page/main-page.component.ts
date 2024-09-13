import { Component } from '@angular/core';
import { AccommodationService } from '../services/accommodation.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { KeycloakService } from '../services/keycloak.service';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { PaginationComponent } from '../pagination/pagination.component';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent, PaginationComponent],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent {
  currLat: number = -1;
  currLong: number = -1;
  accommodations: any;
  checkInDate: string = '';
  checkOutDate: string = '';
  numberOfGuests: number = 0;
  destination: string = '';
  pageSize: number = 3;
  currentPage: number = 1;
  suggestions: string[] = [];

  constructor(private router: Router, private accommodationService: AccommodationService, private keycloakService: KeycloakService) {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(position => {
        this.currLat = position.coords.latitude;
        this.currLong = position.coords.longitude;
        accommodationService.getAccommodations(this.currLong, this.currLat, 1, this.currentPage).subscribe((response) => {
          this.accommodations = response
          console.log(response)

        });
      })
    }
  }
  onChange(): void {
    const location = this.destination;
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
  selectSuggestion(suggestion: string): void {
    console.log(suggestion);
    this.destination = suggestion;

    this.suggestions = [];
  }
  onPageChange(pageNumber: number) {
    this.currentPage = pageNumber;
    console.log(`Trenutna stranica: ${this.currentPage}`);
    this.search()
  }
  search() {
    console.log('Check-in:', this.checkInDate);
    console.log('Check-out:', this.checkOutDate);
    console.log('Number of guests:', this.numberOfGuests);
    console.log('Destination:', this.destination);
    this.accommodationService.getAccommodations(
      this.currLong,
      this.currLat,
      1,
      this.currentPage,
      this.destination,
      this.checkInDate,
      this.checkOutDate,
      this.numberOfGuests
    ).subscribe((response) => {
      this.accommodations = response;
      console.log(response);
    })
  }
  openAccommodation(id: string): void {
    this.router.navigate(['/accommodation', id]);
  }
}
