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
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { Router } from '@angular/router';
@Component({
  selector: 'lib-payroll-in-myob',
  templateUrl: './payroll-in-myob.component.html',
  styleUrls: ['./payroll-in-myob.component.scss'],
  providers: [
    {
      provide: DateAdapter, useClass: AppDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
  ]
})

export class PayrollInMyobComponent implements OnInit {

  displayedColumns: string[] = ['sr', 'lastname', 'firstname', 'payrollCategory', 'job', 'customerLastName', 'customerFirstName', 'notes', 'date',
   'units','empCardId', 'empRecordId', 'startStopTime', 'custCardId', 'custRecordId'];
  empDetailModel: [];
  dataSource: any;
  dataSourceHourDetail: any;
  totalCount = 0;
  ServiceData :any;
  ReportedTosearch: any;
  public searchcontrol: FormControl = new FormControl();
  responseModel: ResponseModel = {};
  response: ResponseModel = {};
  employeeList: any;
  employeeDetailList: any;
  employeeId: Number = 0;
  startdate: string = '';
  enddate: string = '';
  private _onDestroy = new Subject<void>();
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
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
    }, 2000);
  }
  searchEmployee(){
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
  searchservicetype() {
    this.searchcontrol.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterServicetype();
      });
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

  DateChange() {
    this.getPayrollEmployeeList();
  }

  empChange(event: any) {
    this.getPayrollEmployeeList();
  }

  getPayrollEmployeeList() {  
    if (!this.startdate) {
      this.notificationService.Warning({ message: 'please select start date', title: null });
      return;
    }
    if (!this.enddate) {
      this.notificationService.Warning({ message: 'please select end date', title: null });
      return;
    }
    this.payrollService.getEmployeeMyObHoursDetails(Number(this.employeeId), moment(this.startdate).format('YYYY-MM-DD'), moment(this.enddate).format('YYYY-MM-DD'), true).subscribe((res) => {
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
         //Added By DeepakBisht
         this.ReportedTosearch = this.response.responseData || [];
         this.filteredRecords.next(this.employeeList.slice());

      } else {

      }
    }));
  }

  redirectToShiftDetail(shiftId: any) {
    this.cancel.nativeElement.click();
    this.router.navigate(['/roster/view-shift'], { queryParams: { Id: shiftId } });
  }

}
