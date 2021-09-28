import { Component, OnInit, ViewChildren, QueryList, ViewChild, Output, EventEmitter, ElementRef } from '@angular/core';
import { MonthlyTimelineComponent } from '../monthly-timeline/monthly-timeline.component';
import { RosterService } from '../../services/roster.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ShiftInfoViewModel } from '../../viewmodel/roster-shift-info-viewModel';
import { WeekTimelineComponent } from '../week-timeline/week-timeline.component';
import { DayTimelineComponent } from '../day-timeline/day-timeline.component';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { NotificationService } from 'projects/core/src/projects';
import { Paging } from 'projects/viewmodels/paging';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent } from '@angular/material/paginator';
import { start } from 'repl';
import { NewdayTimelineComponent } from '../newday-timeline/newday-timeline.component';

interface employeeModel {
  id?: number;
  fullName?: string;
}

interface shiftTemplate {
  id?: number;
  name?: string;
}

@Component({
  selector: 'lib-scheduler',
  templateUrl: './scheduler.component.html',
  styleUrls: ['./scheduler.component.scss']
})

export class SchedulerComponent implements OnInit {
  @ViewChild(MonthlyTimelineComponent) private monthlyTimeline: MonthlyTimelineComponent;
  @ViewChild(WeekTimelineComponent) private weekTimeline: WeekTimelineComponent;
  @ViewChild(DayTimelineComponent) private dayTimeline: DayTimelineComponent;
  @ViewChild('btnMonth') month: ElementRef;
  @ViewChild('btnWeek') week: ElementRef;
  @ViewChild('btnDay') day: ElementRef;
  @ViewChild('formDirective') formDirective: NgForm;
  @ViewChild('btnCancel') cancel: ElementRef;
  @ViewChild('btnCancelLoadTemplate') cancelloadTemplate: ElementRef;
  @ViewChild('btnLoadTemplate') loadTemplateElem: ElementRef;
  @ViewChild('tempName') tempName: ElementRef;
  displayedColumns: string[] = ['id', 'templatename', 'action'];
  dataSource: any;
  paging: Paging = {};
  selectedDayDiv: boolean;
  selectedWeekDiv: boolean;
  selectedMonthDiv: boolean;
  responseModel: ResponseModel = {};
  shiftModel: ShiftInfoViewModel = {};
  shiftModelArray: ShiftInfoViewModel[] = [];
  empModelArray: employeeModel[] = [];
  startDate: string;
  endDate: string;
  searchByEmpName = 0;
  searchByClientName = 0;
  searchBylocation = 0;
  searchByStatus = 0;
  searchByShiftType = 0;
  searchBymanualAddress = '';
  runningMonth: string;
  runningDay: string;
  runningWeek: string;
  rForm: FormGroup;
  getErrorMessage: any;
  shiftTemplateArray: shiftTemplate[] = [];
  totalCount: number;

  constructor(private rosterService: RosterService, private fb: FormBuilder, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.selectedDayDiv = true;
    const todayDate = new Date();
    this.startDate = todayDate.getFullYear() + '-' +
      (todayDate.getMonth().toString().length !== 1 ? (todayDate.getMonth() + 1) : '0' + (todayDate.getMonth() + 1)) + '-' + '01';
    this.endDate = todayDate.getFullYear() + '-' +
      (todayDate.getMonth().toString().length !== 1 ? (todayDate.getMonth() + 1) : '0' + (todayDate.getMonth() + 1)) + '-' + '31';
    // this.getAllShifts();
    this.getEmployeeList();
    this.createForm();
    this.getShiftTemplate();
  }

  createForm() {
    this.rForm = this.fb.group({
      templateName: ['', Validators.required]
    });
  }

  getAllShifts() {
    const data = {
      searchByEmpName: this.searchByEmpName,
      searchByClientName: this.searchByClientName,
      searchTextBylocation: this.searchBylocation,
      searchTextByStatus: this.searchByStatus,
      searchTextByShiftType: this.searchByShiftType,
      searchTextByManualAddress: this.searchBymanualAddress,
      startDate: this.startDate,
      endDate: this.endDate
    };

    this.rosterService.GetEmployeeViewCalendar(data).subscribe((res: any) => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.shiftModelArray = this.responseModel.responseData;
          break;

        default:
          break;
      }
    });

  }

  loadTemplatePopup() {
    this.loadTemplateElem.nativeElement.click();
  }

  getShiftTemplate() {
    this.rosterService.GetShiftTemplate().subscribe((res: any) => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.shiftTemplateArray = this.responseModel.responseData;
          this.totalCount = this.responseModel.total;
          this.dataSource = new MatTableDataSource(this.shiftTemplateArray);
          break;

        default:
          break;
      }
    });
  }

  getEmployeeList() {
    this.rosterService.GetEmployeeList().subscribe((res: any) => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.empModelArray = this.responseModel.responseData;
          break;

        default:
          break;
      }
    });
  }

  previousMonth() {
    this.monthlyTimeline.previous();
  }

  previousDay() {
    this.dayTimeline.previous();
  }

  previousWeek() {
    this.weekTimeline.previous();
  }

  nextDay() {
    this.dayTimeline.next();
  }

  nextMonth() {
    this.monthlyTimeline.next();
  }

  nextWeek() {
    this.weekTimeline.next();
  }


  showMonth() {
    this.selectedDayDiv = false;
    this.selectedMonthDiv = true;
    this.selectedWeekDiv = false;
    this.month.nativeElement.className = 'nav-item active';
    this.week.nativeElement.className = 'nav-item';
    this.day.nativeElement.className = 'nav-item';
  }

  showWeek() {
    this.selectedDayDiv = false;
    this.selectedMonthDiv = false;
    this.selectedWeekDiv = true;
    this.month.nativeElement.className = 'nav-item';
    this.week.nativeElement.className = 'nav-item active';
    this.day.nativeElement.className = 'nav-item';
  }

  showDay() {
    this.selectedDayDiv = true;
    this.selectedMonthDiv = false;
    this.selectedWeekDiv = false;
    this.month.nativeElement.className = 'nav-item';
    this.week.nativeElement.className = 'nav-item';
    this.day.nativeElement.className = 'nav-item active';
  }

  currentMonth(mon: string) {
    this.runningMonth = mon;
  }

  currentDay(day: string) {
    this.runningDay = day;
  }

  currentWeek(week: string) {
    this.runningWeek = week;
  }

  thisMonth() {
    this.monthlyTimeline.showCurrentMonth();
  }

  thisWeek() {
    this.weekTimeline.showCurrentWeek();
  }

  today() {
    this.dayTimeline.showToday();
  }

  searchScheduler(searchData: any) {
    console.log(searchData);
    if (this.selectedDayDiv) {
      this.dayTimeline.searchDayShifts(searchData);
    } else if (this.selectedWeekDiv) {
      this.weekTimeline.searchWeeklyShifts(searchData);
    } else if (this.selectedMonthDiv) {
      this.monthlyTimeline.searchMonthlyShifts(searchData);
    }
  }

  AddTemplate() {
    if (this.rForm.valid) {
      const params = {
        shiftId: this.weekTimeline.events.map(({ id }) => +id.split('_')[0]),
        name: this.rForm.controls['templateName'].value
      };
      console.log(params);
      this.rosterService.AddShiftTemplate(params).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.rForm.reset();
            this.formDirective.resetForm();
            this.cancel.nativeElement.click();
            this.notificationService.Success({ message: 'Template added successfully', title: null });
            this.shiftTemplateArray.push(this.responseModel.responseData);
            this.dataSource = new MatTableDataSource(this.shiftTemplateArray);
            break;
          case 0:
            this.rForm.reset();
            this.formDirective.resetForm();
            this.cancel.nativeElement.click();
            this.notificationService.Success({ message: this.responseModel.message, title: null });
          default:
            break;
        }
      });
    }
  }

  cancelForm() {
    this.rForm.reset();
    this.formDirective.resetForm();
  }

  Search(data) {
    this.dataSource.filter = data.trim().toLowerCase();
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getShiftTemplate();
  }

  cancelLoadTemplate() {

  }

  loadTemplate(shiftId) {
    const data = {
      shiftTemplateId: shiftId,
      startDate: this.getStartDate(),
      endDate: this.getEndDate()
    }
    this.rosterService.LoadShiftTemplate(data).subscribe((res: any) => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.weekTimeline.loadWeelyShiftFromTemplate(this.responseModel.responseData);
          this.cancelloadTemplate.nativeElement.click();
          this.notificationService.Success({ message: 'Shifts loaded successfully', title: '' });
          break;

        default:
          break;
      }
    });
  }

  getStartDate() {
    var splitDate = this.runningWeek.split('to')[0].toString().trim().split('/');
    var finalDate = splitDate[2] + '-' + splitDate[1] + '-' + splitDate[0];
    return new Date(finalDate);
  }

  getEndDate() {
    var splitDate = this.runningWeek.split('to')[1].toString().trim().split('/');
    var finalDate = splitDate[2] + '-' + splitDate[1] + '-' + splitDate[0];
    return new Date(finalDate);
  }
}
