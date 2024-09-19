import { StripeUser } from "./stripeuser";

export interface Booking {
    date: string;
    totalAmount: number;
    status: string;
    userId: string;
    accommodationId: string;
    type: string;
    dateFrom: string; 
    dateTo: string;  
    stripeUserDto?: StripeUser;
  }