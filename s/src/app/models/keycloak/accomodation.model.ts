export interface AccommodationModel {
    id: string; 
    propertyName: string;
    description: string;
    address: string;
    pricePerNight: number;
    availableFrom: Date; 
    availableTo: Date;
    photos: string[]; 
    userId: string;
    // Facilities
    parking?: boolean; 
    wifi?: boolean;
    petsAllowed?: boolean;
    swimmingPool?: boolean;
    spa?: boolean;
    fitnessCentre?: boolean;
    nonSmokingRooms?: boolean;
    roomService?: boolean;
  }
  