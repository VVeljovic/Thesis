import { Component } from '@angular/core';
import { AccommodationService } from '../services/accommodation.service';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent {
  currLat : number = -1;
  currLong : number = -1;
  constructor(private accommodationService : AccommodationService)
  {
      if(navigator.geolocation)
      {
        navigator.geolocation.getCurrentPosition(position=>{
          this.currLat = position.coords.latitude;
          this.currLong = position.coords.longitude;
        })
          accommodationService.getAccommodations(this.currLong,this.currLat,9,0);
      }
  }
}
