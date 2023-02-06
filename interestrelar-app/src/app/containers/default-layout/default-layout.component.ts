import { MediaMatcher } from '@angular/cdk/layout';
import { ChangeDetectorRef, Component, OnDestroy, ViewChild, OnInit, AfterViewInit, Type, AfterContentInit, HostListener, AfterContentChecked, AfterViewChecked, EventEmitter } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { GlobalService } from 'src/app/services';

@Component({
  selector: 'app-default-layout',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss']
})

export class DefaultLayoutComponent implements OnDestroy, OnInit {
  @ViewChild('sidenav') sidenav!: MatSidenav;


  panelOpenState = false;
  sidebarOpen = false;

  mobileQuery!: MediaQueryList;

  isDesktop = this._globalService.isDesktop();

  fillerNav = Array.from({length: 8}, (_, i) => `Nav Item teste tamanho  ${i + 1}`);

  public isExpanded = false;

  public toggleMenu() {
    this.isExpanded = !this.isExpanded;
  }

  public menu:any = [];






  private _mobileQueryListener: () => void;
  static children: Type<any> | undefined;


  constructor(
    changeDetectorRef: ChangeDetectorRef, media: MediaMatcher
    , private _globalService: GlobalService
    ) {
      this.menu = [
        { link: "auth/dashboard", name: "Dashboard", icon: "dashboard", items: [] }


      ];

    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);

  }




  ngOnInit(): void {
    this.realTimeMethods();
  }

  navigateTo(route: string){
    if(!this._globalService.isDesktop())
    {
      this.sidenav.close();
    }
    this._globalService.navigateTo(route);

  }

  toggle(value?: boolean){
    if(value === null){
      if(!this.sidenav.opened){
        this.sidenav.open();
      }else{
        this.sidenav.close();
      }
    }else{
      this.sidenav.toggle(value);
    }

    this.sidebarOpen = this.sidenav.opened;

  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.isDesktop = this._globalService.isDesktop();
    this.sidebarOpen = this.sidenav.opened;
  }

  ngOnDestroy(): void {

    this.mobileQuery.removeListener(this._mobileQueryListener);

  }




realTimeMethods(){
  this._globalService.sideNavToggle.subscribe(
    (value: any) => {
      this.toggle(value);

    }
  );
}




}

