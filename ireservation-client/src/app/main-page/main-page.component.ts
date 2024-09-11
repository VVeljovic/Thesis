import { Component } from '@angular/core';
import { AccommodationService } from '../services/accommodation.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent {
  currLat : number = -1;
  currLong : number = -1;
  accommodations:any;
  constructor(private router:Router,private accommodationService : AccommodationService)
  {
      if(navigator.geolocation)
      {
        navigator.geolocation.getCurrentPosition(position=>{
          this.currLat = position.coords.latitude;
          this.currLong = position.coords.longitude;
          accommodationService.getAccommodations(this.currLong,this.currLat,6,1).subscribe((response)=>{this.accommodations=response
              console.log(response)

          });
        })
      }
  }
  openAccommodation(id: string): void {
    this.router.navigate(['/accommodation', id]);
  }
}
