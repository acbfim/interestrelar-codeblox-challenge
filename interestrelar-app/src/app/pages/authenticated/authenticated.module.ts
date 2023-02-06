import { MaterialModule } from './../../modules/material.module';
import { AuthenticatedRoutingModule } from './authenticated-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DefaultFooterComponent, DefaultHeaderComponent, DefaultLayoutComponent } from 'src/app/containers';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UsCurrencyPipe } from 'src/app/helpers/util';
import { NgxLoadingModule } from 'ngx-loading';
import { ReactiveFormsModule } from '@angular/forms';

const APP_CONTAINERS = [
  DefaultFooterComponent,
  DefaultHeaderComponent,
  DefaultLayoutComponent,
];

@NgModule({
  declarations: [
    ...APP_CONTAINERS,
    HomeComponent,
    DashboardComponent,
    UsCurrencyPipe,



  ],
  imports: [
    CommonModule
    ,MaterialModule
    ,AuthenticatedRoutingModule
    ,NgxLoadingModule.forRoot({}),
    ReactiveFormsModule

  ]
})
export class AuthenticatedModule { }
