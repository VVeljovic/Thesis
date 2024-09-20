import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Review } from '../models/review';
import { UserProfile } from '../models/userprofile';
import { AccommodationService } from '../services/accommodation.service';

@Component({
  selector: 'app-create-review-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-review-form.component.html',
  styleUrl: './create-review-form.component.css'
})
export class CreateReviewFormComponent {
  ratingValue: number = 0;  
  stars: boolean[] = [false, false, false, false, false];  
  opinion: string = '';
  user!:any ;
  accommodation!:any;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private accommdationService : AccommodationService, private matDialogRef : MatDialogRef<CreateReviewFormComponent>) 
  {
    this.accommodation = data.accommodation;
    console.log(this.accommodation);
    const userInfo = localStorage.getItem('userInfo');
    if (userInfo) {
      this.user = JSON.parse(userInfo) as UserProfile;
      console.log(this.user)
    } else {
      this.user = { id: '', name: '', email: '' }; 
    }
  }

  selectStar(index: number) {
    this.ratingValue = index + 1;

    this.stars = this.stars.map((_, i) => i <= index);
  }

  submitReview() {
    console.log('Selected rating:', this.ratingValue);
    console.log('User opinion:', this.opinion);
    const now = new Date();
    const dateCreated = new Date(now.getTime() + 2 * 60 * 60 * 1000);
    const review : Review ={
      userId : this.user.name,
      accommodationId:this.accommodation.id,
      rating :this.ratingValue,
      comment:this.opinion,
      dateCreated :dateCreated
    } ;
    console.log(review);
    this.accommdationService.createReview(review).subscribe((response)=>{
      this.matDialogRef.close(); window.location.reload();
    })
  }
}