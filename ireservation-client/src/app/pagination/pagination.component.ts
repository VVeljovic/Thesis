import { Component, EventEmitter, Output } from '@angular/core';
import { AccommodationService } from '../services/accommodation.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent {
  @Output() pageChange: EventEmitter<number> = new EventEmitter<number>();
  numberOfPages: number = 0;
  pagesArray: number[] = [];
  currentPage: number = 1;

  constructor() {
    this.numberOfPages = Math.ceil(30 / 5); // Primer broja stranica
    this.pagesArray = Array.from({ length: this.numberOfPages }, (_, i) => i + 1);
  }

  onPageNumberClick(num: number) {
    if (num < 0) {
      // Ako je num -1, smanji trenutnu stranicu; ako je -2, poveća trenutnu stranicu
      if (num === -1 && this.currentPage > 1) {
        this.currentPage--;
      } else if (num === -2 && this.currentPage < this.numberOfPages) {
        this.currentPage++;
      }
    } else {
      this.currentPage = num;
    }
    this.pageChange.emit(this.currentPage); // Emituj trenutnu stranicu
  }
}