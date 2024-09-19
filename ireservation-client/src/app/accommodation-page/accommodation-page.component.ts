import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccommodationService } from '../services/accommodation.service';
import { NavbarComponent } from '../navbar/navbar.component';
import { ReviewComponentComponent } from '../review-component/review-component.component';

@Component({
  selector: 'app-accommodation-page',
  standalone: true,
  imports: [CommonModule, NavbarComponent, ReviewComponentComponent],
  templateUrl: './accommodation-page.component.html',
  styleUrl: './accommodation-page.component.css'
})
export class AccommodationPageComponent {
  id: string ='';
  accommodation :any;
  accommodationId: string = '';
  private amenityLabels: { [key: string]: string } = {
    airConditioning: 'Air Conditioning',
    balcony: 'Balcony',
    fitnessCentre: 'Fitness Centre',
    nonSmokingRooms: 'Non-Smoking Rooms',
    parking: 'Parking',
    petsAllowed: 'Pets Allowed',
    roomService: 'Room Service',
    spa: 'Spa',
    swimmingPool: 'Swimming Pool',
    television: 'Television',
    wiFi: 'WiFi'
  };
  getFalseAmenities(): string[] {
    if (!this.accommodation?.amenity) return [];
    return Object.keys(this.accommodation.amenity)
      .filter(key => !this.accommodation.amenity[key])
      .map(key => this.amenityLabels[key] || key); 
  }
  constructor(private route: ActivatedRoute, private accommodationService:AccommodationService, private router: Router) {
    this.id = this.route.snapshot.paramMap.get('id') ?? '';
    
   this.accommodationService.getAccommodation(this.id).subscribe((response)=>{console.log(response)

    this.accommodation = response;
    this.accommodationId = this.accommodation.id;
   })
  }
  navigate()
  {
    console.log(this.accommodation)
    this.router.navigate([`reservation/${this.id}`],{queryParams : { accommodation: JSON.stringify(this.accommodation)}});
  }
  ngOnInit(): void {

  }
}
