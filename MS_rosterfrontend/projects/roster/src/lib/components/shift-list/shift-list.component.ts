import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DataSource } from '@angular/cdk/table';
import { NotificationService } from 'projects/core/src/projects';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { RosterService } from '../../services/roster.service';
import { ShiftInfoViewModel } from '../../viewmodel/roster-shift-info-viewModel';
import { PageEvent } from '@angular/material/paginator';
import { CommonService, LoaderService } from 'projects/lhs-service/src/projects';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { takeUntil } from 'rxjs/operators';
import { Subject, ReplaySubject } from 'rxjs';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';

export interface RequiredComplianceElement {
  date: string;
  time: string;
  client: string;
  staff: string;
  location: string;
  allowance: string;
  status: string;
}

const REQUIRED_COMPLIANCE_DATA: RequiredComplianceElement[] = [
  { date: '1-May-2020', time: '06:00pm - 07:00am', client: 'Mario Speedwagon', staff: 'Peter Pants', location: '3 Rock Daisy Drive', allowance: 'Sleepover', status: 'Booked' },
  { date: '5-May-2020', time: '06:00pm - 07:00am', client: 'Petey Cruiser', staff: 'Bud Wiser', location: '5 Rock Daisy Drive', allowance: 'Sleepover', status: 'Pending' },
  { date: '15-May-2020', time: '06:00pm - 07:00am', client: 'Anna Sthesia', staff: 'Bill Yerds', location: '8 Rock Daisy Drive', allowance: 'Sleepover', status: 'Pending' },
  { date: '18-May-2020', time: '06:00pm - 07:00am', client: 'Paul Molive', staff: 'Sal Vidge', location: '11 Rock Daisy Drive', allowance: 'Sleepover', status: 'Booked' },
  { date: '20-May-2020', time: '06:00pm - 07:00am', client: 'Gail Forcewind', staff: 'Manuel Labor', location: '2 Rock Daisy Drive', allowance: 'Sleepover', status: 'Booked' },
  { date: '24-May-2020', time: '06:00pm - 07:00am', client: 'Mario Speedwagon', staff: 'Peter Pants', location: '9 Rock Daisy Drive', allowance: 'Sleepover', status: 'Booked' },
  { date: '30-May-2020', time: '06:00pm - 07:00am', client: 'Petey Cruiser', staff: 'Bud Wiser', location: '15 Rock Daisy Drive', allowance: 'Sleepover', status: 'Pending' },
];
@Component({
  selector: 'lib-shift-list',
  templateUrl: './shift-list.component.html',
  styleUrls: ['./shift-list.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})
export class ShiftListComponent implements OnInit {
  displayedColumnsRequired: string[] = ['description', 'date', 'time', 'duration', 'client', 'staff', 'location', 'status', 'action'];
  //dataSourceRequired = new MatTableDataSource(REQUIRED_COMPLIANCE_DATA);
  shiftList: ShiftInfoViewModel[];
  responseModel: ResponseModel = {};
  rForm: FormGroup;
  LocationList: [];
  EmployeeNameList: [];
  clientNameList: [];
  shiftStatusList: [];
  dataSource: any;
  totalCount: number;
  clientToList: any;
  paging: Paging = {};
  searchByEmpName = 0;
  searchByClientName = 0;
  searchBylocation = 0;
  deletelId = 0;
  ReportedTosearch: any;
  private _onDestroy = new Subject<void>();
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredRecordsclient: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  show: boolean = false;

  constructor(private fb: FormBuilder, private notificationService: NotificationService,
    private rosterService: RosterService, private commonService: CommonService, private loaderService: LoaderService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.createForm();
    this.bindDropDown();
    this.getEmployeeList();
    this.searchClient();
    this.searchEmployee();
  }
  ClearFilter() {
    this.rForm.reset();
  }
  bindDropDown() {
    this.getEmployeeDropdownList();
    this.getAllClientNameList();
    this.getLocation();
    this.getShiftStatusList();
  }
  createForm() {
    this.rForm = this.fb.group({
      clientId: [null, Validators.nullValidator],
      shiftType: [null, Validators.nullValidator],
      employeeId: [null, Validators.nullValidator],
      shiftStatus: [null, Validators.nullValidator],
      locationId: [null, Validators.nullValidator],
      otherLocation: [null, Validators.nullValidator],
      startDate: [null, Validators.nullValidator],
      endDate: [null, Validators.nullValidator]
    });
  }

  searchClient() {
    this.searchcontrol.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterClient();
      });
  }


  private filterClient() {
    if (!this.clientToList) {
      return;
    }
    let search = this.searchcontrol.value;
    if (!search) {
      this.filteredRecordsclient.next(this.clientToList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
      this.filteredRecordsclient.next(
        this.clientToList.filter(department => department.fullName.toLowerCase().indexOf(search) > -1)
      );
    }
  }
  searchEmployee() {
    this.control.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterEmployee();
      });
  }
  private filterEmployee() {
    if (!this.ReportedTosearch) {
      return;
    }
    let search = this.control.value;
    if (!search) {
      this.filteredRecords.next(this.ReportedTosearch.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
      this.filteredRecords.next(
        this.ReportedTosearch.filter(department => department.fullName.toLowerCase().indexOf(search) > -1)
      );
    }
  }

  get clientId() {
    return this.rForm.get('clientId');
  }
  get shiftType() {
    return this.rForm.get('shiftType');
  }
  get employeeId() {
    return this.rForm.get('employeeId');
  }
  get shiftStatus() {
    return this.rForm.get('shiftStatus');
  }

  get otherLocation() {
    return this.rForm.get('otherLocation');
  }
  get locationId() {
    return this.rForm.get('locationId');
  }

  get startDate() {
    return this.rForm.get('startDate');
  }

  get endDate() {
    return this.rForm.get('endDate');
  }

  toggleFilters() {
    this.show = !this.show;
  }

  getEmployeeList() {
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      SearchByEmpName: this.employeeId.value ? this.employeeId.value : 0,
      SearchByClientName: this.clientId.value ? this.clientId.value : 0,
      SearchTextBylocation: this.locationId.value ? this.locationId.value : 0,
      SearchTextByStatus: this.shiftStatus.value ? this.shiftStatus.value : 0,
      SearchTextByManualAddress: this.otherLocation.value,
      SearchTextByShiftType: 0,
      SearchByStartDate: this.startDate.value,
      SearchByEndDate: this.endDate.value
    };
    this.loaderService.start();
    this.rosterService.GetShiftList(data).subscribe((res) => {
      this.loaderService.stop();
      this.responseModel = res;
      this.totalCount = this.responseModel.total;
      switch (this.responseModel.status) {
        case 1:
          this.shiftList = this.responseModel.responseData;
          this.dataSource = new MatTableDataSource(this.shiftList);
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
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getEmployeeList();
  }
  getLocation() {
    this.commonService.getLocation().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.LocationList = this.responseModel.responseData || [];

      } else {
        this.notificationService.Error({ message: 'Something went wrong! location not found', title: null });
      }
    }));
  }
  getEmployeeDropdownList() {
    this.rosterService.GetEmployeeList().subscribe((res: any) => {
      this.responseModel = res;
      this.EmployeeNameList = this.responseModel.responseData || []
      this.ReportedTosearch = this.responseModel.responseData || [];
      this.filteredRecords.next(this.ReportedTosearch.slice());

    });
  }
  getAllClientNameList() {
    this.commonService.getAllClientNameList().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.clientNameList = this.responseModel.responseData || [];
        this.clientToList = this.responseModel.responseData || [];
        this.filteredRecordsclient.next(this.clientNameList.slice());

      } else {
        this.notificationService.Error({ message: 'Something went wrong! location not found', title: null });
      }
    }));
  }
  getShiftStatusList() {
    this.commonService.getShiftStatusList().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.shiftStatusList = this.responseModel.responseData || [];

      } else {
        this.notificationService.Error({ message: 'Something went wrong! location not found', title: null });
      }
    }));
  }
  DeleteModal(EmpID, _e) {

    this.deletelId = EmpID;
  }
  deleteShift(event) {
    this.rosterService.DeleteShiftInfo(this.deletelId).subscribe((res => {
      if (res) {
        this.getEmployeeList();
        this.notificationService.Success({ message: 'Shift Deleted Successfully', title: null });


      } else {
        this.notificationService.Error({ message: 'Something went wrong!', title: null });
      }
    }));
  }
  formattedaddress = " ";
  public handleAddressChange(address: any) {
    this.formattedaddress = address.address1
    this.rForm.controls['otherLocation'].setValue(this.formattedaddress);
  }
}
