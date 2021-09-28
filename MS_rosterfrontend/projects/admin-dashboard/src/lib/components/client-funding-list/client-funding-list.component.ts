import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import * as moment from 'moment';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Paging } from 'projects/viewmodels/paging';
import { PageEvent } from '@angular/material/paginator';
import { FundType } from 'projects/lhs-service/src/lib/viewmodels/gender';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { ClientService } from 'projects/client/src/lib/services/client.service';
import { ClientfundingTypeInfo } from 'projects/client/src/lib/view-models/client-fundingTypeInfo';
import { DashboardService } from 'projects/dashboard/src/projects';
import { MainDashboardService } from '../../services/main-dashboard.service';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'lib-client-funding-list',
  templateUrl: './client-funding-list.component.html',
  styleUrls: ['./client-funding-list.component.scss']
})
export class ClientFundingListComponent implements OnInit {
  @Input() ClientfundingTypeInfo: ClientfundingTypeInfo;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  fundtypeInfo: ClientfundingTypeInfo[];
  rForm: FormGroup;
  rFormfundtype: FormGroup;
  rFormEditfundtype: FormGroup;
  response: ResponseModel = {};
  paging: Paging = {};
  FundingtypeList = [];
  orderBy: number;
  orderColumn: number;
  displayedColumnsRequired: string[] = ['clientName', 'fundingSource','referencenumber',  'startDate','endDate','expiry', 'balance'];
  dataSourceRequired = new MatTableDataSource(this.FundingtypeList);
  totalCount: number;
  constructor(private route: ActivatedRoute, private fb: FormBuilder, private dashboardService: MainDashboardService,
    private notificationService: NotificationService, private commonService: CommonService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.GetAllClientFundingList();
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceRequired !== undefined ? this.dataSourceRequired.sort = this.sort : this.dataSourceRequired;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.GetAllClientFundingList())
        )
        .subscribe();
    }, 2000);

  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'clientName':
        this.orderColumn = 0;
        break;
      case 'fundingSource':
        this.orderColumn = 1;
        break;
      case 'startDate':
        this.orderColumn = 2;
        break;
        case 'endDate':
          this.orderColumn = 3;
          break;
          case 'expiry':
          this.orderColumn = 4;
          break;
          case 'balance':
            this.orderColumn = 5;
            break;
      case 'createdDate':
        this.orderColumn = 6;
        break;

      default:
        break;
    }
  }
  GetAllClientFundingList() {
    this.getSortingOrder();
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
     this.dashboardService.getAllClientFundingList(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      let fundtypearray = [];
      if (this.response.responseData) {
        this.fundtypeInfo = this.response.responseData;
        this.dataSourceRequired = new MatTableDataSource(this.fundtypeInfo);
         }
      else {
        this.dataSourceRequired = new MatTableDataSource(this.FundingtypeList);
      }

    });
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetAllClientFundingList();
  }
}
