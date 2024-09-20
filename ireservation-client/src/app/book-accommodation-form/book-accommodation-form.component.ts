import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UploadImagesComponent } from '../create-accommodation/upload-images/upload-images.component';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';
import { ActivatedRoute } from '@angular/router';
import { Booking } from '../models/booking';
import { UserProfile } from '../models/userprofile';
import { MatDialog } from '@angular/material/dialog';
import { CreditCardComponent } from '../credit-card/credit-card.component';
import { SignalrService } from '../services/signalr.service';
import { NotificationPopupComponent } from '../notification-popup/notification-popup.component';

@Component({
  selector: 'app-book-accommodation-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, UploadImagesComponent, NavbarComponent, FormsModule],
  templateUrl: './book-accommodation-form.component.html',
  styleUrl: './book-accommodation-form.component.css'
})
export class BookAccommodationFormComponent implements OnInit{
  accommodation: any;
  obj =   JSON.parse(localStorage.getItem('userInfo') || '{}');
  numberOfGuests : number=0 ;
  checkInDate:string='';
  checkOutDate:string='';
  totalAmount : number = 0;
  receivedMessage: string='';
  minDate = '';
  maxDate = '';
  totalAmountDisplay = '';
  constructor(private route: ActivatedRoute, private dialog:MatDialog, private signalRService : SignalrService) {
    this.route.queryParams.subscribe(params => {
      const accommodationParam = params["accommodation"];
      if (accommodationParam) {
        this.accommodation = JSON.parse(accommodationParam); console.log(this.accommodation)
        this.minDate = this.accommodation.availableFrom.split('T')[0];
        this.maxDate = this.accommodation.availableTo.split('T')[0];
      }
    });
  }
  ngOnInit(): void {
    this.signalRService.startConnection().subscribe(() => {
      this.signalRService.receiveMessage().subscribe((message) => {
        this.receivedMessage = message;
        console.log(this.receivedMessage)
        var messsage = {
          title : "Booking status",
          text : this.receivedMessage
        }
        if(this.receivedMessage!='' && this.receivedMessage!='Connected')
        this.dialog.open(NotificationPopupComponent, {

          data: messsage
        });
      });
    });
  }
  newTotalAmount()
  {
    if(this.checkInDate!='' && this.checkOutDate !='')
    {
      console.log('a')
    const checkIn = new Date(this.checkInDate);
    const checkOut = new Date(this.checkOutDate);
    
    const timeDiff = checkOut.getTime() - checkIn.getTime();
    
    const numberOfNights = Math.ceil(timeDiff / (1000 * 3600 * 24));
    
     this.totalAmount = this.accommodation.pricePerNight * numberOfNights;
     this.totalAmountDisplay = this.totalAmount +"$";
    }
  }
  numberOfGuestChanged()
  {
    //this.totalAmount = this.accommodation.pricePerNight * this.numberOfGuests;
  }
  onSubmit() {

    this.obj =   JSON.parse(localStorage.getItem('userInfo') || '{}');
    const userId = this.obj.sub;
    const now = new Date();
    const dateCreated = new Date(now.getTime() + 2 * 60 * 60 * 1000);
    const bookingObj: Booking = {
      date: dateCreated.toISOString(),
      totalAmount : this.totalAmount,
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