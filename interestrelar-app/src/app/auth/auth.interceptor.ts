import { StorageService } from './../services/storage.service';
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient, HttpHeaders, HttpRequest, HttpInterceptor, HttpHandler, HttpEvent } from '@angular/common/http';
import { Router } from "@angular/router";
import { tap } from "rxjs/internal/operators/tap";

@Injectable({ providedIn: 'root' })

export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private router: Router
    , private storageService: StorageService
  ) {

  }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    var uriReq = req.url.split('//')[1].split('/')[0];
    if (
      uriReq === 'viacep.com.br'
      || uriReq === 'discordapp.com'
      || uriReq === 'cors-anywhere.herokuapp.com'
    ) {
      return next.handle(req.clone());
    } else {

      let token = this.storageService.getAccess().accessToken;


      if (token !== null) {
        const cloneReq = req.clone({
          headers: req.headers.set('Authorization', `Bearer ${token}`)
        });
        return next.handle(cloneReq).pipe(
          tap(
            succ => { },
            err => {
              if (err.status === 401) {
                this.storageService.cleanAccess();
                this.router.navigateByUrl('/login');
              }
            }
          )
        );

      }
      else {
        return next.handle(req.clone());
      }
    }


  }
}
