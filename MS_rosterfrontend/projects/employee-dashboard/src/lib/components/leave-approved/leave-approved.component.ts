import { Component, OnInit, ViewChild, ElementRef, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { CommonService } from 'projects/lhs-service/src/projects';
import * as moment from 'moment';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { EmpDashboardService } from '../../services/emp-dashboard.service';
import { LeaveInfo } from '../../viewmodels/leave';
import { timingSafeEqual } from 'crypto';

@Component({
  selector: 'lib-leave-approved',
  templateUrl: './leave-approved.component.html',
  styleUrls: ['./leave-approved.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class LeaveApprovedComponent implements OnInit {
  employeeLeaveInfo: LeaveInfo[];
  empModel: LeaveInfo = {};
  rForm: FormGroup;
  rForm1: FormGroup;
  totalCount: number;
  paging: Paging = {};
  getErrorMessage: 'Please Enter Value';
  employeeId: any;
  response: ResponseModel = {};
  leaveId: any;
  responseModel: ResponseModel = {};
  @Input() leaveModel: LeaveInfo[];
  displayedColumnsLeave: string[] = ['fullName','leaveTypeName','leaveDateFrom','leaveDateTo','appliedDate','action'];
  dataSourceLeave: any;
  employeePrimaryInfo: {};
  @ViewChild('btnCancel') cancel: ElementRef;
  LeaveInfoModel: LeaveElementInfo = {};
  deleteLeaveId: number;
  EditList: LeaveElementInfo[] = [];
  LeaveListdata = [];
  orderBy: number;
  orderColumn: number;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('formDirective') private formDirective: NgForm;
  empId: any;
  Name: any;
  LeaveType: any;
  DateFrom: any;
  DateTo: any;
  Reason: any;
 
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private loaderService: LoaderService,
    private commonService:CommonService, private empService: EmpDashboardService, private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.employeeId = Number(params.params.id);
    });
     this.GetList();
     this.createForm();
  }
  createForm() {
    this.rForm = this.fb.group({
      remark: ['', Validators.required],
    });
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceLeave !== undefined ? this.dataSourceLeave.sort = this.sort : this.dataSourceLeave;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.GetList())
        )
        .subscribe();
    }, 2000);

  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'fullName':
        this.orderColumn = 0;
        break;
      case 'leaveTypeName':
        this.orderColumn = 1;
        break;
      case 'fromDate':
        this.orderColumn = 2;
        break;
      case 'toDate':
          this.orderColumn = 3;
          break;
      case 'leavereason':
          this.orderColumn = 4;
          break;
      case 'createdDate':
            this.orderColumn = 5;
            break;
      default:
            break;
    }
  }
 
  cancelModal() {
    this.rForm1.reset();
    this.formDirective.resetForm();
  }
  openEditDetails(elem) {
    document.getElementById("openEditModalButton").click();
    this.Name=elem.fullName;
    this.LeaveType=(elem.leaveTypeName);
    this.DateFrom=(elem.dateFrom);
    this.DateTo=(elem.dateTo);
    this.Reason=(elem.reason);
  }
  GetList() {
    this.getSortingOrder();
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      EmployeeId: this.employeeId,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.empService.getApplyleave(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      if (this.response.responseData) {
        this.employeeLeaveInfo = this.response.responseData;
        this.dataSourceLeave = new MatTableDataSource(this.employeeLeaveInfo);
         }
      else {
        this.dataSourceLeave = new MatTableDataSource(this.LeaveListdata);
      }
    });
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetList();
  }
  acceptLeave(leave: LeaveInfo) {
    const data =
    {
      id: leave.leaveId,
      EmployeeId: leave.employeeId,
      EmpId:this.employeeId
    };
    this.empService.updateLeaveApproveStatus(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
         this.notificationService.Success({ message: 'Leave accepted successfully', title: '' });
         this.GetList();
          const index = this.employeeLeaveInfo.findIndex(x => x.id == this.response.responseData.id);
          if (index > -1) {
            this.employeeLeaveInfo[index].isApproved = true;
          }

          break;

        default:
          break;
      }
    });
  }

  showModalRejectLeave(leavereject: LeaveInfo) {
    this.leaveId = leavereject.leaveId;
    this.empId = leavereject.employeeId;

  }

  rejectLeave() {
    if (this.rForm.valid) {
      const data = {
        Id: this.leaveId,
        remark: this.rForm.controls['remark'].value,
        EmployeeId: this.empId,
        EmpId:this.employeeId
      };
      this.empService.updateLeaveRejectStatus(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.notificationService.Success({ message: 'Leave rejected successfully', title: '' });
            const index = this.employeeLeaveInfo.findIndex(x => x.id == this.response.responseData.id);
            if (index > -1) {
              this.employeeLeaveInfo[index].isRejected = true;
            }
            this.GetList();
            this.cancel.nativeElement.click();
            this.formDirective.resetForm();
            this.rForm.reset();
            break;

          default:
            break;
        }
      });
    } else {
      this.notificationService.Warning({ message: 'Remark is missing', title: '' });
    }
  }

  cancelForm() {
    this.rForm.reset();
    this.formDirective.resetForm();
  }
  
}
export interface LeaveElementInfo {
  Id?: number
}