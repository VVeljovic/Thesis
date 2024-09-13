import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Accommodation } from '../models/accommodation';

@Injectable({
  providedIn: 'root'
})
export class AccommodationService {

  private apiKey = '4f618020d8624275a04534be277ad372';
  private apiKey2 = '0c8f2e1b803645a890e17450f7f1ee86';
  
  constructor(private httpClient: HttpClient) { }

  getAccommodations(
    longitude: number, 
    latitude: number, 
    pageSize: number, 
    pageNumber: number, 
    address?: string, 
    checkIn?: string, 
    checkOut?: string,
    numberOfGuests?:number
  ) {
    let params: any = {};
  
    if (address) {
      params.address = address;
    }
    if (checkIn) {
      params.checkIn = checkIn;
    }
    if (checkOut) {
      params.checkOut = checkOut;
    }
    if(numberOfGuests && numberOfGuests!=0)
    {
      params.numberOfGuests = numberOfGuests;
    }
  
    return this.httpClient.get(`${environment.api}/Accommodation/get-accommodations/${longitude}/${latitude}/${pageSize}/${pageNumber}`, { params });
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
  createAccomodation(accommodationModel : Accommodation) : Observable<any>
  {
    console.log(accommodationModel);
     return this.httpClient.post(`http://localhost:5153/Accommodation/create-accommodation`,accommodationModel);
  }
  getAccommodation(id : string)
  {
    console.log(id);
     return this.httpClient.get(`http://localhost:5153/Accommodation/get-accommodation-by-id/${id}`);
  }
}
