import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  constructor(private httpClient: HttpClient) { }
  private apiUrl = 'http://localhost:5225/Payment/get-customer';
  getCustomerByEmail(email: string): Observable<any> {
   
    return this.httpClient.get(`${this.apiUrl}/${email}`);
  }
}

