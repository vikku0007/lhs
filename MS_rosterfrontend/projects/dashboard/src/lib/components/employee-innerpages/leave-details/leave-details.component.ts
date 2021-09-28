import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, NgForm, FormControl } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { EmpServiceService } from '../../../services/emp-service.service';
import { EmployeeLeave } from '../view-model/employee-leave';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { CommonService } from 'projects/lhs-service/src/projects';
import * as moment from 'moment';
import { merge, Subject, ReplaySubject } from 'rxjs';
import { tap, takeUntil } from 'rxjs/operators';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';

export interface LeaveElement {
  leaveDate: string;
  leaveStatus: string;
}

const LEAVE_DATA: LeaveElement[] = [
  { leaveDate: 'Thursday, 28 May - Friday, 29 May', leaveStatus: 'Approved' },
  { leaveDate: 'Wednesday, 06 May - Thursday, 14 May', leaveStatus: 'Approved' },
];

@Component({
  selector: 'emp-leave-details',
  templateUrl: './leave-details.component.html',
  styleUrls: ['./leave-details.component.scss'],
  providers: [
    {
      provide: DateAdapter, useClass: AppDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
  ]
})
export class LeaveDetailsComponent implements OnInit {
  employeeLeaveInfo: EmployeeLeave[];
  empModel: EmployeeLeave = {};
  rForm: FormGroup;
  rForm1: FormGroup;
  rejectForm: FormGroup;
  totalCount: number;
  paging: Paging = {};
  getErrorMessage: 'Please Enter Value';
  employeeId: any;
  response: ResponseModel = {};
  leaveId: any;
  responseModel: ResponseModel = {};
  displayedColumnsLeave: string[] = ['leaveTypeName', 'leaveDateFrom', 'leaveDateTo', 'leavereason', 'appliedDate', 'action'];
  //dataSourceLeave = new MatTableDataSource(LEAVE_DATA);
  dataSourceLeave: any;
  employeePrimaryInfo: {};
  LeaveInfoModel: LeaveElementInfo = {};
  deleteLeaveId: number;
  EditList: LeaveElementInfo[] = [];
  LeaveListdata = [];
  orderBy: number;
  orderColumn: number;
  @ViewChild('btnAddEducationCancel') cancel: ElementRef;
  @ViewChild('btnEditEducationCancel') editCancel: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator; z
  LeaveTypelist: any;
  EditId: number;
  todayDate = new Date();
  selectedType: any;
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild('Select') select: MatSelect;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('btnCancel') cancelReject: ElementRef;
  @ViewChild('formDirective') formDirectiveReject: NgForm;
  tempLeaveInfo: EmployeeLeave;

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private loaderService: LoaderService, private commonService: CommonService, private empService: EmpServiceService, private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      // tslint:disable-next-line: radix
      this.employeeId = parseInt(params.Id);
      this.EditId = parseInt(params.EId);
    });
    this.createForm();
    this.createEditForm(null);
    this.GetList();
    this.searchleavetype();
    this.searchEditleavetype();
    if (this.employeeId > 0) {
      this.getEmployeeDetails();
    }
    this.getLeaveType();
    if (this.EditId > 0) {
      this.getLeaveById();
    }
    this.createRejectForm();

  }
  createRejectForm() {
    this.rejectForm = this.fb.group({
      remark: ['', Validators.required],
    });
  }

  get remark() {
    return this.rejectForm.get('remark');
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

      case 'leaveTypeName':
        this.orderColumn = 0;
        break;
      case 'leaveDateFrom':
        this.orderColumn = 1;
        break;
      case 'leaveDateTo':
        this.orderColumn = 2;
        break;
      case 'leavereason':
        this.orderColumn = 3;
        break;
      case 'createdDate':
        this.orderColumn = 4;
        break;
      default:
        break;
    }
  }
  getLeaveById() {
    const data = {
      Id: Number(this.EditId)
    };
    this.empService.getLeaveById(data).subscribe(res => {
      this.response = res;
      this.EditList = this.response.responseData;
      document.getElementById("openEditModalButton").click();
      this.leaveId = this.EditList[0]['id'];
      this.rForm.controls['leavetype'].setValue(this.EditList[0]['leaveType']);
      this.rForm.controls['dateFrom'].setValue(this.EditList[0]['dateFrom']);
      this.rForm.controls['dateTo'].setValue(this.EditList[0]['dateTo']);
      this.rForm.controls['leavereason'].setValue(this.EditList[0]['reasonOfLeave']);
    })
  }
  searchleavetype() {
    this.control.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterleavetype();
      });
  }


  private filterleavetype() {
    if (!this.LeaveTypelist) {
      return;
    }
    let search = this.control.value;
    if (!search) {
      this.filteredRecords.next(this.LeaveTypelist.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
      this.filteredRecords.next(
        this.LeaveTypelist.filter(ServiceType => ServiceType.codeDescription.toLowerCase().indexOf(search) > -1)
      );
    }
  }
  searchEditleavetype() {
    this.searchcontrol.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterEditleavetype();
      });
  }


  private filterEditleavetype() {
    if (!this.LeaveTypelist) {
      return;
    }
    let search = this.searchcontrol.value;
    if (!search) {
      this.filteredRecords.next(this.LeaveTypelist.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
      this.filteredRecords.next(
        this.LeaveTypelist.filter(ServiceType => ServiceType.codeDescription.toLowerCase().indexOf(search) > -1)
      );
    }
  }
  getLeaveType() {
    this.commonService.getLeaveType().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.LeaveTypelist = this.responseModel.responseData || [];
        this.filteredRecords.next(this.LeaveTypelist.slice());

      } else {

      }
    }));
  }

  createForm() {
    this.rForm1 = this.fb.group({
      addleavetype: ['', Validators.required],
      adddateFrom: ['', Validators.required],
      adddateTo: ['', Validators.required],
      addleavereason: ['', Validators.required],
    });
  }
  openEditModal(elem) {
    document.getElementById("openEditModalButton").click();
    const index = this.employeeLeaveInfo.findIndex(x => x.id == elem.id);
    this.createEditForm(index);
    this.leaveId = this.employeeLeaveInfo[index].id;
    // this.rForm.controls['id'].setValue(this.employeeEducationInfo[index].id);
  }
  openModal(open: boolean) {
    document.getElementById("openModalButton").click();
  }
  createEditForm(index) {
    if (index != null) {
      this.rForm = this.fb.group({
        leavetype: [this.employeeLeaveInfo[index].leaveType, Validators.required],
        dateFrom: [this.employeeLeaveInfo[index].dateFrom, Validators.required],
        dateTo: [this.employeeLeaveInfo[index].dateTo, Validators.required],
        leavereason: [this.employeeLeaveInfo[index].reasonOfLeave, Validators.required],

      });
    }
    else {
      this.rForm = this.fb.group({
        leavetype: ['', Validators.required],
        dateFrom: ['', Validators.required],
        dateTo: ['', Validators.required],
        leavereason: ['', Validators.required],

      });
    }
  }
  cancelModal() {
    this.rForm1.reset();
    this.formDirective.resetForm();
  }
  get leavetype() {
    return this.rForm.get('leavetype');
  }
  get addapprove() {
    return this.rForm1.get('addapprove');
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
    this.empService.getEmpLeaveList(data).subscribe(res => {
      console.log("res accident", res);
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
  addLeave() {
    if (this.rForm1.valid) {
      const data = {
        'EmployeeId': this.employeeId,
        'LeaveType': this.rForm1.get('addleavetype').value,
        'DateFrom': moment(this.rForm1.get('adddateFrom').value).format('YYYY-MM-DD'),
        'DateTo': moment(this.rForm1.get('adddateTo').value).format('YYYY-MM-DD'),
        'ReasonOfLeave': this.rForm1.get('addleavereason').value,
      };

      this.empService.AddEmpLeave(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.empModel = this.response.responseData;
            if (!this.employeeLeaveInfo) {
              this.employeeLeaveInfo = [];
            }
            this.employeeLeaveInfo.push(this.empModel);
            this.dataSourceLeave = new MatTableDataSource(this.employeeLeaveInfo);
            this.cancel.nativeElement.click();
            this.notificationService.Success({ message: 'Leave Added successfully', title: null });
            this.rForm1.reset();
            this.formDirective.resetForm();
            this.GetList();
            break;

          default:
            this.notificationService.Warning({ message: this.response.message, title: null });
            break;
        }
      });
    }

  }
  updateLeave() {
    if (this.rForm.valid) {
      const data = {
        'Id': this.leaveId,
        'EmployeeId': this.employeeId,
        'LeaveType': this.rForm.get('leavetype').value,
        'DateFrom': moment(this.rForm.get('dateFrom').value).format('YYYY-MM-DD'),
        'DateTo': moment(this.rForm.get('dateTo').value).format('YYYY-MM-DD'),
        'ReasonOfLeave': this.rForm.get('leavereason').value,
      };

      this.empService.UpdateEmpLeave(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.editCancel.nativeElement.click();
            this.GetList();
            this.notificationService.Success({ message: 'Leave updated successfully', title: null });
            break;
          default:
            this.notificationService.Warning({ message: this.response.message, title: null });
            break;
        }
      });
    }
  }
  getEmployeeDetails() {

    this.empService.getEmployeeDetails(this.employeeId).subscribe(res => {

      this.responseModel = res;
      console.log('response', this.responseModel);
      if (this.responseModel.status > 0) {


      }
      else {

      }
    });
  }
  DeleteModal(leaveID, _e) {

    this.deleteLeaveId = leaveID;
  }

  DeleteLeaveInfo(event) {
    this.LeaveInfoModel.Id = this.deleteLeaveId;
    this.empService.DeleteLeaveDetails(this.LeaveInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.GetList();
      }
      else {

        this.notificationService.Success({ message: 'Some error occured', title: null });
      }

    })
  }

  showModalRejectShift(leave: EmployeeLeave) {
    this.tempLeaveInfo = leave;
  }

  acceptShift(leave: EmployeeLeave) {
    const data = {
      id: leave.id
    }
    this.empService.ApproveLeaveById(data).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.GetList();
      }
      else {

        this.notificationService.Success({ message: 'Some error occured', title: null });
      }

    })
  }

  rejectShift() {
    if (this.rejectForm.valid) {
      const data = {
        id: this.tempLeaveInfo.id,
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
            this.GetList();
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
export interface LeaveElementInfo {
  Id?: number
}