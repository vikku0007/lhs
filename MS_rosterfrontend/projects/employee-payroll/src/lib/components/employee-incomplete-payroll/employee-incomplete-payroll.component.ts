import { DatePipe } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { NotificationService } from 'projects/core/src/projects';
import { Paging } from 'projects/viewmodels/paging';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { PayrollService } from '../../services/payroll.service';
import { EmployeeDetailModel } from '../../view-models/employee-detail-model';

@Component({
  selector: 'lib-employee-incomplete-payroll',
  templateUrl: './employee-incomplete-payroll.component.html',
  styleUrls: ['./employee-incomplete-payroll.component.scss']
})
export class EmployeeIncompletePayrollComponent implements OnInit {
  displayedColumns: string[] = ['sr', 'description', 'date', 'time', 'duration', 'client', 'staff', 'location', 'status', 'action'];
  dataSource: any;
  empDetailModel: EmployeeDetailModel[] = [];
  empDetail: EmployeeDetailModel = {};
  responseModel: ResponseModel = {};
  paging: Paging = {};
  rForm: FormGroup;
  todayDatemax = new Date();
  @ViewChild('btnCancel') cancel: ElementRef;
  totalCount: number;
  time = { hour: 13, minute: 30 };
  meridian = true;
  startDate?: Date;
  endDate?: Date;
  startTime?: string;
  endTime?: string;

  constructor(private payrollService: PayrollService, private fb: FormBuilder, private datePipe: DatePipe,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.dataSource = new MatTableDataSource(this.empDetailModel);
    this.createForm(this.empDetail);
    this.getIncompleteShifts();
  }

  createForm(data: EmployeeDetailModel) {
    this.rForm = this.fb.group({
      checkIn: [{ value: data != null ? data.checkInDate : null, disabled: false }, Validators.required],
      checkOut: [{ value: data != null ? data.checkOutDate != null : null, disabled: false }, Validators.required],
      loginTime: [{ value: data?.checkInDate != undefined ? this.setCheckInTime(data.checkInTimeString) : null, disabled: false },
      Validators.nullValidator],
      logoutTime: [{ value: data?.checkOutDate != undefined ? new Date(data.checkOutTimeString) : null, disabled: false },
      Validators.nullValidator],
      remark: [null, Validators.required]
    });
  }

  setCheckInTime(date: string) {
    const hours = date.split(' ')[0].split(':')[0];
    const min = date.split(' ')[0].split(':')[1];
    const dur = date.split(' ')[1];
    (<HTMLInputElement>document.getElementsByClassName('bs-timepicker-field')[0]).value = hours;
    (<HTMLInputElement>document.getElementsByClassName('bs-timepicker-field')[1]).value = min;
    (<HTMLInputElement>document.getElementsByClassName('btn btn-default text-center')[0]).innerText = dur;
  }

  getIncompleteShifts() {
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      SearchByEmpName: 0,
      SearchByClientName: 0,
      SearchTextBylocation: 0,
      SearchTextByStatus: 0,
      SearchTextByManualAddress: null,
      SearchTextByShiftType: 0,
      SearchByStartDate: null,
      SearchByEndDate: null
    };
    this.payrollService.IncompleteShift(data).subscribe(res => {
      this.responseModel = res;
      this.totalCount = this.responseModel.total;
      if (this.responseModel.status == 1) {
        this.empDetailModel = this.responseModel.responseData;
        this.dataSource = new MatTableDataSource(this.empDetailModel);
      }
    })
  }

  AddCheckoutDetails() {
    if (this.rForm.valid) {
      const checkInHrs = (<HTMLInputElement>document.getElementsByClassName('bs-timepicker-field')[0]).value;
      const checkInMin = (<HTMLInputElement>document.getElementsByClassName('bs-timepicker-field')[1]).value;
      const checkInDur = (<HTMLInputElement>document.getElementsByClassName('btn btn-default text-center')[0]).innerText;
      const checkOutHrs = (<HTMLInputElement>document.getElementsByClassName('bs-timepicker-field')[2]).value;
      const checkOutMin = (<HTMLInputElement>document.getElementsByClassName('bs-timepicker-field')[3]).value;
      const checkOutDur = (<HTMLInputElement>document.getElementsByClassName('btn btn-default text-center')[1]).innerText;
      const checkInTime24Hour = this.convertTimeTo24HrFormat(Number(checkInHrs), Number(checkInMin), checkInDur);
      const checkOutTime24Hour = this.convertTimeTo24HrFormat(Number(checkOutHrs), Number(checkOutMin), checkOutDur);
      const data = {
        checkInDate: moment(this.rForm.controls['checkIn'].value).format('YYYY-MM-DD'),
        checkOutDate: moment(this.rForm.controls['checkOut'].value).format('YYYY-MM-DD'),
        checkInTime: checkInTime24Hour,
        checkOutTime: checkOutTime24Hour,
        employeeId: Number((<HTMLInputElement>document.getElementById('employeeId')).value),
        shiftId: Number((<HTMLInputElement>document.getElementById('shiftId')).value),
        remark: this.rForm.controls['remark'].value
      }
      console.log(this.rForm.controls['checkIn'].value, this.rForm.controls['checkOut'].value,)
      console.log(data);
      this.payrollService.AddCheckoutDetails(data).subscribe(res => {
        this.responseModel = res;
        if (this.responseModel.status == 1) {
          this.notificationService.Success({ message: 'Checkout details added successfully', title: '' });
          this.cancel.nativeElement.click();
          this.getIncompleteShifts();
        } else if (this.responseModel.status == 0) {
          this.notificationService.Warning({ message: 'Some error occured', title: '' });
          this.cancel.nativeElement.click();
        }
      })
    }
  }

  convertTimeTo24HrFormat(hr: number, min: number, dur: string) {
    let finalTimeString = null;
    if (dur.toLowerCase() == 'pm') {
      hr = hr + 12;
    } else {
      if (hr == 12) {
        hr = hr - 12;
      }
      hr = hr;
    }
    finalTimeString = hr + ':' + min;
    return finalTimeString;
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getIncompleteShifts();
  }

  editCheckoutDetails(data: EmployeeDetailModel) {
    this.startDate = data.startDate;
    this.endDate = data.endDate;
    this.startTime = data.startTimeString;
    this.endTime = data.endTimeString;
    (<HTMLInputElement>document.getElementById('employeeId')).value = data.employeeShiftInfoViewModel[0].employeeId;
    (<HTMLInputElement>document.getElementById('shiftId')).value = data.employeeShiftInfoViewModel[0].id;
    if (data.isLogin == true) {
      this.createForm(data);
    } else {
      this.rForm.controls['checkIn'].patchValue(null);
      this.createForm(null);
      (<HTMLInputElement>document.getElementsByClassName('bs-timepicker-field')[0]).value = null;
      (<HTMLInputElement>document.getElementsByClassName('bs-timepicker-field')[1]).value = null;
      (<HTMLInputElement>document.getElementsByClassName('bs-timepicker-field')[2]).value = null;
      (<HTMLInputElement>document.getElementsByClassName('bs-timepicker-field')[3]).value = null;
    }
  }

  toggleMeridian() {
    this.meridian = !this.meridian;
  }

  cancelForm() {

  }

}
