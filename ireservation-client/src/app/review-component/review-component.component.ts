import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CreateReviewFormComponent } from '../create-review-form/create-review-form.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-review-component',
  standalone: true,
  imports: [CreateReviewFormComponent, CommonModule],
  templateUrl: './review-component.component.html',
  styleUrl: './review-component.component.css'
})
export class ReviewComponentComponent {
  @Input() accommodation: any;
  constructor(public dialog: MatDialog) {} addReview() {
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
  }

