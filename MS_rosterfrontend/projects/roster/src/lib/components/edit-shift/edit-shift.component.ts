import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { FormGroup, NgForm, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { ShiftInfoViewModel } from '../../viewmodel/roster-shift-info-viewModel';
import { ActivatedRoute, Router } from '@angular/router';
import { RosterService } from '../../services/roster.service';
import { CommonService } from 'projects/lhs-service/src/projects';
import { NotificationService } from 'projects/core/src/projects';
import { EmployeeShiftInfoViewModel } from '../../viewmodel/employee-shiftInfo-viewModel';
import * as moment from 'moment';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { MatTableDataSource } from '@angular/material/table';
import { Subject, ReplaySubject } from 'rxjs';
import { MatSelect } from '@angular/material/select';
import { takeUntil } from 'rxjs/operators';
export interface EmpList {
  id?: number;
  fullName?: string;
}
export interface TimeList {
  id?: string;
  text?: string;
}
const ELEMENT_DATA: TimeList[] = [
  { id: '00:00', text: '12:00 AM ' },
  { id: '00:30', text: '12:30 AM ' },
  { id: '01:00', text: '1:00 AM ' },
  { id: '01:30', text: '1:30 AM ' },
  { id: '02:00', text: '2:00 AM ' },
  { id: '02:30', text: '2:30 AM ' },
  { id: '03:00', text: '3:00 AM ' },
  { id: '03:30', text: '3:30 AM ' },
  { id: '04:00', text: '4:00 AM ' },
  { id: '04:30', text: '4:30 AM ' },
  { id: '05:00', text: '5:00 AM ' },
  { id: '05:30', text: '5:30 AM ' },
  { id: '06:00', text: '6:00 AM ' },
  { id: '06:30', text: '6:30 AM ' },
  { id: '07:00', text: '7:00 AM ' },
  { id: '07:30', text: '7:30 AM ' },
  { id: '08:00', text: '8:00 AM ' },
  { id: '08:30', text: '8:30 AM ' },
  { id: '09:00', text: '9:00 AM ' },
  { id: '09:30', text: '9:30 AM ' },
  { id: '10:00', text: '10:00 AM ' },
  { id: '10:30', text: '10:30 AM ' },
  { id: '11:00', text: '11:00 AM ' },
  { id: '11:30', text: '11:30 AM ' },
  { id: '12:00', text: '12:00 PM ' },
  { id: '12:30', text: '12:30 PM ' },
  { id: '13:00', text: '1:00 PM ' },
  { id: '13:30', text: '1:30 PM ' },
  { id: '14:00', text: '2:00 PM ' },
  { id: '14:30', text: '2:30 PM ' },
  { id: '15:00', text: '3:00 PM ' },
  { id: '15:30', text: '3:30 PM ' },
  { id: '16:00', text: '4:00 PM ' },
  { id: '16:30', text: '4:30 PM ' },
  { id: '17:00', text: '5:00 PM ' },
  { id: '17:30', text: '5:30 PM ' },
  { id: '18:00', text: '6:00 PM ' },
  { id: '18:30', text: '6:30 PM ' },
  { id: '19:00', text: '7:00 PM ' },
  { id: '19:30', text: '7:30 PM ' },
  { id: '20:00', text: '8:00 PM ' },
  { id: '20:30', text: '8:30 PM ' },
  { id: '21:00', text: '9:00 PM ' },
  { id: '21:30', text: '9:30 PM ' },
  { id: '22:00', text: '10:00 PM ' },
  { id: '22:30', text: '10:30 PM ' },
  { id: '23:00', text: '11:00 PM ' },
  { id: '23:30', text: '11:30 PM ' },
];
@Component({
  selector: 'lib-edit-shift',
  templateUrl: './edit-shift.component.html',
  styleUrls: ['./edit-shift.component.scss'],
  providers: [
    {
      provide: DateAdapter, useClass: AppDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
  ]
})
export class EditShiftComponent implements OnInit {

  getErrorMessage = "";
  rForm: FormGroup;
  response: ResponseModel = {};
  LocationList: [];
  ReportedToList: EmpList[];
  clientNameList: [];
  shiftStatusList: [];
  LocationTypeList: [];
  shiftId: number = 0;
  shiftInfo: ShiftInfoViewModel = {}
  sleepOverData = [];
  @ViewChild('formDirective') formDirective: NgForm;
  @ViewChild('btnCancel') cancel: ElementRef;
  isShownOtherLocation: boolean = false;
  isShownLocatiodropdown: boolean = false;
  selectedType: any;
  isShownSleepOver: boolean = false;
  ServiceTypeList: any;
  ReportedTosearch: any;
  clientToList: any;
  TimeDropdownList: TimeList[] = ELEMENT_DATA;
  minDate = new Date();
  @ViewChild('btnCancelLoadTemplate') btnCancelLoadTemplate: ElementRef;
  @ViewChild('btnLoadTemplate') btnLoadShiftResult: ElementRef;
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  public filteredRecordsclient: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild('Select') select: MatSelect;
  dataSource: any;
  shiftResponse: any;
  displayedColumns: string[] = ['description', 'date', 'time', 'employee', 'action'];

  constructor(private router: Router, private route: ActivatedRoute, private rosterService: RosterService, private fb: FormBuilder, private commonService: CommonService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.createForm();
    this.bindDropdowns();
    this.route.queryParams.subscribe(params => {
      this.shiftId = parseInt(params['Id']);
      if (this.shiftId && this.shiftId > 0) {
        this.getShiftInfo(this.shiftId);
      }
    });
    this.dataSource = new MatTableDataSource(this.shiftResponse);
    this.searchEmployee();
    this.searchClient();
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
  createForm() {
    this.rForm = this.fb.group({
      clientId: [null, Validators.required],
      description: [null, Validators.required],
      employeeId: [null, Validators.required],
      ratio: [{ value: null, disabled: true }, Validators.nullValidator],
      shifStatus: [null, Validators.required],
      locationId: [null, Validators.nullValidator],
      otherLocation: [null, Validators.nullValidator],
      startDate: [null, Validators.required],
      startTime: [null, Validators.required],
      endDate: [null, Validators.required],
      endTime: [null, Validators.required],
      duration: [{ value: null, disabled: true }, Validators.nullValidator],
      locationtype: [null, Validators.required],
      reminder: ['0', Validators.required],
      isSleepOver: new FormArray([]),
      ServiceType: [null, Validators.nullValidator],
      remark: [null, Validators.required]
    });
  }
  private addCheckboxes() {
    this.sleepOverData.forEach(element => { this.ordersFormArray.push(new FormControl(element.isSleepOver)) });
  }
  get ordersFormArray() {
    return this.rForm.controls.isSleepOver as FormArray;
  }
  get clientId() {
    return this.rForm.get('clientId');
  }
  get description() {
    return this.rForm.get('description');
  }
  get employeeId() {
    return this.rForm.get('employeeId');
  }
  get ratio() {
    return this.rForm.get('ratio');
  }
  get shifStatus() {
    return this.rForm.get('shifStatus');
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
  get startTime() {
    return this.rForm.get('startTime');
  }
  get endDate() {
    return this.rForm.get('endDate');
  }
  get endTime() {
    return this.rForm.get('endTime');
  }
  get duration() {
    return this.rForm.get('duration');
  }

  get locationtype() {
    return this.rForm.get('locationtype');
  }

  get reminder() {
    return this.rForm.get('reminder');
  }
  get ServiceType() {
    return this.rForm.get('ServiceType');
  }
  get remark() {
    return this.rForm.get('remark');
  }
  BindForm() {
    this.rForm.controls.clientId.setValue(this.shiftInfo.clientShiftInfoViewModel.map(x => x.clientId));
    this.rForm.controls.description.setValue(this.shiftInfo.description);
    this.rForm.controls.employeeId.setValue(this.shiftInfo.employeeShiftInfoViewModel.map(x => x.employeeId));
    this.rForm.controls.ratio.setValue(this.shiftInfo.clientCount + ' : ' + this.shiftInfo.employeeCount);
    this.rForm.controls.shifStatus.setValue(this.shiftInfo.statusId);
    this.rForm.controls.ServiceType.setValue(this.shiftInfo.serviceTypeViewModel.map(x => x.serviceTypeId));
    this.rForm.controls.startDate.setValue(this.shiftInfo.startDate);
    this.rForm.controls.startTime.setValue(moment(this.shiftInfo.startDate).format('HH:mm'));
    this.rForm.controls.endDate.setValue(this.shiftInfo.endDate);
    this.rForm.controls.endTime.setValue(moment(this.shiftInfo.endDate).format('HH:mm'));
    this.rForm.controls.duration.setValue(this.shiftInfo.duration + ' hours');

    this.rForm.controls.reminder.setValue(this.shiftInfo.reminder === true ? '1' : '0');
    this.sleepOverData = [];
    this.rForm.controls.isSleepOver = new FormArray([]);
    this.rForm.controls.isSleepOver.setValue([]);
    this.shiftInfo.employeeShiftInfoViewModel.forEach(element => {
      this.sleepOverData.push({ id: element.employeeId, name: element.name, isSleepOver: element.isSleepOver });
      if (element.isSleepOver && this.isShownSleepOver === false) {
        this.isShownSleepOver = true;
      }
    });
    this.addCheckboxes();
    this.rForm.controls.locationtype.setValue(this.shiftInfo.locationType);
    if (this.shiftInfo.otherLocation) {
      this.rForm.controls.otherLocation.setValue(this.shiftInfo.otherLocation);
      this.isShownOtherLocation = true;
      this.isShownLocatiodropdown = false;
    }
    else if (this.shiftInfo.locationId != null && this.shiftInfo.locationId > 0) {
      this.getLocation();
      this.rForm.controls.locationId.setValue(this.shiftInfo.locationId);
      this.isShownOtherLocation = false;
      this.isShownLocatiodropdown = true;
    }
  }

  bindDropdowns() {
    this.getLocationType();
    // this.getLocation();
    this.getEmployeeList();
    this.getAllClientNameList();
    this.getShiftStatusList();
    this.getServiceTypelist();
  }
  ratioChange() {
    let clientLength = this.clientId.value ? this.clientId.value.length : 0;
    let employeeLength = this.employeeId.value ? this.employeeId.value.length : 0;
    this.rForm.controls.ratio.setValue(employeeLength + ' : ' + clientLength);
    this.sleepOverData = [];
    this.rForm.controls.isSleepOver = new FormArray([]);
    this.rForm.controls.isSleepOver.setValue([]);
    if (employeeLength > 0) {
      this.employeeId.value.forEach(element => {
        let empName = this.ReportedToList.filter(x => x.id === Number(element))[0].fullName;
        this.sleepOverData.push({ id: element, name: empName, isSleepOver: false });
      });
      this.addCheckboxes();
    }
  }
  timeChange(event: any) {
    let startDate = new Date(this.startDate.value);
    startDate.setHours(Number(this.startTime.value.split(':')[0]), Number(this.startTime.value.split(':')[1]));
    // let endDate = new Date(this.endDate.value);
    if (startDate != new Date(null) && this.endTime.value) {
      let endtime = new Date(null)
      endtime.setHours(Number(this.endTime.value.split(':')[0]), Number(this.endTime.value.split(':')[1]));
      let endDate = new Date(this.startDate.value);
      let mytime = new Date(this.startDate.value);;
      mytime.setHours(Number(this.startTime.value.split(':')[0]), Number(this.startTime.value.split(':')[1]));
      mytime.setHours(mytime.getHours() + endtime.getHours());
      // tslint:disable-next-line: max-line-length
      let hours = startDate.getHours() > 12 && endtime.getHours() < 12 ? Number(startDate.getHours()) + (24 - Number(startDate.getHours())) + Number(endtime.getHours()) : Number(startDate.getHours()) + (12 - Number(startDate.getHours())) + endtime.getHours() - 12;

      // endDate.setHours(this.startTime.value.toString().split(':')[0],this.startTime.value.toString().split(':')[1]);
      // tslint:disable-next-line: max-line-length
      if (endtime.getHours() === 24 || endtime.getHours() < startDate.getHours() && startDate.getHours() > 0) {
        endDate.setDate(endDate.getDate() + 1);
      }
      endDate.setHours(endtime.getHours(), endtime.getMinutes());
      // this.endDate.setValue(endDate);
      this.duration.setValue(this.timeDiffCalc(endDate, startDate));
      if (startDate.getHours() >= 22 && endtime.getHours() <= 6) {
        this.isShownSleepOver = true;
      }
      else {
        this.isShownSleepOver = false;
      }
    }



  }
  timeDiffCalc(dateFuture, dateNow) {
    let diffInMilliSeconds = Math.abs(dateFuture - dateNow) / 1000;

    // calculate days
    const days = Math.floor(diffInMilliSeconds / 86400);
    diffInMilliSeconds -= days * 86400;
    console.log('calculated days', days);

    // calculate hours
    const hours = Math.floor(diffInMilliSeconds / 3600) % 24;
    diffInMilliSeconds -= hours * 3600;
    console.log('calculated hours', hours);

    // calculate minutes
    const minutes = Math.floor(diffInMilliSeconds / 60) % 60;
    diffInMilliSeconds -= minutes * 60;
    console.log('minutes', minutes);

    let difference = '';
    if (days > 0) {
      difference += (days === 1) ? `${days} day, ` : `${days} days, `;
    }

    difference += (hours === 0 || hours === 1) ? `${hours} hour, ` : `${hours} hours, `;

    difference += (minutes === 0 || hours === 1) ? `${minutes} minutes` : `${minutes} minutes`;

    return difference;
  }
  getShiftInfo(Id) {
    this.rosterService.GetShiftDetail(Id).subscribe((res => {
      if (res) {
        this.response = res;
        this.shiftInfo = this.response.responseData || {}
        this.BindForm();

      } else {
        this.notificationService.Error({ message: 'Something went wrong! Shift not found', title: null });
      }
    }));
  }



  UpdateShift() {
    if (this.rForm.valid) {
      const selectedOrderIds = this.isShownSleepOver === false ? [] : this.rForm.value.isSleepOver
        .map((checked, i) => checked ? this.sleepOverData[i].id : null)
        .filter(v => v !== null);
      let employee: EmployeeShiftInfoViewModel[] = [];
      this.employeeId.value.forEach(element => {
        let IsSleepOverid = selectedOrderIds.filter(x => x === Number(element))[0];
        if (!IsSleepOverid) {
          IsSleepOverid = 0;
        }
        employee.push({ employeeId: Number(element), isSleepOver: IsSleepOverid > 0 ? true : false, id: 0, name: null });
      });
      // this.empInfoModel = this.rForm.value;
      this.shiftInfo.id = this.shiftId;
      this.shiftInfo.clientId = this.clientId.value;
      this.shiftInfo.employeeId = employee;

      this.shiftInfo.description = this.description.value;
      this.shiftInfo.locationType = this.locationtype.value;
      this.shiftInfo.locationId = this.locationId.value;
      this.shiftInfo.otherLocation = this.otherLocation.value;
      this.shiftInfo.startDate = moment(this.startDate.value).format('YYYY-MM-DD');
      this.shiftInfo.startTime = this.startTime.value;
      this.shiftInfo.endDate = moment(this.endDate.value).format('YYYY-MM-DD');
      this.shiftInfo.endTime = this.endTime.value;
      this.shiftInfo.reminder = this.reminder.value === '1' ? true : false;
      this.shiftInfo.statusId = this.shifStatus.value;
      this.shiftInfo.isPublished = false;
      this.shiftInfo.serviceTypeId = this.ServiceType.value;
      this.shiftInfo.remark = this.remark.value;
      this.rosterService.UpdateShift(this.shiftInfo).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.rForm.reset();
            this.router.navigate(['/roster/scheduler']);
            this.notificationService.Success({ message: 'Shift updated successfully', title: null });
            break;
          case 0:
            if (this.response.responseData == null) {
              this.notificationService.Success({ message: this.response.message, title: null });
            } else if (this.response.responseData !== null) {
              this.btnLoadShiftResult.nativeElement.click();
              this.shiftResponse = this.response.responseData;
              this.dataSource = new MatTableDataSource(this.shiftResponse);
            }
          default:
            break;
        }
      });

    } else {
      this.validateAllFields(this.rForm)
    }
  }
  validateAllFields(formGroup: FormGroup) {
    Object.keys(this.rForm.controls).map(controlName => {
      this.rForm.get(controlName).markAsTouched({ onlySelf: true })
    });
    Object.keys(this.rForm.controls).map(controlName => {
      this.rForm.get(controlName).markAsDirty({ onlySelf: true })
    });
  }


  getLocation() {
    this.commonService.getLocation().subscribe((res => {
      if (res) {
        this.response = res;
        this.LocationList = this.response.responseData || [];

      } else {
        this.notificationService.Error({ message: 'Something went wrong! location not found', title: null });
      }
    }));
  }
  getEmployeeList() {
    this.rosterService.GetEmployeeList().subscribe((res: any) => {
      if (res) {
        this.response = res;
        this.ReportedToList = this.response.responseData || []
        this.ReportedTosearch = this.response.responseData || [];
        this.filteredRecords.next(this.ReportedToList.slice());
      } else {
        this.notificationService.Error({ message: 'Something went wrong! employees not found', title: null });
      }
    });
  }
  getAllClientNameList() {
    this.commonService.getAllClientNameList().subscribe((res => {
      if (res) {
        this.response = res;
        this.clientNameList = this.response.responseData || [];
        this.clientToList = this.response.responseData || [];
        this.filteredRecordsclient.next(this.clientNameList.slice());
      } else {
        this.notificationService.Error({ message: 'Something went wrong! location not found', title: null });
      }
    }));
  }
  getShiftStatusList() {
    this.commonService.getShiftStatusList().subscribe((res => {
      if (res) {
        this.response = res;
        this.shiftStatusList = this.response.responseData || [];

      } else {
        this.notificationService.Error({ message: 'Something went wrong! location not found', title: null });
      }
    }));
  }
  selectChangeHandler(event: any) {
    // this.list=this.LocationTypeList;
    this.selectedType = event;
    if (this.selectedType == 5) {
      this.getLocation()
      this.isShownLocatiodropdown = true;
      this.isShownOtherLocation = false;
      this.rForm.controls['otherLocation'].setValue('');
    }
    else {
      this.isShownLocatiodropdown = false;
      this.isShownOtherLocation = true;
    }
  }
  getLocationType() {
    this.commonService.getLocationType().subscribe((res => {
      if (res) {
        this.response = res;
        this.LocationTypeList = this.response.responseData || [];

      } else {

      }
    }));
  }
  DateChange() {
    let startDate = new Date(this.startDate.value);
    // tslint:disable-next-line: one-variable-per-declaration
    let starttime = new Date(null), endtime = new Date(null);
    if (this.startTime.value && this.endTime.value) {
      starttime = new Date(null);
      starttime.setHours(Number(this.startTime.value.split(':')[0]), Number(this.startTime.value.split(':')[1]))
      endtime.setHours(Number(this.endTime.value.split(':')[0]), Number(this.endTime.value.split(':')[1]));
    }

    if (startDate != new Date(null) && starttime && endtime) {
      let endDate = new Date(this.startDate.value)

      let hours = starttime.getHours() > 12 && endtime.getHours() < 12 ? Number(starttime.getHours()) + (24 - Number(starttime.getHours())) + Number(endtime.getHours()) : Number(starttime.getHours()) + (12 - Number(starttime.getHours())) + endtime.getHours() - 12;
      // endDate.setHours(this.startTime.value.toString().split(':')[0],this.startTime.value.toString().split(':')[1]);
      // tslint:disable-next-line: max-line-length
      let startTimein24 = startDate.getHours();
      if (startTimein24 === 0) {
        startTimein24 = 24;
      }
      if (endtime.getHours() === 24 || endtime.getHours() < startTimein24) {
        endDate.setDate(endDate.getDate() + 1);
      }
      endDate.setHours(endtime.getHours(), endtime.getMinutes());
      //this.endDate.setValue(endDate);
    }
  }
  getServiceTypelist() {
    this.commonService.getServiceList().subscribe((res => {
      if (res) {
        this.response = res;
        this.ServiceTypeList = this.response.responseData || [];
        // let item = this.ServiceTypeList.filter(x => x.supportItemName === 'Admin')[0];
        // let item2 = this.ServiceTypeList.filter(x => x.supportItemName === 'Cleaning')[0];
        // this.ServiceTypeList = [];
        // if (item) {
        //   this.ServiceTypeList.push(item);
        // }
        // if (item2) {
        //   this.ServiceTypeList.push(item2);
        // }
      } else {

      }
    }));
  }

  formattedaddress = " ";

  public handleAddressChange(address: any) {
    this.formattedaddress = address.address1
    this.rForm.controls['otherlocation'].setValue(this.formattedaddress);
  }

  redirectToRoster() {
    this.btnCancelLoadTemplate.nativeElement.click();
    this.router.navigate(['/roster/scheduler']);
  }
}
