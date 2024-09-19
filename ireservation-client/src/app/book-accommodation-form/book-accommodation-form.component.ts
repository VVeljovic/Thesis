import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UploadImagesComponent } from '../create-accommodation/upload-images/upload-images.component';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';
import { ActivatedRoute } from '@angular/router';
import { Booking } from '../models/booking';
import { UserProfile } from '../models/userprofile';
import { MatDialog } from '@angular/material/dialog';
import { CreditCardComponent } from '../credit-card/credit-card.component';

@Component({
  selector: 'app-book-accommodation-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, UploadImagesComponent, NavbarComponent, FormsModule],
  templateUrl: './book-accommodation-form.component.html',
  styleUrl: './book-accommodation-form.component.css'
})
export class BookAccommodationFormComponent {
  accommodation: any;
  obj: any;
  numberOfGuests : number=0 ;
  checkInDate!:string;
  checkOutDate!:string;
  totalAmount : number = 0;
  constructor(private route: ActivatedRoute, private dialog:MatDialog) {
    this.route.queryParams.subscribe(params => {
      const accommodationParam = params["accommodation"];
      if (accommodationParam) {
        this.accommodation = JSON.parse(accommodationParam); console.log(this.accommodation)
      }
    });
  }
  numberOfGuestChanged()
  {
    this.totalAmount = this.accommodation.pricePerNight * this.numberOfGuests;
  }
  onSubmit() {
    console.log(this.checkInDate)
    console.log(this.checkOutDate)
    this.obj =   JSON.parse(localStorage.getItem('userInfo') || '{}');
    const userId = this.obj.sub;
    const now = new Date();
    const dateCreated = new Date(now.getTime() + 2 * 60 * 60 * 1000);
    const bookingObj: Booking = {
      date: dateCreated.toISOString(),
      totalAmount : this.accommodation.pricePerNight * 10,
      status : "Pending",
      userId : userId,
      accommodationId : this.accommodation.id,
      type : "booking",
      dateFrom : this.checkInDate,
      dateTo : this.checkOutDate
    }
    const dialogRef = this.dialog.open(CreditCardComponent, {
      
      width: '400px',
      data: { booking: bookingObj }
    });

    
    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      console.log(result);  
  })}
}