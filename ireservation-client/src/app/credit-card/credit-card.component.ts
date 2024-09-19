import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Booking } from '../models/booking';
import { CreditCard } from '../models/creditcard';
import { StripeUser } from '../models/stripeuser';
import { TransactionService } from '../services/transaction.service';

@Component({
  selector: 'app-credit-card',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './credit-card.component.html',
  styleUrl: './credit-card.component.css',
 
})
export class CreditCardComponent {
  cardHolderName: string = '';
  cardNumber: string = '';
  expirationDate: string = '';
  cvv :string =''
  formattedCardNumber: string = '';
  formattedExpirationDate: string = '';
  constructor(@Inject(MAT_DIALOG_DATA) public data:any, private transactionService : TransactionService)
  {
    console.log(data.booking)
  }
  formatCardNumber() {
    let rawCardNumber = this.cardNumber.replace(/\D/g, '');
    this.formattedCardNumber = rawCardNumber.replace(/(.{4})/g, '$1 ').trim();
  }

  formatExpirationDate() {
    let rawExpirationDate = this.expirationDate.replace(/\D/g, '');
    if (rawExpirationDate.length >= 2) {
      this.formattedExpirationDate = rawExpirationDate.substring(0, 2) + '/' + rawExpirationDate.substring(2, 4);
    } else {
      this.formattedExpirationDate = rawExpirationDate;
    }
  }
  onSubmit()
  {
   const obj =   JSON.parse(localStorage.getItem('userInfo') || '{}');
    console.log(this.cardHolderName);
    console.log(this.cardNumber);
    console.log(this.cvv);
    console.log(this.expirationDate)
    const month = this.expirationDate.substring(0, 2);

    const year = "20" + this.expirationDate.substring(2, 4);
    const creditCardModel : CreditCard =
    {
      name :this.cardHolderName,
      creditCard:this.cardNumber,
      expirationYear:year,
      expirationMonth:month,
      cvc :this.cvv
    }
    const stripeUser : StripeUser = 
    {
      name : this.cardHolderName,
      email : obj.email,
      creditCard : creditCardModel
    }
    this.data.booking.stripeUserDto = stripeUser;
    this.transactionService.bookAccommodation(this.data.booking).subscribe((response)=>{
      console.log(response)
    }) 
   }
}