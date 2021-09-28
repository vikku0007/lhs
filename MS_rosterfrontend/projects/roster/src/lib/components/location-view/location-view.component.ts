import { Component, OnInit, ViewChild } from '@angular/core';
import { DayPilotSchedulerComponent, DayPilot } from 'daypilot-pro-angular';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { RosterService } from '../../services/roster.service';
import { ShiftInfoViewModel, CalendarShiftInfoViewModel } from '../../viewmodel/roster-shift-info-viewModel';
import { NotificationService } from 'projects/core/src/projects';
import { LoaderService } from 'projects/lhs-service/src/projects';

interface Resources {
  name?: string;
  id: string | number;
  expanded?: boolean;
  children?: Resources[];
}

interface Shifts {
  id?: string;
  resource?: number | string;
  start?: string;
  end?: string;
  text?: string;
  color?: string;
  backColor?: string;
  status?: string;
  startTime?: string;
  endTime?: string;
  location?: string;
  duration?: number;
  employeeShiftInfo?: any;
  clientShiftInfo?: any;
  resourceName?: string;
}

interface employeeModel {
  id?: number | string;
  fullName?: string;
}

interface Employee {
  id?: number | string;
  name?: string;
}

interface searchFilters {
  searchByEmpName?: number;
  searchByClientName?: number;
  searchBylocation?: number;
  searchByStatus?: number;
  searchByShiftType?: number;
  searchBymanualAddress?: string;
}

@Component({
  selector: 'lib-location-view',
  templateUrl: './location-view.component.html',
  styleUrls: ['./location-view.component.scss']
})

export class LocationViewComponent implements OnInit {
  @ViewChild('scheduler', { static: false })
  scheduler: DayPilotSchedulerComponent;
  events: any[] = [];
  responseModel: ResponseModel = {};
  shiftsArray: Shifts[] = [];
  dayVal: string;
  shiftModelArray: CalendarShiftInfoViewModel[] = [];
  shiftMod: Shifts = {};
  monthVal: string;
  month: number;
  year: number;
  empModelArray: employeeModel[];
  employeeModel: Employee = {};
  employeeArray: any[] = [];
  runningMonth: string;
  searchData: searchFilters = {
    searchByClientName: 0,
    searchByEmpName: 0,
    searchByShiftType: 0,
    searchByStatus: 0,
    searchBylocation: 0,
    searchBymanualAddress: ''
  };
  resources: Resources[] = [];
  resource: Resources = {
    id: '',
    name: '',
    children: [{ id: '', name: '' }]
  };

  config: DayPilot.SchedulerConfig = {
    timeHeaders: [{ "groupBy": "Month" }, { "groupBy": "Day", "format": "d" }],
    scale: "Day",
    cellWidth: 50,
    days: DayPilot.Date.today().daysInMonth(),
    startDate: DayPilot.Date.today().firstDayOfMonth(),
    treeEnabled: true,
    onBeforeEventRender: args => {
      if (args !== undefined) {
        // const index = args.data.employeeShiftInfo.findIndex(x => x.employeeId == args.data.resource.split('_')[1]);
        // args.data.cssClass = "shift-" + args.data.employeeShiftInfo[index].statusName.toLowerCase();
        args.data.cssClass = "shift-" + args.data.status.toLowerCase();
      }
    },
    bubble: new DayPilot.Bubble({
      onLoad: args => {
        let event = args.source;
        var shiftId = event.data.id.split('_')[0];
        let clientShiftInfoViewModel = [];
        let employeeShiftInfoViewModel = [];

        // this.rosterService.GetShiftPopOverInfo({ shiftId: Number(shiftId) }).subscribe((res: any) => {
        //   this.responseModel = res;
        //   switch (this.responseModel.status) {
        //     case 1:
        //       clientShiftInfoViewModel = this.responseModel.responseData.responseData.clientShiftInfoViewModel;
        //       employeeShiftInfoViewModel = this.responseModel.responseData.responseData.employeeShiftInfoViewModel;
        //       break;
        //     default:
        //       break;
        //   }
        // });



        args.html = "<h2>" + event.data.resourceName + "</h2>";
        args.html += "<h3><a href='/roster/view-shift?Id=" + shiftId + "'>" + event.text() + "</a></h3>";
        args.html += "<p><b>Date</b> " + event.start().toString("MMMM d, yyyy") + "<b> Time </b>" + event.data.startTime + " to " + event.data.endTime + "</p>";
        args.html += "<p><b>Location </b>" + event.data.location + "</p>";
        args.html += "<p><b>Staff</b></p>";
        let staffName = '';
        let clientName = '';
        // event.data.employeeShiftInfo.forEach(staff => {
        //   staffName = staff.name + ' | ';
        // });
        // employeeShiftInfoViewModel.forEach(staff => {
        //   staffName = staff.name + ' | ';
        // });
        args.html += staffName;
        args.html += event.data.duration + ' Hrs';
        args.html += "<p><b>Clients</b></p>";
        // event.data.clientShiftInfo.forEach(client => {
        //   clientName = client.name;
        // });
        // clientShiftInfoViewModel.forEach(client => {
        //   clientName = client.name;
        // });
        args.html += clientName;
        args.html += '<a title="Edit Shift" class="edit-icon" href="/roster/edit-shift?Id=' + shiftId + '"><svg height="16px" id="Layer_1" style="enable-background:new 0 0 16 16;" version="1.1" viewBox="0 0 16 16" width="16px" xml:space="preserve" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"><path d="M2.453,9.297C1.754,9.996,1,13.703,1,14c0,0.521,0.406,1,1,1c0.297,0,4.004-0.754,4.703-1.453l5.722-5.722l-4.25-4.25  L2.453,9.297z M12,1c-0.602,0-1.449,0.199-2.141,0.891L9.575,2.175l4.25,4.25l0.284-0.284C14.746,5.504,15,4.695,15,4  C15,2.343,13.656,1,12,1z"/></svg></a>';
      }
    }),
  }

  constructor(private rosterService: RosterService, private notificationService: NotificationService, private loaderService: LoaderService) { }

  ngOnInit(): void {
    this.month = new Date().getMonth();
    this.year = new Date().getFullYear();
    this.getMonth(this.month);
    this.monthVal = this.monthVal + ' ' + this.year;
    this.runningMonth = this.monthVal;
    this.getEmployeeList();
  }

  schedulerViewChanged(args) {
    if (args.visibleRangeChanged) {
      const fromDate = this.scheduler.control.visibleStart();
      const toDate = fromDate.addDays(this.config.days - 1);
      this.getAllShiftsDateWise(fromDate, toDate);
    }
  }

  getAllShiftsDateWise(startDate, endDate) {
    const data = {
      searchByEmpName: this.searchData.searchByEmpName,
      searchByClientName: this.searchData.searchByClientName,
      searchTextBylocation: this.searchData.searchBylocation,
      searchTextByStatus: this.searchData.searchByStatus,
      searchTextByShiftType: this.searchData.searchByShiftType,
      searchTextByManualAddress: this.searchData.searchBymanualAddress,
      startDate: startDate,
      endDate: endDate
    };
    this.loaderService.start();
    this.rosterService.GetEmployeeViewCalendar(data).subscribe((res: any) => {
      this.loaderService.stop();
      this.responseModel = res;
      this.shiftsArray = [];
      this.resources = [];
      switch (this.responseModel.status) {
        case 1:
          this.shiftModelArray = this.responseModel.responseData;
          for (let i = 0; i < this.shiftModelArray.length; i++) {
            //  for (let j = 0; j < this.shiftModelArray[i].employeeShiftInfoViewModel.length; j++) {
            this.shiftMod = {};
            // this.shiftMod.id = this.shiftModelArray[i].id + '_' + this.shiftModelArray[i].employeeShiftInfoViewModel[j].employeeId;
            // this.shiftMod.resource = this.shiftModelArray[i].id + '_' + this.shiftModelArray[i].employeeShiftInfoViewModel[j].employeeId;              
            // this.shiftMod.resourceName = this.shiftModelArray[i].employeeShiftInfoViewModel[j].name;
            this.shiftMod.id = this.shiftModelArray[i].id + '_' + this.shiftModelArray[i].employeeId;
            this.shiftMod.resource = this.shiftModelArray[i].employeeId;
            this.shiftMod.resourceName = this.shiftModelArray[i].name;
            this.shiftMod.start = this.shiftModelArray[i].startDate;
            this.shiftMod.end = this.shiftModelArray[i].endDate;
            this.shiftMod.text = this.shiftModelArray[i].description;
            this.shiftMod.status = this.shiftModelArray[i].statusName;
            this.shiftMod.duration = this.shiftModelArray[i].duration;
            this.shiftMod.location = this.shiftModelArray[i].locationName;
            this.shiftMod.startTime = this.shiftModelArray[i].startTimeString;
            this.shiftMod.endTime = this.shiftModelArray[i].endTimeString;
            // this.shiftMod.employeeShiftInfo = this.shiftModelArray[i].employeeShiftInfoViewModel;
            // this.shiftMod.clientShiftInfo = this.shiftModelArray[i].clientShiftInfoViewModel;
            this.shiftsArray.push(this.shiftMod);
            //  }
          }

          if (this.empModelArray !== undefined && this.empModelArray.length > 0) {
            for (let i = 0; i < this.empModelArray.length; i++) {
              this.employeeModel = {};
              this.employeeModel.id = this.empModelArray[i].id;
              this.employeeModel.name = this.empModelArray[i].fullName;
              this.employeeArray.push(this.employeeModel);
            }
          }

          for (let j = 0; j < this.shiftsArray.length; j++) {
            this.resource = { id: '', children: [{ id: '' }] };
            if (this.shiftsArray[j].location !== null) {
              const index = this.checkLocation(this.shiftsArray[j].location, this.resources);
              if (index > -1) {
                // const ind = this.resources[index].children.findIndex(x => x.name.toLowerCase() === this.shiftsArray[j].resourceName.toLowerCase());
                const ind = this.resources[index].children.findIndex(x => x.id === this.shiftsArray[j].resource);
                if (ind < 0) {
                  this.resources[index].children.push({ id: this.shiftsArray[j].resource, name: this.shiftsArray[j].resourceName })
                }
              } else {
                this.resource.id = j;
                this.resource.expanded = true;
                this.resource.name = this.shiftsArray[j].location;
                this.resource.children[0].id = this.shiftsArray[j].resource;
                this.resource.children[0].name = this.shiftsArray[j].resourceName;
                this.resources.push(this.resource);
              }
            }
          }

          if (data.searchTextBylocation == 0) {
            this.resource = { id: '', children: [{ id: '' }] };
            this.resource.id = 100;
            this.resource.expanded = true;
            this.resource.name = 'Other';

            for (let i = 0; i < this.empModelArray.length; i++) {
              this.resource = { id: '', children: [{ id: '' }] };
              // const index = this.shiftsArray.findIndex(x => x.resource == this.empModelArray[i].id);
              const index = this.shiftsArray.findIndex(x => x.resource == this.empModelArray[i].id);
              if (index == -1) {
                const indices = this.checkLocation('Other', this.resources);
                if (indices > -1) {
                  this.resources[indices].children.push({ id: this.empModelArray[i].id, name: this.empModelArray[i].fullName })
                } else {
                  this.resource.id = this.resources.length + (i + 1);
                  this.resource.expanded = true;
                  this.resource.name = 'Other';
                  this.resource.children[0].id = this.empModelArray[i].id;
                  this.resource.children[0].name = this.empModelArray[i].fullName;
                  this.resources.push(this.resource);
                }
              }
            }
          }

          this.events = this.shiftsArray;
          this.config.resources = this.resources;
          console.log(this.shiftsArray);
          console.log(this.resources);
          this.getMonth(this.month);
          this.monthVal = this.monthVal + ' ' + this.year;
          this.runningMonth = this.monthVal;
          // this.runningMonth.emit(this.monthVal);
          break;
        case 0:
          this.getMonth(this.month);
          this.monthVal = this.monthVal + ' ' + this.year;
          // this.runningMonth.emit(this.monthVal);
          this.runningMonth = this.monthVal;
          this.events = [];
          this.notificationService.Warning({ message: this.responseModel.message, title: '' });
          break;

        default:
          break;
      }
    });
  }

  searchScheduler(data: any) {
    this.searchData.searchByClientName = data.searchByClientName;
    this.searchData.searchByEmpName = data.searchByEmpName;
    this.searchData.searchByShiftType = data.searchByShiftType;
    this.searchData.searchByStatus = data.searchByStatus;
    this.searchData.searchBylocation = data.searchBylocation;
    this.searchData.searchBymanualAddress = data.searchBymanualAddress;
    const args = { visibleRangeChanged: true }
    this.schedulerViewChanged(args);
  }

  checkLocation(location: string, shifts: Resources[]) {
    const index = shifts.findIndex(x => x.name.toLowerCase() == location.toLowerCase());
    return index;
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
    this.config.startDate = new DayPilot.Date(this.config.startDate).addMonths(-1);
    this.config.days = this.config.startDate.daysInMonth();
    if (this.month >= 0) {
      this.month--;
    }
    if (this.month == -1) {
      this.month = 11;
      this.year--;
    }
  }

  thisMonth() {
    this.config.startDate = DayPilot.Date.today().firstDayOfMonth()
    this.config.days = this.config.startDate.daysInMonth();
    this.month = new Date().getMonth();
    this.year = new Date().getFullYear();
  }

  nextMonth() {
    this.config.startDate = new DayPilot.Date(this.config.startDate).addMonths(+1);
    this.config.days = this.config.startDate.daysInMonth()
    if (this.month <= 11) {
      this.month++;
    }
    if (this.month == 12) {
      this.month = 0;
      this.year++;
    }
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
