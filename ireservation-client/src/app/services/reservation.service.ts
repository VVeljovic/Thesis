import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { GetReservation } from '../models/getreservation';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private baseUrl = 'http://localhost:5243/Reservation';
  constructor(private httpClient : HttpClient) { }
  getReservations(reservationModel: GetReservation) {
    let params: any = {};

    if (reservationModel.userId) {
      params.UserId = reservationModel.userId;
    }
    if (reservationModel.pageSize && reservationModel.pageSize > 0) {
      params.PageSize = reservationModel.pageSize;
    }
    if (reservationModel.pageNumber && reservationModel.pageNumber > 0) {
      params.PageNumber = reservationModel.pageNumber;
    }
    if (reservationModel.status) {
      params.Status = reservationModel.status;
    }

    return this.httpClient.get(`${this.baseUrl}`, { params });
  }
}
