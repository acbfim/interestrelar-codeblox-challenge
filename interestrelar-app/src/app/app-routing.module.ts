import { DefaultLayoutComponent } from './containers/default-layout/default-layout.component';
import { LoginComponent } from './pages/login/login.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


const routes: Routes = [

  {
    path: 'login', component: LoginComponent
  },
  {
    path: 'auth',
    loadChildren: () =>
      import ('./pages/authenticated/authenticated.module').then(x => x.AuthenticatedModule)
  }

  , { path: '', redirectTo: 'login', pathMatch: 'full' }
  , { path: '**', redirectTo: '', pathMatch: 'full' }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
