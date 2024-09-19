import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Booking } from '../models/booking';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  constructor(private httpClient: HttpClient) { }

  public bookAccommodation (booking : Booking)
  {
    return this.httpClient.post(`http://localhost:5152/Transaction`,booking);

  }
}
