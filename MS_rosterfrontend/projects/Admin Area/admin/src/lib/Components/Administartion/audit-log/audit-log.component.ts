import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { AdminService } from '../../../admin.service';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-audit-log',
  templateUrl: './audit-log.component.html',
  styleUrls: ['./audit-log.component.scss']
})

export class AuditLogComponent implements OnInit {
 response: ResponseModel = {};
  orderBy: number;
  orderColumn: number;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('EntityName') entityName: ElementRef;
  @ViewChild('status') Status: ElementRef;
  paging: Paging = {};
  searchByName: any;
  totalCount: number;
  AuditList: [];
  displayedColumns: string[] = ['sr', 'Description','EntityName', 'appliedDate'];
  dataSource = new MatTableDataSource(this.AuditList);
  constructor(private adminService: AdminService,private notification: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }
  
  ngOnInit(): void {
    this.getActivityLogList();
    this.dataSource.sort = this.sort;
   }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getActivityLogList())
        )
        .subscribe();
    }, 2000);

  }
  
  Search() {
    this.searchByName = this.entityName.nativeElement.value;
    this.getActivityLogList();
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'Description':
        this.orderColumn = 0;
        break;
        case 'EntityName':
          this.orderColumn = 1;
          break;
      case 'appliedDate':
        this.orderColumn = 2;
        break;
       case 'createdDate':
        this.orderColumn = 5;
        break;
      default:
        break;
    }
  }
  getActivityLogList() {
   this.getSortingOrder();
    const data = {
      SearchTextByName: this.searchByName,
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.adminService.getUserActivitylog(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      if (this.response.responseData) {
        this.AuditList = this.response.responseData;
        this.dataSource = new MatTableDataSource(this.AuditList);
         }
      else {
        this.notification.Warning({ message: this.response.message, title: null });
        this.dataSource = new MatTableDataSource(null);
      }
    });
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getActivityLogList();
  }

}