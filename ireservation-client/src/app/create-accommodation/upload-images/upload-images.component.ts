import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-upload-images',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './upload-images.component.html',
  styleUrl: './upload-images.component.scss'
})
export class UploadImagesComponent {
  imagePreviews: (string | ArrayBuffer | null)[] = Array(8).fill(null);

  @Output() imagesSelected = new EventEmitter<(string | ArrayBuffer | null)[]>();

  onFileSelected(event: Event, boxNumber: number): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) {
      return;
    }

    const file = input.files[0];
    const reader = new FileReader();
    
    reader.onload = () => {
      this.imagePreviews[boxNumber - 1] = reader.result;
      this.imagesSelected.emit(this.imagePreviews);  
    };
    
    reader.readAsDataURL(file);
  }
}