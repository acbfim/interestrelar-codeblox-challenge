import { Component, HostListener, Input, OnInit } from '@angular/core';
import { GlobalService } from 'src/app/services';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
  styleUrls: ['./default-header.component.scss']
})
export class DefaultHeaderComponent implements OnInit {

  sidebarOpen = true;
  isDesktop = this._globalService.isDesktop();

  @Input() sidenavValue: any = false;

  /**
   *
   */
  constructor(
    private _globalService: GlobalService
  ) {
    this.autoToggleSidebar();
  }
  ngOnInit(): void {
    this.sidebarOpen = this.sidenavValue.opened
  }

  toggle(){
    this.sidebarOpen = !this.sidebarOpen;
    this._globalService._sideNavToggle(null);


  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.isDesktop = this._globalService.isDesktop();
    this.autoToggleSidebar();
  }

  autoToggleSidebar(){
    if(this.isDesktop)
    {
      this.sidebarOpen = true;
    }
    else
    {
      this.sidebarOpen = false;
    }

    this._globalService._sideNavToggle(this.sidebarOpen);
  }



}
