import { AuthGuard } from './../../auth/auth.guard';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DefaultLayoutComponent } from 'src/app/containers';
import { LoginComponent } from '../login/login.component';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [

{
  path: ''
  , component: DefaultLayoutComponent
  , children: [
    {
      path: 'home', component: HomeComponent, canActivate: [AuthGuard]
    }
    ,{
      path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard]
    }
  ],
},

];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthenticatedRoutingModule { }
