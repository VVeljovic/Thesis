import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-credit-card',
  standalone: true,
  imports: [FormsModule,    ButtonModule],
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
    console.log(this.cardHolderName);
    console.log(this.cardNumber);
    console.log(this.cvv);
    console.log(this.expirationDate)
  }
}