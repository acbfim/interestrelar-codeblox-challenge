import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { RetornoDto } from 'src/app/interfaces/RetornoDto';
import { GlobalService } from 'src/app/services';
import { GeralService } from 'src/app/services/geral.service';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  public loading = false;

  avaliableYears: any = [];
  avaliableMonths: any = [];
  allExits: any;
  allData: any = [];
  dashboard: any = {};



  displayedColumns: string[] = [
    'Cargueiro',
    'Saída',
    'Minerio',
    'DataRecebimento',
    'Valor',
  ];
  dataSource = new MatTableDataSource<any>();
  resultsLength = 0;
  pageEvent: PageEvent | undefined;

  pageSize = 10;
  pageIndex = 0;

  selectedFilter: any = {};

  handlePageEvent(e: PageEvent) {
    this.pageEvent = e;
    this.pageSize = e.pageSize;
    this.pageIndex = e.pageIndex;

    console.log('NEXT: ', this.pageEvent);
    this.GetAllExitsByYearAndMonth(this.selectedFilter.year, this.selectedFilter.month);
    this.GetAllByYearAndMonth(this.selectedFilter.year, this.selectedFilter.month);
  }

  getByFilter(year: number, month: number) {
    if (year > 0) this.selectedFilter.year = year;

    if (month > 0) this.selectedFilter.month = month;

    this.GetAllExitsByYearAndMonth(year, month);
    this.GetAllByYearAndMonth(year, month);
  }

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;

  constructor(
    private _globalService: GlobalService,
    private _geralService: GeralService
  ) {}

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.paginator.pageSize = 10;
  }

  async ngOnInit() {
    try {
      this.selectedFilter.month = 1;

      await this.getAllAvaliableYears();
      await this.getAllAvaliableMonthsByYear();

      this.GetAllExitsByYearAndMonth(
        this.avaliableYears,
        this.selectedFilter.month
      );
      this.GetAllByYearAndMonth(this.selectedFilter.year, this.selectedFilter.month);
    } catch (error) {
      this._globalService.sendAlertError('Erro desconhecido', 'Ok');
    }
  }

  async getAllAvaliableYears(): Promise<void> {
    this.loading = true;
    return new Promise((resolve, reject) => {
      this._geralService.getAllAvaliableYear().subscribe(
        (_retorno: RetornoDto) => {
          this.loading = false;
          if (!_retorno.success) {
            reject(_retorno.message);
          } else {
            this.avaliableYears.push(_retorno.data[0]);
            this.selectedFilter.year = this.avaliableYears[0];
            console.log('YEARS', this.avaliableYears);
            resolve();
          }
        },
        (error) => {
          this.loading = false;
          reject(error.message);
        }
      );
    });
  }

  async getAllAvaliableMonthsByYear(): Promise<void> {
    this.loading = true;
    return new Promise((resolve, reject) => {
      this._geralService
        .GetAllAvaliableMonthsByYear(this.avaliableYears)
        .subscribe(
          (_retorno: RetornoDto) => {
            this.loading = false;
            if (!_retorno.success) {
              reject(_retorno.message);
            } else {
              this.avaliableMonths = _retorno.data;
              console.log('MONTHS', this.avaliableMonths);
              resolve();
            }
          },
          (error) => {
            this.loading = false;
            reject(error.message);
          }
        );
    });
  }

  GetAllExitsByYearAndMonth(year: number, month: number) {
    this.loading = true;

    this._geralService
      .getAllExitsByYearAndMonth(this.pageIndex, year, month)
      .subscribe(
        (_retorno: RetornoDto) => {
          this.loading = false;

          if (_retorno.success) {
            this.allExits = _retorno;
            console.log('allExits', this.allExits);

            this.dataSource = new MatTableDataSource(_retorno.data);

            this.resultsLength = _retorno.totalItems;
          } else {
            this._globalService.sendAlertError(_retorno.message);
          }
        },
        (error) => {
          this.loading = false;
          this._globalService.sendAlertError(error.message);
        }
      );
  }

  GetAllByYearAndMonth(year: number, month: number) {
    this.loading = true;

    console.log("YEAR: ", year)
    console.log("MONTH: ", month)

    this._geralService
      .getAllByYearAndMonth(0, year, month)
      .subscribe(
        (_retorno: RetornoDto) => {
          this.loading = false;

          if (_retorno.success) {
            this.allData = _retorno;
            console.log('allData', this.allData);
            this.tratarDashboard();
          } else {
            this._globalService.sendAlertError(_retorno.message);
          }
        },
        (error) => {
          this.loading = false;
          this._globalService.sendAlertError(error.message);
        }
      );
  }

  tratarDashboard() {
    let data = this.allData.data;
    let dashFreighter: any = [];
    let dashMineral: any = [];

    for (let i = 0; i < data.length; i++) {
      let freightersAux = data[i].totalByFreighter;
      let mineralsAux = data[i].totalByMineral;

      for (let j = 0; j < freightersAux.length; j++) {
        let elFreighter = data[i].totalByFreighter[j];
        let elMineral = data[i].totalByMineral[j];

        let f = Object.assign({
          totalUsed: elFreighter.totalUsed,
          totalIdle: elFreighter.totalIdle,
          freighter: elFreighter.freighter.class,
        });
        let m = Object.assign({
          mineral: elMineral.mineral.name,
          feature: elMineral.mineral.feature,
          amount: elMineral.amount,
        });

        dashFreighter.push(f);
        dashMineral.push(m);
      }
    }

    const groupedDataMineral: any = [];
    dashMineral.forEach((data: any) => {
      const existingGroup = groupedDataMineral.find(
        (group: any) =>
          group.mineral === data.mineral && group.feature === data.feature

      );

      if (existingGroup) {
        existingGroup.amount += data.amount;
      } else {
        groupedDataMineral.push({
          mineral: data.mineral,
          feature: data.feature,
          amount: data.amount,
        });
      }
    });

    const groupedDataFreighter = dashFreighter.reduce((acc: any, currentValue: any) => {
      const existingGroup = acc.find((group: { freighter: any; }) => group.freighter === currentValue.freighter);

      if (existingGroup) {
        existingGroup.totalUsed += currentValue.totalUsed;
        existingGroup.totalIdle += currentValue.totalIdle;
        existingGroup.percentIdle = (existingGroup.totalIdle / (existingGroup.totalIdle + existingGroup.totalUsed)) * 100;
      } else {
        acc.push({
          freighter: currentValue.freighter,
          totalUsed: currentValue.totalUsed,
          totalIdle: currentValue.totalIdle,
          percentIdle: (data.totalIdle / (data.totalIdle + data.totalUsed)) * 100
        });
      }

      return acc;
    }, []);

    this.dashboard = {};

    this.dashboard = Object.assign({freighter: groupedDataFreighter, mineral: groupedDataMineral});

    console.log("DASH: ",this.dashboard)


  }
}
