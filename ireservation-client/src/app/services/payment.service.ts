import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  constructor(private httpClient: HttpClient) { }
  private apiUrl = 'http://localhost:5225/Payment';
  getCustomerByEmail(email: string): Observable<any> {
    const params = new HttpParams().set('email', email);
    return this.httpClient.get<any>(`${this.apiUrl}/email`, { params });
  }
}

