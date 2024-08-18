import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccommodationModel } from '../../models/keycloak/accomodation.model';
import { environment } from '../../../environments/environment';
import { v4 as uuidv4 } from 'uuid';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AccommodationService {

  private apiKey = '4f618020d8624275a04534be277ad372';
  private apiKey2 = '0c8f2e1b803645a890e17450f7f1ee86';
  constructor(private httpClient: HttpClient) 
  { }
  createAccomodation(accommodationModel : AccommodationModel)
  {
    const randomGuid = uuidv4();
  console.log('Generated GUID:', randomGuid);
    console.log(accommodationModel);
    accommodationModel.id = randomGuid;
     this.httpClient.post(`https://localhost:7061/Accommodation/createAccommodation`,accommodationModel).subscribe((response)=>console.log(response));
  }
  getAccommodations( pageSize:number, pageIndex:number)
  {
    return this.httpClient.get(`${environment.api}/Accommodation/getAccommodations/${pageSize}/${pageIndex}`)
  }

  getMyLocation()
  {
    return this.httpClient.get('https://ipapi.co/json/');
  }
  getCoordinates(city: string): Observable<any> {
    const apiUrl = `https://api.opencagedata.com/geocode/v1/json?q=${city}&key=${this.apiKey}`;
    return this.httpClient.get(apiUrl);
  }
  searchCities(query:string):Observable<any>{
    const apiUrl = `https://api.opencagedata.com/geocode/v1/json?q=${query}&key=${this.apiKey2}&limit=5`;
    return this.httpClient.get(apiUrl);
  }
}
