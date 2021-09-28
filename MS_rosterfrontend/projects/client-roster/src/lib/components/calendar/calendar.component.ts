import { Component, OnInit, ViewChild } from '@angular/core';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { DayPilotMonthComponent, DayPilot } from 'daypilot-pro-angular';
import { ShiftViewModel, TempShiftViewModel } from '../../view-models/shift-view-model';
import { RosterService } from '../../services/roster.service';
import { NotificationService } from 'projects/core/src/projects';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'lib-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {
  clientId: number = 0;
  responseModel: ResponseModel = {};
  runningMonth: string;
  month: number;
  year: number;
  monthVal: string;
  events: ShiftViewModel[] = [];
  tempEventsArray: TempShiftViewModel[] = [];
  tempEvents: TempShiftViewModel = {};
  @ViewChild("calendar") calendar: DayPilotMonthComponent;

  config: DayPilot.CalendarConfig = {
    days: DayPilot.Date.today().daysInMonth(),
    startDate: DayPilot.Date.today().firstDayOfMonth(),
    onBeforeEventRender: args => {
      args.data.cssClass = "shift-" + args.data.statusName.toLowerCase();
    },
    bubble: new DayPilot.Bubble({
      onLoad: args => {
        let event = args.source;
        var shiftId = event.data.id;
        args.html = "<h3><a href='/client/dashboard/view-shift/" + shiftId + "'>" + event.text() + "</a></h3>";
        args.html += "<p><b>Date</b> " + event.start().toString("MMMM d, yyyy") + "<b> Time </b>" + event.data.startTime + " to " + event.data.endTime + "</p>";
        args.html += "<p><b>Location </b>" + event.data.location + "</p>";
      }
    }),
  }

  constructor(private rosterService: RosterService, private notificationService: NotificationService, private route: ActivatedRoute) {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = params.params.id;
    });
  }

  ngOnInit(): void {
    this.month = new Date().getMonth();
    this.year = new Date().getFullYear();
    let fromDate = this.config.startDate as DayPilot.Date;
    let toDate = fromDate.addDays(this.config.days - 1);
    this.getAssignedShift(fromDate, toDate);
  }

  getAssignedShift(fromDate, toDate) {
    const data = {
      id: Number(this.clientId),
      fromDate: fromDate,
      toDate: toDate
    }
    this.rosterService.getClientCalendarShifts(data).subscribe(result => {
      this.responseModel = result;
      switch (this.responseModel.status) {
        case 1:
          this.events = this.responseModel.responseData;
          this.tempEventsArray = [];
          for (let i = 0; i < this.events.length; i++) {
            this.tempEvents = {};
            this.tempEvents.id = this.events[i].id;
            this.tempEvents.start = this.events[i].startDate;
            this.tempEvents.end = this.events[i].endDate;
            this.tempEvents.text = this.events[i].description;
            this.tempEvents.startTime = this.events[i].startTimeString;
            this.tempEvents.endTime = this.events[i].endTimeString;
            this.tempEvents.location = this.events[i].location;
            this.tempEvents.statusName = this.events[i].statusName;
            this.tempEventsArray.push(this.tempEvents);
          }
          break;
        case 0:
          this.notificationService.Warning({ message: 'Error occured while loading shifts', title: '' });
          break;

        default:
          break;
      }
    });
  }

  previousMonth() {
    this.config.startDate = new DayPilot.Date(this.config.startDate).addMonths(-1);
    this.config.days = this.config.startDate.daysInMonth();
    if (this.month >= 0) {
      this.month--;
    }
    if (this.month == -1) {
      this.month = 11;
      this.year--;
    }
    this.getMonth(this.month);
    this.runningMonth = this.monthVal + ' ' + this.year;
    this.getAssignedShift(this.config.startDate, (this.config.startDate.addDays(this.config.days - 1)));
  }

  thisMonth() {
    this.config.startDate = DayPilot.Date.today().firstDayOfMonth()
    this.config.days = this.config.startDate.daysInMonth();
    this.month = new Date().getMonth();
    this.year = new Date().getFullYear();
    this.getMonth(this.month);
    this.runningMonth = this.monthVal + ' ' + this.year;
    this.getAssignedShift(this.config.startDate, (this.config.startDate.addDays(this.config.days - 1)));
  }

  nextMonth() {
    this.config.startDate = new DayPilot.Date(this.config.startDate).addMonths(+1);
    this.config.days = this.config.startDate.daysInMonth();
    if (this.month <= 11) {
      this.month++;
    }
    if (this.month == 12) {
      this.month = 0;
      this.year++;
    }
    this.getMonth(this.month);
    this.runningMonth = this.monthVal + ' ' + this.year;
    this.getAssignedShift(this.config.startDate, (this.config.startDate.addDays(this.config.days - 1)));
  }

  getMonth(monthNo: number) {
    switch (monthNo) {
      case 0:
        this.monthVal = 'January';
        break;
      case 1:
        this.monthVal = 'February';
        break;
      case 2:
        this.monthVal = 'March';
        break;
      case 3:
        this.monthVal = 'April';
        break;
      case 4:
        this.monthVal = 'May';
        break;
      case 5:
        this.monthVal = 'June';
        break;
      case 6:
        this.monthVal = 'July';
        break;
      case 7:
        this.monthVal = 'August';
        break;
      case 8:
        this.monthVal = 'September';
        break;
      case 9:
        this.monthVal = 'October';
        break;
      case 10:
        this.monthVal = 'November';
        break;
      case 11:
        this.monthVal = 'December';
        break;
      default:
        break;
    }
  }
}
