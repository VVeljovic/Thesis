import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccommodationService {

  private apiKey = '4f618020d8624275a04534be277ad372';
  private apiKey2 = '0c8f2e1b803645a890e17450f7f1ee86';
  
  constructor(private httpClient: HttpClient) { }

  getAccommodations(longitude:number,latitude:number,pageSize: number, pageNumber: number) {
    return this.httpClient.get(`${environment.api}/Accommodation/get-accommodations/${longitude}/${latitude}/${pageSize}/${pageNumber}`)
  }
  getCoordinates(city: string): Observable<any> {
    const apiUrl = `https://api.opencagedata.com/geocode/v1/json?q=${city}&key=${this.apiKey}`;
    return this.httpClient.get(apiUrl);
  }
  searchCities(query:string):Observable<any>{
    const apiUrl = `https://api.opencagedata.com/geocode/v1/json?q=${query}&key=${this.apiKey2}&limit=5`;
    return this.httpClient.get(apiUrl);
  }
  getMyLocation()
  {
    return this.httpClient.get('https://ipapi.co/json/');
  }
}
