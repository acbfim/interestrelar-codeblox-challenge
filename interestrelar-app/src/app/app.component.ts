import { Component, Inject, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { StorageService } from './services/storage.service';


@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  title = 'ProSales-app';

  teste = environment;



  constructor(
    private _storageService: StorageService
      ) {

  }

  ngOnInit(): void {

  }



}
