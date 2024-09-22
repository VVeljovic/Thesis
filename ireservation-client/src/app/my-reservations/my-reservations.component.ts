import { Component } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { ReservationService } from '../services/reservation.service';
import { GetReservation } from '../models/getreservation';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-my-reservations',
  standalone: true,
  imports: [NavbarComponent, CommonModule],
  templateUrl: './my-reservations.component.html',
  styleUrl: './my-reservations.component.css'
})
export class MyReservationsComponent {
  obj =   JSON.parse(localStorage.getItem('userInfo') || '{}'); reservations: any;
  constructor(private reservationService:ReservationService)
  {
    const reservationModel: GetReservation ={
      userId:this.obj.sub,
      pageNumber : 1,
      pageSize : 10,
      status : "Success"
    }
    this.reservationService.getReservations(reservationModel).subscribe((response)=>{
     this.reservations = response;
    })
  }
}
