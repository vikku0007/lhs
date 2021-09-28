import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { RosterService } from '../../services/roster.service';
import { NotificationService } from 'projects/core/src/projects';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { DayPilotMonthComponent, DayPilot } from 'daypilot-pro-angular';
import { Shiftviewmodel, TempShiftViewModel } from '../../view-models/shiftviewmodel';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'lib-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit, AfterViewInit {
  employeeId: number = 0;
  responseModel: ResponseModel = {};
  runningMonth: string;
  month: number;
  year: number;
  monthVal: string;
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
        //  args.html = "<h2>" + event.data.resourceName + "</h2>";
        args.html = "<h3><a href='/employee/dashboard/view-shift/" + shiftId + "'>" + event.text() + "</a></h3>";
        args.html += "<p><b>Date</b> " + event.start().toString("MMMM d, yyyy") + "<b> Time </b>" + event.data.startTime + " to " + event.data.endTime + "</p>";
        args.html += "<p><b>Location </b>" + event.data.location + "</p>";
        //args.html += "<p><b>Staff</b></p>";
        // let staffName = '';
        // let clientName = '';
        // event.data.employeeShiftInfo.forEach(staff => {
        //   staffName = staff.name + ' | ';
        // });
        // args.html += staffName;
        // args.html += event.data.duration + ' Hrs';
        // args.html += "<p><b>Clients</b></p>";
        // event.data.clientShiftInfo.forEach(client => {
        //   clientName = client.name;
        // });
        // args.html += clientName;
        // args.html += '<a class="edit-icon" href="/roster/edit-shift?Id=' + shiftId + '"><svg height="16px" id="Layer_1" style="enable-background:new 0 0 16 16;" version="1.1" viewBox="0 0 16 16" width="16px" xml:space="preserve" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"><path d="M2.453,9.297C1.754,9.996,1,13.703,1,14c0,0.521,0.406,1,1,1c0.297,0,4.004-0.754,4.703-1.453l5.722-5.722l-4.25-4.25  L2.453,9.297z M12,1c-0.602,0-1.449,0.199-2.141,0.891L9.575,2.175l4.25,4.25l0.284-0.284C14.746,5.504,15,4.695,15,4  C15,2.343,13.656,1,12,1z"/></svg></a>';
      }
    }),
  }

  events: Shiftviewmodel[] = [];
  tempEventsArray: TempShiftViewModel[] = [];
  tempEvents: TempShiftViewModel = {};

  constructor(private rosterService: RosterService, private notificationService: NotificationService, private route: ActivatedRoute) {
    this.route.paramMap.subscribe((params: any) => {
      this.employeeId = params.params.id;
    });
  }

  ngAfterViewInit(): void {

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
      id: Number(this.employeeId),
      fromDate: fromDate,
      toDate: toDate
    }
    this.rosterService.getEmployeeCalendarShifts(data).subscribe(result => {
      this.tempEventsArray = [];
      this.responseModel = result;
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
