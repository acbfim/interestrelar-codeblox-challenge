import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GlobalService } from './global.service';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class GeralService {
  baseUrl = environment.urlApi + "/geral";
constructor(
  private http: HttpClient
    ,private _storageService: StorageService
    , private _globalService: GlobalService
) { }

getAll(skip: number): Observable<any> {
  return this.http.get<any>(`${this.baseUrl}/get-all/${skip}`);
}

getAllExitsByYearAndMonth(skip: number,year: number,month: number): Observable<any> {
  return this.http.get<any>(`${this.baseUrl}/get-all-exits/by-year-and-month/${skip}/${year}/${month}`);
}

getAllByYearAndMonth(skip: number,year: number,month: number): Observable<any> {
  return this.http.get<any>(`${this.baseUrl}/get-all/by-year-and-month/${skip}/${year}/${month}`);
}

getAllAvaliableYear(): Observable<any> {
  return this.http.get<any>(`${this.baseUrl}/get-all/avaliable-year`);
}

GetAllAvaliableMonthsByYear(year: number): Observable<any> {
  return this.http.get<any>(`${this.baseUrl}/get-all/avaliable-month/by-year/${year}`);
}



}
