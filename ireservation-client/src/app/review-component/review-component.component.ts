import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CreateReviewFormComponent } from '../create-review-form/create-review-form.component';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from '../pagination/pagination.component';
import { AccommodationService } from '../services/accommodation.service';

@Component({
  selector: 'app-review-component',
  standalone: true,
  imports: [CreateReviewFormComponent, CommonModule, PaginationComponent],
  templateUrl: './review-component.component.html',
  styleUrl: './review-component.component.css'
})
export class ReviewComponentComponent {
  @Input() accommodation: any;
  currentPage : number = 1;
  constructor(public dialog: MatDialog, private accommodationService : AccommodationService) {} addReview() {
    console.log(this.accommodation);

   
    const dialogRef = this.dialog.open(CreateReviewFormComponent, {
      
      width: '400px',
      data: { accommodation: this.accommodation }
    });

    
    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      console.log(result);  
    });
  }
  onPageChange(pageNumber: number) {
    this.currentPage = pageNumber;
    console.log(`Trenutna stranica: ${this.currentPage}`);
    this.getReviews()
  }
  getReviews()
  {
    this.accommodationService.getReviews(this.accommodation.id,5,this.currentPage).subscribe((response)=>{
      console.log(response);
      this.accommodation.reviews = response;
    })
    }
  }

