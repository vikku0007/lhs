import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { MatSort } from '@angular/material/sort';
import { CommonService } from 'projects/lhs-service/src/projects';
import { NotificationService } from 'projects/core/src/projects';
import { PayrollService } from '../../services/payroll.service';
import { FormControl } from '@angular/forms';
import * as moment from 'moment';
import { takeUntil } from 'rxjs/operators';
import { Subject, ReplaySubject } from 'rxjs';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { MAT_DATE_FORMATS, DateAdapter } from '@angular/material/core';
import { Router } from '@angular/router';

@Component({
  selector: 'lib-employee-rejectedshift-payroll-list',
  templateUrl: './employee-rejectedshift-payroll-list.component.html',
  styleUrls: ['./employee-rejectedshift-payroll-list.component.scss'],
  providers: [
    {
      provide: DateAdapter, useClass: AppDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
  ]
})
export class EmployeeRejectedshiftPayrollListComponent implements OnInit {

  displayedColumns: string[] = ['sr', 'name', 'admin', 'cleaning', 'weekday', 'sat', 'sun', 'ph', 'sleepover', 'anwd', 'ansat', 'ansun', 'anp', 'action'];
  displayedHourDetailColumns: string[] = ['sr', 'name', 'client', 'date', 'time', 'admin', 'cleaning', 'weekday', 'sat', 'sun', 'ph', 'sleepover', 'anwd', 'ansat', 'ansun', 'anp', 'action'];
  empDetailModel: [];
  ReportedTosearch: any;
  dataSource: any;
  public searchcontrol: FormControl = new FormControl();
  dataSourceHourDetail: any;
  totalCount = 0;
  ServiceData :any;
  private _onDestroy = new Subject<void>();
  responseModel: ResponseModel = {};
  response: ResponseModel = {};
  employeeList: any;
  employeeDetailList: any;
  employeeId: Number = 0;
  startdate: string = '';
  enddate: string = '';
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredRecordsService: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('btnCancel') cancel: ElementRef;

  constructor(private commonService: CommonService, private notificationService: NotificationService,
    private payrollService: PayrollService, private router: Router) {
    let startDate = new Date();
    startDate.setDate(startDate.getDate() - 7);
    let endDate = new Date();
    if (this.startdate) {
      startDate = new Date(this.startdate);
    }
    if (this.enddate) {
      endDate = new Date(this.enddate);
    }
    this.startdate = moment(startDate).format('YYYY-MM-DD');
    this.enddate = moment(endDate).format('YYYY-MM-DD');
  }
  // tslint:disable-next-line: ban-types

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.empDetailModel);
    this.dataSourceHourDetail = new MatTableDataSource(this.employeeDetailList);
    this.getEmployeeList();
    this.getPayrollEmployeeList();
    this.searchEmployee();
    this.searchservicetype();
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      // tslint:disable-next-line: no-unused-expression
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;

    }, 2000);

  }
  DateChange() {

    this.getPayrollEmployeeList();
  }
  empChange(event: any) {
    this.getPayrollEmployeeList();
  }
  openEditDetails(element) {
    document.getElementById("openEditModalButton").click();
    this.getPayrollEmployeeDetail(element.employeeId);
  }
  getPayrollEmployeeDetail(employeeid) {
    if (!this.startdate) {
      this.notificationService.Warning({ message: 'please select start date', title: null });
    }
    if (!this.enddate) {
      this.notificationService.Warning({ message: 'please select end date', title: null });
    }


    this.payrollService.getEmployeeHoursDetails(Number(employeeid), moment(this.startdate).format('YYYY-MM-DD'), moment(this.enddate).format('YYYY-MM-DD'), false).subscribe((res) => {
      this.responseModel = res;
      this.totalCount = this.responseModel.total;
      switch (this.responseModel.status) {
        case 1:
          this.employeeDetailList = this.responseModel.responseData;
          this.dataSourceHourDetail = new MatTableDataSource(this.employeeDetailList);
          break;
        case 0:
          this.notificationService.Warning({ message: this.responseModel.message, title: null });
          break;
        default:
          // this.notificationService.Error({ message: 'Some error occured while fetching employee listing', title: null });
          break;
      }
    })
  }
  getPayrollEmployeeList() {
    debugger;

    if (!this.startdate) {
      this.notificationService.Warning({ message: 'please select start date', title: null });
      return;
    }
    if (!this.enddate) {
      this.notificationService.Warning({ message: 'please select end date', title: null });
      return;
    }
    this.payrollService.getEmployeeHoursList(Number(this.employeeId), moment(this.startdate).format('YYYY-MM-DD'), moment(this.enddate).format('YYYY-MM-DD'), false).subscribe((res) => {
      this.responseModel = res;
      this.totalCount = this.responseModel.total;
      switch (this.responseModel.status) {
        case 1:
          this.empDetailModel = this.responseModel.responseData;
          this.dataSource = new MatTableDataSource(this.empDetailModel);
          break;
        case 0:
          this.notificationService.Warning({ message: this.responseModel.message, title: null });
          break;
        default:
          // this.notificationService.Error({ message: 'Some error occured while fetching employee listing', title: null });
          break;
      }
    })
  }
  getEmployeeList() {
    this.commonService.getallemployee().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.employeeList = this.responseModel.responseData || [];
        //Added By Deepak bisht
        this.ReportedTosearch = this.response.responseData || [];
        this.filteredRecords.next(this.employeeList.slice());

      } else {

      }
    }));
  }
  searchservicetype() {
    this.searchcontrol.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterServicetype();
      });
  }
  searchEmployee() {
    this.searchcontrol.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterEmployee();
      });
  }
  private filterEmployee(){
    if (!this.ReportedTosearch) {
      return;
    }
    let search = this.searchcontrol.value;
    if (!search) {
      this.filteredRecords.next(this.employeeList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
      this.filteredRecords.next(
        this.employeeList.filter(department => department.fullName.toLowerCase().indexOf(search) > -1)
      );
    }
  }

  private filterServicetype(){
    if (!this.ServiceData) {
      return;
    }
    let search = this.searchcontrol.value;
    if (!search) {
      this.filteredRecordsService.next(this.ServiceData.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
      this.filteredRecordsService.next(
        this.ServiceData.filter(ServiceType => ServiceType.supportItemName.toLowerCase().indexOf(search) > -1)
      );
    }
  }
  UpdateStatus(elem) {
    debugger;
    const data = {

      EmployeeTrackerId: elem.shiftTrackerId
    }
    this.payrollService.ApproveHours(data).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.dataSourceHourDetail.data = this.dataSourceHourDetail.data.filter(i => i !== elem)
          this.notificationService.Success({ message: 'Shift Hours Approved', title: '' })

          break;
        case 0:
          this.notificationService.Warning({ message: this.responseModel.message, title: '' });
        default:
          break;
      }
    });
  }

  redirectToShiftDetail(shiftId: any) {
    this.cancel.nativeElement.click();
    this.router.navigate(['/roster/view-shift'], { queryParams: { Id: shiftId } });
  }

}
