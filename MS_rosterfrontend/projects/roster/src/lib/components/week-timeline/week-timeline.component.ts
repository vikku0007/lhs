import { Component, OnInit, ViewChild, Input, OnChanges, SimpleChanges, Output, EventEmitter, ElementRef } from '@angular/core';
import { DayPilotSchedulerComponent, DayPilot } from 'daypilot-pro-angular';
import { RosterService } from '../../services/roster.service';
import { ShiftInfoViewModel, CalendarShiftInfoViewModel } from '../../viewmodel/roster-shift-info-viewModel';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { NotificationService } from 'projects/core/src/projects';
import { MatTableDataSource } from '@angular/material/table';

interface employeeModel {
  id?: number;
  fullName?: string;
}
interface Employee {
  id?: number;
  name?: string;
}
interface Shifts {
  id?: string;
  resource?: number;
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
  isShiftCompleted?: boolean;
  customDuration?: string;
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
  selector: 'lib-week-timeline',
  templateUrl: './week-timeline.component.html',
  styleUrls: ['./week-timeline.component.scss']
})

export class WeekTimelineComponent implements OnInit, OnChanges {
  @ViewChild('scheduler', { static: false })
  scheduler: DayPilotSchedulerComponent;
  events: any[] = [];
  @Input() shiftModel: CalendarShiftInfoViewModel[];
  @Input() empModel: employeeModel[];
  @Output() runningWeek = new EventEmitter<string>();
  employeeArray: any[] = [];
  employeeModel: Employee = {};
  shiftsArray: any[] = [];
  shiftMod: Shifts = {};
  responseModel: ResponseModel = {};
  shiftModelArray: CalendarShiftInfoViewModel[] = [];
  weekVal: string;
  searchData: searchFilters = {
    searchByClientName: 0,
    searchByEmpName: 0,
    searchByShiftType: 0,
    searchByStatus: 0,
    searchBylocation: 0,
    searchBymanualAddress: ''
  };
  @ViewChild('btnLoadTemplateWeekly') btnLoadShiftResult: ElementRef;
  dataSourceWeekly: any;
  shiftResponse: any;
  displayedColumns: string[] = ['description', 'date', 'time', 'employeeName', 'action'];

  constructor(private rosterService: RosterService, private loaderService: LoaderService, private notificationService: NotificationService) { }

  ngOnChanges(changes: SimpleChanges): void {
    // this.loaderService.start();
    this.shiftsArray = [];
    this.employeeArray = [];
    // if (this.shiftModel !== undefined && this.shiftModel.length > 0) {
    //   for (let i = 0; i < this.shiftModel.length; i++) {
    //     for (let j = 0; j < this.shiftModel[i].employeeShiftInfoViewModel.length; j++) {
    //       this.shiftMod = {};
    //       this.shiftMod.id = this.shiftModel[i].id + '_' + this.shiftModel[i].employeeShiftInfoViewModel[j].employeeId;
    //       this.shiftMod.resource = this.shiftModel[i].employeeShiftInfoViewModel[j].employeeId;
    //       this.shiftMod.start = this.shiftModel[i].startDate;
    //       this.shiftMod.end = this.shiftModel[i].endDate;
    //       this.shiftMod.color = '#e69138';
    //       this.shiftMod.text = this.shiftModel[i].description;
    //       this.shiftMod.status = this.shiftModelArray[i].statusName;
    //       this.shiftsArray.push(this.shiftMod);
    //     }
    //   }
    // }

    if (this.empModel !== undefined && this.empModel.length > 0) {
      for (let i = 0; i < this.empModel.length; i++) {
        this.employeeModel = {};
        this.employeeModel.id = this.empModel[i].id;
        this.employeeModel.name = this.empModel[i].fullName;
        this.employeeArray.push(this.employeeModel);
      }
    }
    if (this.empModel.length > 0) {
      // this.loaderService.stop();
      this.configWeek.resources = this.employeeArray;
      this.events = this.shiftsArray;
    }
  }

  ngOnInit(): void {
    // this.searchData.searchByClientName = 0;
    // this.searchData.searchByEmpName = 0;
    // this.searchData.searchByShiftType = 0;
    // this.searchData.searchByStatus = 0;
    // this.searchData.searchBylocation = 0;
    // this.searchData.searchBymanualAddress = '';
    this.dataSourceWeekly = new MatTableDataSource(this.shiftResponse);
  }

  ngAfterViewInit(): void {

  }

  clipboard: DayPilot.Event[] = [];

  configWeek: DayPilot.SchedulerConfig = {
    // viewType: "Week",
    cellWidthSpec: "Fixed",
    cellWidth: 100,
    timeHeaders: [
      {
        "groupBy": "Month"
      },
      {
        "groupBy": "Day",
        "format": "d"
      }
    ],
    scale: "Day",
    days: 7,
    startDate: DayPilot.Date.today().firstDayOfWeek(),
    timeRangeSelectedHandling: "Enabled",
    // onTimeRangeSelected: function (args) {
    //   var dp = this;
    //   DayPilot.Modal.prompt("Create a new event:", "Event 1").then(function (modal) {
    //     dp.clearSelection();
    //     if (!modal.result) { return; }
    //     dp.events.add(new DayPilot.Event({
    //       start: args.start,
    //       end: args.end,
    //       id: DayPilot.guid(),
    //       resource: args.resource,
    //       text: modal.result
    //     }));
    //   });
    // },
    eventMoveHandling: "Notify",
    onEventMove: args => {
      const params = {
        shiftId: Number(args.e.data.id.split('_')[0]),
        startDate: args.newStart,
        endDate: args.newEnd,
        employeeId: Number(args.newResource),
        // duration: args.e.duration().ticks
      };
      this.rosterService.DragDropShift(params).subscribe((result: any) => {
        const args = { visibleRangeChanged: true }
        this.schedulerViewChanged(args);
        this.notificationService.Success({ message: 'Shift updated successfully', title: '' });
      });
    },
    eventResizeHandling: "Disabled",
    onEventResized: function (args) {
      this.message("Event resized: " + args.e.text());
    },
    eventDeleteHandling: "CallBack",
    onEventDelete: args => {
      DayPilot.Modal.confirm("Are you sure you want to delete this shift?").then(res => {
        if (res.result) {
          const id = Number(args.e.data.id.split('_')[0]);
          this.rosterService.DeleteShiftInfo(id).subscribe((result: any) => {
            this.responseModel = result;
            if (this.responseModel.status == 1) {
              this.notificationService.Success({ message: 'Shift deleted successfully', title: '' });
              const args = { visibleRangeChanged: true }
              this.schedulerViewChanged(args);
            } else if (this.responseModel.status == 0) {
              this.notificationService.Success({ message: 'There is some issue while deleting', title: '' });
            }
          });
        }
      });
    },
    onBeforeEventRender: args => {
      // const index = args.data.employeeShiftInfo.findIndex(x => x.employeeId == args.data.resource);
      // args.data.cssClass = "shift-" + args.data.employeeShiftInfo[index].statusName.toLowerCase();
      args.data.cssClass = "shift-" + args.data.status.toLowerCase();
    },
    eventClickHandling: "Disabled",
    eventHoverHandling: "Bubble",
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
        args.html += "<p><b>Duration </b>" + event.data.customDuration + '</p>';
        if (!event.data.isShiftCompleted) {
          args.html += '<a *ngIf="!event.data.isShiftCompleted" title="Edit Shift" class="edit-icon" href="/roster/edit-shift?Id=' + shiftId + '"><svg height="16px" id="Layer_1" style="enable-background:new 0 0 16 16;" version="1.1" viewBox="0 0 16 16" width="16px" xml:space="preserve" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"><path d="M2.453,9.297C1.754,9.996,1,13.703,1,14c0,0.521,0.406,1,1,1c0.297,0,4.004-0.754,4.703-1.453l5.722-5.722l-4.25-4.25  L2.453,9.297z M12,1c-0.602,0-1.449,0.199-2.141,0.891L9.575,2.175l4.25,4.25l0.284-0.284C14.746,5.504,15,4.695,15,4  C15,2.343,13.656,1,12,1z"/></svg></a>';
        }
      }
    }),
    cellBubble: new DayPilot.Bubble({
      onLoad: function (args) {
        let event = args.source;
        args.html = '<a title="Add Shift" href="/roster/add-shift"><svg id="Group_66134" data-name="Group 66134" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 20 20"><rect id="Rectangle_117" data-name="Rectangle 117" width="20" height="20" rx="4" fill="#8d36b8"/><path id="iconfinder_add_126583_3_" data-name="iconfinder_add_126583 (3)" d="M11.25,4.5H7.5V.75A.752.752,0,0,0,6.75,0H5.25A.752.752,0,0,0,4.5.75V4.5H.75A.752.752,0,0,0,0,5.25v1.5a.752.752,0,0,0,.75.75H4.5v3.75a.752.752,0,0,0,.75.75h1.5a.752.752,0,0,0,.75-.75V7.5h3.75A.752.752,0,0,0,12,6.75V5.25A.752.752,0,0,0,11.25,4.5Z" transform="translate(4 4)" fill="#fff"/></svg></a>';
      }
    }),
    contextMenu: new DayPilot.Menu({
      onShow: args => {
        if (!this.scheduler.control.multiselect.isSelected(args.source)) {
          this.scheduler.control.multiselect.clear();
          this.scheduler.control.multiselect.add(args.source);
        }
      },
      items: [
        {
          text: "Copy",
          onClick: args => {
            let selected = this.scheduler.control.multiselect.events();
            this.clipboard = selected.sort((e1, e2) => e1.start().getTime() - e2.start().getTime());
          }
        }
      ]
    }),
    contextMenuSelection: new DayPilot.Menu({
      onShow: args => {
        let noItemsInClipboard = this.clipboard.length === 0;
        args.menu.items[0].disabled = noItemsInClipboard;
      },
      items: [
        {
          text: "Paste",
          onClick: args => {
            if (this.clipboard.length === 0) {
              return;
            }

            let targetStart = args.source.start;
            let targetResource = args.source.resource;
            let firstStart = this.clipboard[0].start();

            this.scheduler.control.clearSelection();

            this.clipboard.forEach(e => {
              let offset = new DayPilot.Duration(firstStart, e.start());
              let duration = e.duration().ticks;
              let start = targetStart.addTime(offset);
              let end = start.addTime(duration);

              let params = {
                shiftId: Number(this.clipboard[0].data.id.split('_')[0]),
                startDate: targetStart,
                endDate: targetStart,
                employeeId: targetResource,
                // duration: duration
              };
              this.rosterService.AddCopyPasteShift(params).subscribe((result: any) => {
                const args = { visibleRangeChanged: true }
                this.schedulerViewChanged(args);
                this.notificationService.Success({ message: 'Shift added successfully', title: '' });
              });
            });
          }
        }
      ]
    }),
    treeEnabled: true,
    rowHeaderHideIconEnabled: true
  }

  schedulerViewChanged(args) {
    if (args.visibleRangeChanged) {
      const fromDate = this.scheduler.control.visibleStart();
      const toDate = this.scheduler.control.visibleEnd().addDays(-1);
      this.weekVal = fromDate.toString('dd/MM/yyyy') + ' to ' + toDate.toString('dd/MM/yyyy');
      this.getAllShiftsDateWise(fromDate, toDate);
    }
  }

  previous() {
    this.configWeek.startDate = new DayPilot.Date(this.configWeek.startDate).addDays(-7);
  }

  next() {
    this.configWeek.startDate = new DayPilot.Date(this.configWeek.startDate).addDays(7);
  }

  showCurrentWeek() {
    this.configWeek.startDate = DayPilot.Date.today().firstDayOfWeek();
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

    this.rosterService.GetEmployeeViewCalendar(data).subscribe((res: any) => {
      this.responseModel = res;
      this.shiftsArray = [];
      if (this.searchData.searchByEmpName > 0) {
        this.configWeek.resources = this.employeeArray.filter(x => x.id == this.searchData.searchByEmpName);
      } else {
        this.configWeek.resources = this.employeeArray;
      }
      switch (this.responseModel.status) {
        case 1:
          this.shiftModelArray = this.responseModel.responseData;
          for (let i = 0; i < this.shiftModelArray.length; i++) {
            // for (let j = 0; j < this.shiftModelArray[i].employeeShiftInfoViewModel.length; j++) {
            this.shiftMod = {};
            // this.shiftMod.id = this.shiftModelArray[i].id + '_' + this.shiftModelArray[i].employeeShiftInfoViewModel[j].employeeId;
            // this.shiftMod.resource = this.shiftModelArray[i].employeeShiftInfoViewModel[j].employeeId;
            // this.shiftMod.resourceName = this.shiftModelArray[i].employeeShiftInfoViewModel[j].name;
            this.shiftMod.id = this.shiftModelArray[i].id + '_' + this.shiftModelArray[i].employeeId;
            this.shiftMod.resource = this.shiftModelArray[i].employeeId;
            this.shiftMod.resourceName = this.shiftModelArray[i].name;
            this.shiftMod.start = this.shiftModelArray[i].startDate;
            this.shiftMod.end = this.shiftModelArray[i].endDate;
            this.shiftMod.color = '#e69138';
            this.shiftMod.status = this.shiftModelArray[i].statusName;
            this.shiftMod.text = this.shiftModelArray[i].description;
            this.shiftMod.duration = this.shiftModelArray[i].duration;
            this.shiftMod.location = this.shiftModelArray[i].locationName;
            this.shiftMod.startTime = this.shiftModelArray[i].startTimeString;
            this.shiftMod.endTime = this.shiftModelArray[i].endTimeString;
            this.shiftMod.isShiftCompleted = this.shiftModelArray[i].isShiftCompleted;
            this.shiftMod.customDuration = this.shiftModelArray[i].customDuration;
            // this.shiftMod.employeeShiftInfo = this.shiftModelArray[i].employeeShiftInfoViewModel;
            // this.shiftMod.clientShiftInfo = this.shiftModelArray[i].clientShiftInfoViewModel;
            this.shiftsArray.push(this.shiftMod);
            // }
          }
          this.events = this.shiftsArray;
          this.runningWeek.emit(this.weekVal);
          break;
        case 0:
          this.events = [];
          this.runningWeek.emit(this.weekVal);
          this.notificationService.Warning({ message: this.responseModel.message, title: '' });
          break;

        default:
          break;
      }
    });
  }

  searchWeeklyShifts(data: searchFilters) {
    this.searchData.searchByClientName = data.searchByClientName;
    this.searchData.searchByEmpName = data.searchByEmpName;
    this.searchData.searchByShiftType = data.searchByShiftType;
    this.searchData.searchByStatus = data.searchByStatus;
    this.searchData.searchBylocation = data.searchBylocation;
    this.searchData.searchBymanualAddress = data.searchBymanualAddress;
    const args = { visibleRangeChanged: true }
    this.schedulerViewChanged(args);
  }

  loadWeelyShiftFromTemplate(shifts: any) {
    this.shiftModelArray = shifts.shiftInfoViewModel;
    for (let i = 0; i < this.shiftModelArray.length; i++) {
      //  for (let j = 0; j < this.shiftModelArray[i].employeeShiftInfoViewModel.length; j++) {
      this.shiftMod = {};
      // this.shiftMod.id = this.shiftModelArray[i].id + '_' + this.shiftModelArray[i].employeeShiftInfoViewModel[j].employeeId;
      // this.shiftMod.resource = this.shiftModelArray[i].employeeShiftInfoViewModel[j].employeeId;
      // this.shiftMod.resourceName = this.shiftModelArray[i].employeeShiftInfoViewModel[j].name;
      this.shiftMod.id = this.shiftModelArray[i].id + '_' + this.shiftModelArray[i].employeeId;
      this.shiftMod.resource = this.shiftModelArray[i].employeeId;
      this.shiftMod.resourceName = this.shiftModelArray[i].name;
      this.shiftMod.start = this.shiftModelArray[i].startDate;
      this.shiftMod.end = this.shiftModelArray[i].endDate;
      this.shiftMod.status = this.shiftModelArray[i].statusName;
      this.shiftMod.text = this.shiftModelArray[i].description;
      this.shiftMod.duration = this.shiftModelArray[i].duration;
      this.shiftMod.location = this.shiftModelArray[i].locationName;
      this.shiftMod.startTime = this.shiftModelArray[i].startTimeString;
      this.shiftMod.endTime = this.shiftModelArray[i].endTimeString;
      // this.shiftMod.employeeShiftInfo = this.shiftModelArray[i].employeeShiftInfoViewModel;
      // this.shiftMod.clientShiftInfo = this.shiftModelArray[i].clientShiftInfoViewModel;
      this.shiftsArray.push(this.shiftMod);
      //  }
    }
    this.events = this.shiftsArray;
    this.btnLoadShiftResult.nativeElement.click();
    this.shiftResponse = shifts.shiftResponseList;
    this.dataSourceWeekly = new MatTableDataSource(this.shiftResponse);
  }

}
