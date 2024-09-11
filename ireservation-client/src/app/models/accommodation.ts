import { Amenity } from "./amenity";

export interface Accommodation {
    propertyName: string;
    description: string;
    address: string;
    pricePerNight: number; 
    numberOfGuests: number;
    availableFrom: Date;
    availableTo: Date;
    photos: (string | ArrayBuffer | null)[]; 
    userId: string;
    amenity: Amenity; 
    longitude: number;
    latitude : number;
  }