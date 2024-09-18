import { Component } from '@angular/core';
import { KeycloakService } from '../services/keycloak.service';
import { Router } from '@angular/router';
import { PaymentService } from '../services/payment.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  constructor(private keycloakService : KeycloakService, private router : Router, private paymentService : PaymentService)
  {
    const obj =   JSON.parse(localStorage.getItem('userInfo') || '{}');
    this.paymentService.getCustomerByEmail(obj.email).subscribe((response)=>console.log(response))
  }
  logout()
  {
    this.keycloakService.logout();
  }
  redirect(url : string)
  {
    this.router.navigate([url]);
  }
}
