import { StorageService } from './storage.service';
import { login } from './../interfaces/login';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from 'rxjs';
import { AuthenticatedResponse } from '../interfaces/AuthenticatedResponse';
import { GlobalService } from './global.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.urlApi;
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(
    private http: HttpClient
    ,private _storageService: StorageService
    , private _globalService: GlobalService
    ) { }

  login(cred: any) {
    return this.http
      .post<login>(`${this.baseUrl}/user/login`, cred);
  }

  public async tryRefreshingTokens(): Promise<boolean> {
    const access: any = this._storageService.getAccess();
    if (access === null) {
      return false;
    }

    const credentials = JSON.stringify({ accessToken: access.accessToken, refreshToken: access.refreshToken });
    let isRefreshSuccess: boolean;
    const refreshRes = await new Promise<any>((resolve, reject) => {
      this.http.post<AuthenticatedResponse>(`${this.baseUrl}/user/refresh`, credentials, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe({
        next: (res: AuthenticatedResponse) => resolve(res),
        error: (_) => { reject; isRefreshSuccess = false;}
      });
    });

    this._globalService.log(refreshRes,"REFRESH TOKEN_")

    access.accessToken = refreshRes.object.accessToken;
    access.refreshToken = refreshRes.object.refreshToken;

    this._storageService.cleanAccess();
    this._storageService.setAccess(access);

    isRefreshSuccess = true;
    return isRefreshSuccess;
  }

  public getAllowPagesByUser(userExternalI: string) : any[] {
    return [
      {page: "home"}
      ,{page: "dashboard"}
  ];
  }
}
