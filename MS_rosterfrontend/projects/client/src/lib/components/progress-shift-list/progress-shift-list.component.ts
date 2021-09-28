import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { ClientService } from '../../services/client.service';
import * as moment from 'moment';
import { environment } from 'src/environments/environment';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'lib-progress-shift-list',
  templateUrl: './progress-shift-list.component.html',
  styleUrls: ['./progress-shift-list.component.scss']
})

export class ProgressShiftListComponent implements OnInit {
 getErrorMessage: 'Please Enter Value';
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  employeeId: number;
  responseModel: ResponseModel = {};
  rForm: FormGroup;
  paging: Paging = {};
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  orderBy: number;
  orderColumn: number;
  ShiftModel: ClientShiftDetails[] = [];
  displayedColumnsRequired: string[] = ['name','startdate',  'enddate', 'starttime', 'endtime', 'duration','action'];
  dataSourceRequired = new MatTableDataSource(this.ShiftModel);
  ClientId: number;
  totalCount: number;
  
  constructor(private route: ActivatedRoute, private clientService: ClientService,private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params.Id);
     
    });
   
    this.ClientId > 0 ? this.getClientShitList() : null;
    this.dataSourceRequired.sort = this.sort;
   
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceRequired !== undefined ? this.dataSourceRequired.sort = this.sort : this.dataSourceRequired;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getClientShitList())
        )
        .subscribe();
    }, 2000);

  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'name':
        this.orderColumn = 0;
        break;
      case 'startdate':
        this.orderColumn = 1;
        break;
      case 'enddate':
        this.orderColumn = 2;
        break;
        case 'starttime':
          this.orderColumn = 3;
          break; 
          case 'endtime':
          this.orderColumn = 4;
          break;
          case 'duration':
            this.orderColumn = 5;
            break;
      case 'createdDate':
        this.orderColumn = 6;
        break;

      default:
        break;
    }
  }
  getClientShitList() {
    this.getSortingOrder();
   const data = {
      ClientId: this.ClientId,
      PageSize: this.paging.pageSize,
      pageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
 }
    this.clientService.getClientShitList(data).subscribe(res => {
      this.responseModel = res;
      this.totalCount = this.responseModel.total;
      switch (this.responseModel.status) {
        case 1:
          this.ShiftModel = this.responseModel.responseData;
          this.dataSourceRequired = new MatTableDataSource(this.ShiftModel);
          break;
         default:
         this.dataSourceRequired = new MatTableDataSource([]);
         this.notificationService.Warning({ message: this.responseModel.message, title: '' });
         break;
          break;
      }

    });
  }
 
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getClientShitList();
  }
 
}
export interface ClientShiftDetails {
  id?: number
  employeeId?: number;
  name?: number;
  startDate?: Date;
  endDate?: Date;
  startTimeString?: string;
  endTimeString?: string;
  duration?: number;
  
}