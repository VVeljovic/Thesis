import { Component } from '@angular/core';
import { KeycloakService } from '../services/keycloak.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  constructor(private keycloakService : KeycloakService, private router : Router)
  {

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
