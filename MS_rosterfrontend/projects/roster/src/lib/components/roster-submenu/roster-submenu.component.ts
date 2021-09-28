import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { CommonService } from 'projects/lhs-service/src/projects';
import { RosterService } from '../../services/roster.service';
import { NotificationService } from 'projects/core/src/projects';
import { FormControl } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { Subject, ReplaySubject } from 'rxjs';
import { ResponseModel } from 'projects/viewmodels/response-model';


@Component({
  selector: 'lib-roster-submenu',
  templateUrl: './roster-submenu.component.html',
  styleUrls: ['./roster-submenu.component.scss']
})

export class RosterSubmenuComponent implements OnInit {
  show: boolean = false;
  rForm: FormGroup;
  LocationList: [];
  EmployeeNameList: [];
  employeeList: any;
  clientNameList: [];
  clientToList: any;
  shiftStatusList: [];
  ReportedTosearch: any;
  ServiceData :any;
  response: ResponseModel = {};
  public searchcontrol: FormControl = new FormControl();
  responseModel: ResponseModel = {};
  @Output() searchData = new EventEmitter<any>();
  LocationTypeList: [];
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredRecordsService: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredRecordsclient: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  isShownOtherLocation: boolean = false;
  isShownLocatiodropdown: boolean = false;
  selectedType: any;
  list: any;

  constructor(private fb: FormBuilder,
    private commonService: CommonService,
    private rosterService: RosterService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.createForm();
    this.bindDropdowns();
    this.searchEmployee();
    this.searchservicetype();
    this.searchClient();
  }

  createForm() {
    this.rForm = this.fb.group({
      clientId: [null, Validators.nullValidator],
      shiftType: [null, Validators.nullValidator],
      employeeId: [null, Validators.nullValidator],
      shiftStatus: [null, Validators.nullValidator],
      locationId: [null, Validators.nullValidator],
      otherLocation: [null, Validators.nullValidator],
      locationtype: [null, Validators.nullValidator]
    });
  }
  searchClient(){
    this.searchcontrol.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterClient();
      });
  }
  private filterClient(){
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

  bindDropdowns() {
    this.getEmployeeDropdownList();
    this.getAllClientNameList();
    // this.getLocation();
    this.getLocationType();
    this.getShiftStatusList();
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

  getEmployeeDropdownList() {
    this.rosterService.GetEmployeeList().subscribe((res: any) => {
      this.responseModel = res;
      //this.EmployeeNameList = this.responseModel.responseData || [];
      this.employeeList = this.responseModel.responseData || [];
      //Added By DeepakBisht
      this.ReportedTosearch = this.response.responseData || [];
      this.filteredRecords.next(this.employeeList.slice());

    });
  }

  getAllClientNameList() {
    this.commonService.getAllClientNameList().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.clientNameList = this.responseModel.responseData || [];
        //added by Deepak Bisht
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

  toggleFilters() {
    this.show = !this.show;
  }

  searchCalendar() {
    if (this.rForm.valid) {
      const data = {
        searchByEmpName: this.rForm.controls['employeeId'].value != null ? this.rForm.controls['employeeId'].value : 0,
        searchByClientName: this.rForm.controls['clientId'].value != null ? this.rForm.controls['clientId'].value : 0,
        searchBylocation: this.rForm.controls['locationId'].value != null ? this.rForm.controls['locationId'].value : 0,
        searchByStatus: this.rForm.controls['shiftStatus'].value != null ? this.rForm.controls['shiftStatus'].value : 0,
        searchBymanualAddress: this.rForm.controls['otherLocation'].value != null ? this.rForm.controls['otherLocation'].value : '',
        searchByShiftType: Number(this.rForm.controls['shiftType'].value)
      };
      this.searchData.emit(data);
    }
  }

  ClearFilter() {
    this.rForm.reset();
  }

  getLocationType() {
    this.commonService.getLocationType().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.LocationTypeList = this.responseModel.responseData || [];

      } else {

      }
    }));
  }

  selectChangeHandler(event: any) {
    this.list = this.LocationTypeList;
    this.selectedType = event;
    if (this.selectedType == 5) {
      this.getLocation()
      this.isShownLocatiodropdown = true;
      this.isShownOtherLocation = false;
      this.rForm.controls['otherlocation'].setValue('');
    }
    else {
      this.isShownLocatiodropdown = false;
      this.isShownOtherLocation = true;
    }
  }
}
