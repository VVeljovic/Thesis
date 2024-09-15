import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccommodationService } from '../services/accommodation.service';

@Component({
  selector: 'app-accommodation-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './accommodation-page.component.html',
  styleUrl: './accommodation-page.component.css'
})
export class AccommodationPageComponent {
  id: string ='';
  accommodation :any;
  constructor(private route: ActivatedRoute, private accommodationService:AccommodationService, private router: Router) {
    this.id = this.route.snapshot.paramMap.get('id') ?? '';
    
   this.accommodationService.getAccommodation(this.id).subscribe((response)=>{console.log(response)

    this.accommodation = response;
   })
  }
  navigate()
  {
    this.router.navigate([`reservation/${this.id}`]);
  }
  ngOnInit(): void {

  }
}
