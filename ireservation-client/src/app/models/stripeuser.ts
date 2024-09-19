import { CreditCard } from "./creditcard";

export interface StripeUser {
    email: string;
    name: string;
    creditCard: CreditCard;
  }