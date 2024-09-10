import { Component } from '@angular/core';
import { PaginationComponent } from '../pagination/pagination.component';
import { AccommodationModel } from '../models/keycloak/accomodation.model';
import { Observable } from 'rxjs';
import { AccommodationService } from '../services/accomodation/accommodation.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-front-page',
  standalone: true,
  imports: [PaginationComponent, CommonModule],
  templateUrl: './front-page.component.html',
  styleUrl: './front-page.component.scss'
})
export class FrontPageComponent {
  accommodationList :any ; 
constructor (private accommodationService : AccommodationService)
{
  this.accommodationService.getAccommodations(10,0).subscribe((response)=>{
    this.accommodationList = response; 
    console.log(response);
  })
  this.accommodationService.getMyLocation().subscribe((response)=>console.log(response));
  this.accommodationService.getCoordinates("Belgrade").subscribe((response)=>console.log(response));
}
}
