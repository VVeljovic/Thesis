import { Routes } from '@angular/router';
import { AccommodationPageComponent } from './accommodation-page/accommodation-page.component';
import { MainPageComponent } from './main-page/main-page.component';
import { UploadImagesComponent } from './create-accommodation/upload-images/upload-images.component';
import { CreateFormComponent } from './create-form/create-form.component';
import { BookAccommodationFormComponent } from './book-accommodation-form/book-accommodation-form.component';
import { ReviewComponentComponent } from './review-component/review-component.component';
import { CreateReviewFormComponent } from './create-review-form/create-review-form.component';
import { CreditCardComponent } from './credit-card/credit-card.component';
import { MyReservationsComponent } from './my-reservations/my-reservations.component';

export const routes: Routes = [
    { path: 'createAccommodation', component: CreateFormComponent }, 
    {path:'',component:MainPageComponent},
    { path: 'accommodation/:id', component: AccommodationPageComponent },
    {path: 'reservation/:id',component:BookAccommodationFormComponent},
    {path:'review',component:ReviewComponentComponent},
    {path:'createReview',component:CreateReviewFormComponent},
    {path:'creditCard', component:CreditCardComponent},
    {path:'myreservations',component:MyReservationsComponent}
];
