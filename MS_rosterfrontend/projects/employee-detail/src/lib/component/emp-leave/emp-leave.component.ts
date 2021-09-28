import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { CommonService } from 'projects/lhs-service/src/projects';
import * as moment from 'moment';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { EmployeeLeave } from '../../view-models/employee-leave';
import { EmpDetailService } from '../../services/emp-detail.service';

@Component({
  selector: 'lib-emp-leave',
  templateUrl: './emp-leave.component.html',
  styleUrls: ['./emp-leave.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})
export class EmpLeaveComponent implements OnInit {
  employeeLeaveInfo: EmployeeLeave[];
  empModel: EmployeeLeave = {};
  rForm: FormGroup;
  rForm1: FormGroup;
  totalCount: number;
  paging: Paging = {};
  getErrorMessage: 'Please Enter Value';
  employeeId: any;
  response: ResponseModel = {};
  leaveId: any;
  responseModel: ResponseModel = {};
  displayedColumnsLeave: string[] = ['leaveTypeName','leaveDateFrom','leaveDateTo','appliedDate','leavereason' ,'action'];
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
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  LeaveTypelist: any;
  EditId: number;
  todayDate = new Date();
  selectedType: any;
  
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('formDirective') private formDirective: NgForm;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, 
    private loaderService: LoaderService,private router: Router,private commonService:CommonService, private empService: EmpDetailService, private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.employeeId = Number(params.params.id);
    });
    this.createForm();
    this.createEditForm(null);
    this.GetList();
    this.getLeaveType();
    
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
      case 'fromDate':
        this.orderColumn = 1;
        break;
        case 'toDate':
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
  
  getLeaveType(){
    this.commonService.getLeaveType().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.LeaveTypelist=this.responseModel.responseData||[];
       }else{

      }
    }));
  }
 
  createForm() {
    this.rForm1 = this.fb.group({
      addleavetype: ['', Validators.required],
      adddateFrom: ['', Validators.required],
      adddateTo: ['', Validators.required],
      addleavereason:['', Validators.required],
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
        leavereason:['', Validators.required],

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
  openEmployeeDetail() {
    this.router.navigate(['employeedetail/employeedetails', this.employeeId]);
  }
}
export interface LeaveElementInfo {
  Id?: number
}