import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { EmpServiceService } from '../../services/emp-service.service';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { EmployeeLeave } from '../employee-innerpages/view-model/employee-leave';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';


@Component({
  selector: 'app-leave-list',
  templateUrl: './leave-list.component.html',
  styleUrls: ['./leave-list.component.scss']
})
export class LeaveListComponent implements OnInit {

  getErrorMessage: 'Please Enter Value';
  LeaveData: LeaveElement[];
  paging: Paging = {};
  LeaveList = [];
  employeeLeaveInfo: EmployeeLeave[];
  rForm: FormGroup;
  totalCount: number;
  deleteLeaveId: number;
  leaveId: any;
  searchByName = null;
  searchByStatus = null;
  LeaveInfoModel: LeaveElementInfo = {};
  response: ResponseModel = {};
  orderBy: number;
  orderColumn: number;
  displayedColumns: string[] = ['sr', 'employeeName', 'leaveTypeName', 'fromDate', 'toDate', 'appliedDate', 'leavereason', 'leavestatus', 'action'];
  dataSource = new MatTableDataSource(this.LeaveList);
  tempLeaveInfo: EmployeeLeave;
  rejectForm: FormGroup;
  @ViewChild('btnCancel') cancelReject: ElementRef;
  @ViewChild('formDirective') formDirectiveReject: NgForm;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('btnEditEducationCancel') editCancel: ElementRef;
  @ViewChild('empName') employeeName: ElementRef;
  @ViewChild('status') Status: ElementRef;
  constructor(private fb: FormBuilder, private empService: EmpServiceService, private loaderService: LoaderService, private router: Router, private notification: NotificationService, private activatedRoute: ActivatedRoute,
    private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }
  Leavestatus: string;
  ngOnInit(): void {
    this.getAllEmployeeLeaveList();
    this.dataSource.sort = this.sort;
    this.createEditForm(null);
    this.createRejectForm();
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getAllEmployeeLeaveList())
        )
        .subscribe();
    }, 2000);

  }
  createForm() {
    this.rForm = this.fb.group({
      addapprove: ['', Validators.required],
      adddateFrom: ['', Validators.required],
      adddateTo: ['', Validators.required],
    });
  }
  createRejectForm() {
    this.rejectForm = this.fb.group({
      remark: ['', Validators.required],
    });
  }

  get remark() {
    return this.rejectForm.get('remark');
  }
  Search() {
    this.searchByName = this.employeeName.nativeElement.value;
    this.searchByStatus = "";
    this.getAllEmployeeLeaveList();
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'employeeName':
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
      case 'leavestatus':
        this.orderColumn = 5;
        break;
      case 'createdDate':
        this.orderColumn = 6;
        break;
      default:
        break;
    }
  }
  getAllEmployeeLeaveList() {
    this.getSortingOrder();
    const data = {
      SearchTextByName: this.searchByName,
      SearchTextBystatus: this.searchByStatus,
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };

    this.empService.getAllEmployeeLeaveList(data).subscribe((res: any) => {
      this.totalCount = res.total;
      if (res) {
        let Leavearray = [];
        if (res.responseData != null) {
          this.LeaveList = res.responseData;

          this.LeaveList.forEach(function (value) {

            let Commdata = {
              Id: value.requireComp['id'],
              employeeId: value.requireComp['employeeId'],
              employeeName:
                value['firstName'] +
                ((value['middleName'] === undefined || value['middleName'] === null) ? '' : ' ' + value['middleName'])
                +
                ((value['lastName'] === undefined || value['lastName'] === null) ? '' : ' ' + value['lastName']),
              fromDate: value.requireComp['dateFrom'],
              toDate: value.requireComp['dateTo'],
              leaveTypeName: value['leaveTypeName'],
              leavereason: value.requireComp['reasonOfLeave'],
              createdDate: value['createdDate'],
              leaveStatus: value['leaveStatus'],
              ACTION: '',
              isApproved: value['isApproved'],
              isRejected: value['isRejected']
            }
            Leavearray.push(Commdata);
          })
          this.LeaveData = Leavearray;
          this.dataSource.data = this.LeaveData
          this.loaderService.stop();
        }
        else {
          this.dataSource.data = Leavearray;
          this.loaderService.stop();
          // this.noData = this.dataSource.connect().pipe(map(data => data.length === 0));
        }
      }
      else {

      } return this.dataSource.data;

    })
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getAllEmployeeLeaveList();
  }

  DeleteModal(leaveID, _e) {

    this.deleteLeaveId = leaveID;
  }

  DeleteLeaveInfo(event) {
    this.LeaveInfoModel.Id = this.deleteLeaveId;
    this.empService.DeleteLeaveDetails(this.LeaveInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notification.Success({ message: data.message, title: null });
        this.getAllEmployeeLeaveList();
      }
      else {

        this.notification.Success({ message: 'Some error occured', title: null });
      }

    })
  }
  updateLeave() {
    if (this.rForm.valid) {
      const data = {
        'Id': this.leaveId,
        'Approve': this.rForm.get('approve').value == '1' ? true : false,
        'DateFrom': this.rForm.get('dateFrom').value,
        'DateTo': this.rForm.get('dateTo').value,
      };

      this.empService.UpdateEmpLeave(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.notificationService.Success({ message: this.response.message, title: null });
            this.editCancel.nativeElement.click();
            this.getAllEmployeeLeaveList();
            break;

          default:
            break;
        }
      });
    }
  }
  // openEditModal(elem) {
  //   debugger;
  //   // console.log(elem);
  //   document.getElementById("openEditModalButton").click();
  //   this.leaveId=elem.Id;
  //   this.rForm.patchValue({
  //     approve: elem.status=='Approve'?'1':'0',
  //     dateFrom: elem.fromDate,
  //     dateTo: elem.toDate,

  //   });

  //   // this.rForm.controls['id'].setValue(this.employeeEducationInfo[index].id);
  // }
  createEditForm(index) {
    if (index != null) {
      this.rForm = this.fb.group({
        // approve: [this.employeeLeaveInfo[index].approve == true ? '1' : '0', Validators.required],
        dateFrom: [this.employeeLeaveInfo[index].dateFrom, Validators.required],
        dateTo: [this.employeeLeaveInfo[index].dateTo, Validators.required],

      });
    }
    else {
      this.rForm = this.fb.group({
        approve: [null, Validators.required],
        dateFrom: [null, Validators.required],
        dateTo: [null, Validators.required],

      });
    }
  }
  OpenEditmodal(employeeId, LeaveId, _e) {
    this.router.navigate(['/employee/leave-details'], { queryParams: { Id: employeeId, EId: LeaveId } });
  }

  showModalRejectShift(leave: EmployeeLeave) {
    this.tempLeaveInfo = leave;
  }

  acceptShift(leave: EmployeeLeave) {
    const data = {
      id: leave.Id
    }
    this.empService.ApproveLeaveById(data).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.getAllEmployeeLeaveList();
      }
      else {

        this.notificationService.Success({ message: 'Some error occured', title: null });
      }

    })
  }

  rejectShift() {
    if (this.rejectForm.valid) {
      const data = {
        id: this.tempLeaveInfo.Id,
        remark: this.rejectForm.controls['remark'].value,
      };
      this.empService.RejectLeaveById(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.notificationService.Success({ message: 'Leave rejected successfully', title: '' });
            this.cancelReject.nativeElement.click();
            this.formDirectiveReject.resetForm();
            this.rejectForm.reset();
            this.getAllEmployeeLeaveList();
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
    this.rejectForm.reset();
    this.formDirectiveReject.resetForm();
  }

}

export interface LeaveElement {
  Id?: number
  employeeId?: number,
  employeeName: string;
  leaveTypeName: string;
  fromDate: string;
  toDate: string;
  leavereason: string;
  status: string;
  leaveStatus: string;
}
export interface LeaveElementInfo {
  Id?: number
}