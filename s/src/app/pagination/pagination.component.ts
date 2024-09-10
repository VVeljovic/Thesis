import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { AccommodationService } from '../services/accomodation/accommodation.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent {

  numberOfPages:number=0;
  pagesArray:number[]=[];
  constructor(private accommodationService:AccommodationService){
    // this.subscription=this.categoryService.numberOfArticles.subscribe((respo)=>{
     
      this.numberOfPages=Math.ceil(30/5);
     this.pagesArray=Array.from({ length: this.numberOfPages }, (_, i) => i + 1);
    // })
  }
  currentPage:number = 1;
onPageNumberClick(num:number){
  if(num<0)
  {
    //this.categoryService.incrementOrDecrementPageNumber(num);
    if(num == -1)
    {
      this.currentPage--;
    }
    else
    {
      this.currentPage++;
    }
  }
  else
  {
    this.currentPage=num;
 
  }
}
}
