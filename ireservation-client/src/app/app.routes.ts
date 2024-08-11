import { Routes } from '@angular/router';
import { CreateAccommodationFormComponent } from './create-accommodation-form/create-accommodation-form.component';
import { FrontPageComponent } from './front-page/front-page.component';
import { PaginationComponent } from './pagination/pagination.component';

export const routes: Routes = [
    { path: 'create', component: CreateAccommodationFormComponent }, 
    { path: 'front', component: FrontPageComponent },
    {path:'pagination', component: PaginationComponent} 
];
