import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Paging } from 'projects/viewmodels/paging';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MainDashboardService } from '../../services/main-dashboard.service';

@Component({
  selector: 'lib-admin-notification',
  templateUrl: './admin-notification.component.html',
  styleUrls: ['./admin-notification.component.scss']
})
export class AdminNotificationComponent implements OnInit, AfterViewInit {
  paging: Paging = {};
  orderBy: number;
  orderColumn: number;
  response: ResponseModel = {};
  notificationlist: Notificationlist[] = [];
  notificationCount: number;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumnsRequired: string[] = ['employeeName', 'description', 'createdDate'];
  dataSourceRequired = new MatTableDataSource(this.notificationlist);

  constructor(private dashboardService: MainDashboardService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.showNotification();
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceRequired !== undefined ? this.dataSourceRequired.sort = this.sort : this.dataSourceRequired;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.showNotification())
        )
        .subscribe();
    }, 2000);

  }
  
  showNotification() {
    this.getSortingOrder();    
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.dashboardService.getAdminNotification(data).subscribe(res => {
      this.response = res;
      if (this.response.responseData) {
        this.notificationCount = this.response.total;
        // this.notificationCount = this.notificationlist.length > 0 ? this.notificationlist.length : 0;
        this.notificationlist = this.response.responseData;
        this.dataSourceRequired = new MatTableDataSource(this.notificationlist);
      }
    });
  }

  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'createdDate':
        this.orderColumn = 0;
        break;
      case 'employeeName':
        this.orderColumn = 1;
        break;
      case 'description':
        this.orderColumn = 2;
        break;
      default:
        this.orderColumn = 0;
        break;
    }
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.showNotification();
  }

}

export interface Notificationlist {
  employeeName?: string;
  description?: string;
  eventName?: Date;
  createdDate?: Date;
}