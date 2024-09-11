import { Routes } from '@angular/router';
import { AccommodationPageComponent } from './accommodation-page/accommodation-page.component';
import { MainPageComponent } from './main-page/main-page.component';
import { UploadImagesComponent } from './create-accommodation/upload-images/upload-images.component';
import { CreateFormComponent } from './create-form/create-form.component';

export const routes: Routes = [
    { path: 'createAccommodation', component: CreateFormComponent }, 
    {path:'uploadImages',component:UploadImagesComponent}
];
