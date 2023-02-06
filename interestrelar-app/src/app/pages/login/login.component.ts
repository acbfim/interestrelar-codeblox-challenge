import { JwtHelperService } from '@auth0/angular-jwt';
import { login } from './../../interfaces/login';
import { StorageService } from './../../services/storage.service';
import { AuthService } from './../../services/auth.service';
import { Component, HostListener, OnInit } from '@angular/core';
import { GlobalService } from 'src/app/services';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  isDesktop = this._globalService.isDesktop();
  hide = true;
  load = false;
  jwtHelper = new JwtHelperService();

  creds: login = {
    username: '',
    password: ''
  };

  salvarLogin = false;

  constructor(
    private _globalService: GlobalService
    , private _authService: AuthService
    , private _storageService: StorageService
  ) {

  }
  ngOnInit(): void {
    this.getLoginCreds();
    this.getAccess();

    const token = this._storageService.getAccess().accessToken;

    console.log("exp",this.jwtHelper.isTokenExpired(token))

  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.isDesktop = this._globalService.isDesktop();
  }

  login() {
    if (this.creds) {

      this.load = true;

      this._authService.login(this.creds).subscribe(
        (_retorno: any) => {
          this.load = false;
          this._globalService.log(_retorno,"LOGUEI")

            this._storageService.setAccess(_retorno);
            this._globalService.navigateTo('auth/dashboard');
            this._globalService.sendAlert("Login realizado com sucesso!", 'Ok');

            if (this.salvarLogin) {
              this._storageService.setItem("loginAccess", this.creds);
            } else {
              this.clearAccess()
            }

        }, error => {
          this.load = false;
          console.log(error);
          let msg = error.message;

          if(error.status === 401)
            msg = 'Login ou senha inválidos';
          this._globalService.sendAlertError(msg, 'OK');
          this._globalService.navigateTo('login');

        }
      );
    }

  }

  getLoginCreds() {
    let cred = this._storageService.getItem("loginAccess");
    if (cred !== null) {
      this.creds = cred;
      this.salvarLogin = true;
    }
  }

  getAccess() {
    this._globalService.log(this._storageService.getAccess(),"ACCESS_")
  }

  clearAccess() {
    if (this.salvarLogin) {
      this.creds = {
        username: '',
        password: ''
      };
      this._storageService.cleanItem("loginAccess");
    }
  }

  async refresh(){

   const isRefreshSuccess = await this._authService.tryRefreshingTokens();
   console.log(isRefreshSuccess)
  }



}
