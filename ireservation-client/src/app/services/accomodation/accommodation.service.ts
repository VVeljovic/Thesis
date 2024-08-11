import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccommodationModel } from '../../models/keycloak/accomodation.model';
import { environment } from '../../../environments/environment';
import { v4 as uuidv4 } from 'uuid';
@Injectable({
  providedIn: 'root'
})
export class AccommodationService {

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
}
