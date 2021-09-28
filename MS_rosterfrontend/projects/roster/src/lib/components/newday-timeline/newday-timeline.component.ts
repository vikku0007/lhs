import { Component, OnInit, ViewChild, OnChanges, SimpleChanges, AfterViewInit, Input, Output, EventEmitter } from '@angular/core';
import { DayPilotSchedulerComponent, DayPilot } from 'daypilot-pro-angular';
import { RosterService } from '../../services/roster.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ShiftInfoViewModel } from '../../viewmodel/roster-shift-info-viewModel';
import { NotificationService } from 'projects/core/src/projects';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';

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
}
interface searchFilters {
  searchByEmpName?: number;
  searchByClientName?: number;
  searchBylocation?: number;
  searchByStatus?: number;
  searchByShiftType?: number;
  searchBymanualAddress?: string;
}

interface Resources {
  name?: string;
  id: string | number;
  expanded?: boolean;
  children?: Resources[];
}

@Component({
  selector: 'lib-newday-timeline',
  templateUrl: './newday-timeline.component.html',
  styleUrls: ['./newday-timeline.component.scss']
})
export class NewdayTimelineComponent implements OnInit, OnChanges, AfterViewInit {
  @ViewChild('scheduler', { static: false })
  scheduler: DayPilotSchedulerComponent;
  events: any[] = [];
  @Input() empModel: employeeModel[];
  @Output() runningDay = new EventEmitter<string>();
  shiftMod: Shifts = {};
  responseModel: ResponseModel = {};
  shiftModelArray: ShiftInfoViewModel[] = [];
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
  shiftsArray: Shifts[] = [];
  dayVal: string;
  employeeModel: Employee = {};
  employeeArray: any[] = [];
  clipboard: DayPilot.Event[] = [];
  config: DayPilot.SchedulerConfig = {
    timeHeaders: [{ "groupBy": "Day" }, { "groupBy": "Hour" }],
    scale: 'Hour',
    startDate: DayPilot.Date.today(),
    days: 1,
    treeEnabled: true,
    eventMovingStartEndEnabled: true,
    eventResizingStartEndEnabled: true,
    timeRangeSelectingStartEndEnabled: true,
    timeRangeSelectedHandling: "Enabled",
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
    eventClickHandling: "Disabled",
    eventHoverHandling: "Bubble",
    bubble: new DayPilot.Bubble({
      onLoad: args => {
        let event = args.source;
        var shiftId = event.data.id.split('_')[0];
        args.html = "<h2>" + event.data.resourceName + "</h2>";
        args.html += "<h3><a href='/roster/view-shift?Id=" + shiftId + "'>" + event.text() + "</a></h3>";
        args.html += "<p><b>Date</b> " + event.start().toString("MMMM d, yyyy") + "<b> Time </b>" + event.data.startTime + " to " + event.data.endTime + "</p>";
        args.html += "<p><b>Location </b>" + event.data.location + "</p>";
        args.html += "<p><b>Staff</b></p>";
        let staffName = '';
        let clientName = '';
        event.data.employeeShiftInfo.forEach(staff => {
          staffName = staff.name + ' | ';
        });
        args.html += staffName;
        args.html += event.data.duration + ' Hrs';
        args.html += "<p><b>Clients</b></p>";
        event.data.clientShiftInfo.forEach(client => {
          clientName = client.name;
        });
        args.html += clientName;
        args.html += '<a class="edit-icon" href="/roster/edit-shift?Id=' + shiftId + '"><svg height="16px" id="Layer_1" style="enable-background:new 0 0 16 16;" version="1.1" viewBox="0 0 16 16" width="16px" xml:space="preserve" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"><path d="M2.453,9.297C1.754,9.996,1,13.703,1,14c0,0.521,0.406,1,1,1c0.297,0,4.004-0.754,4.703-1.453l5.722-5.722l-4.25-4.25  L2.453,9.297z M12,1c-0.602,0-1.449,0.199-2.141,0.891L9.575,2.175l4.25,4.25l0.284-0.284C14.746,5.504,15,4.695,15,4  C15,2.343,13.656,1,12,1z"/></svg></a>';
      }
    }),
    cellBubble: new DayPilot.Bubble({
      onLoad: function (args) {
        let event = args.source;
        args.html = '<a href="/roster/add-shift"><svg id="Group_66134" data-name="Group 66134" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 20 20"><rect id="Rectangle_117" data-name="Rectangle 117" width="20" height="20" rx="4" fill="#8d36b8"/><path id="iconfinder_add_126583_3_" data-name="iconfinder_add_126583 (3)" d="M11.25,4.5H7.5V.75A.752.752,0,0,0,6.75,0H5.25A.752.752,0,0,0,4.5.75V4.5H.75A.752.752,0,0,0,0,5.25v1.5a.752.752,0,0,0,.75.75H4.5v3.75a.752.752,0,0,0,.75.75h1.5a.752.752,0,0,0,.75-.75V7.5h3.75A.752.752,0,0,0,12,6.75V5.25A.752.752,0,0,0,11.25,4.5Z" transform="translate(4 4)" fill="#fff"/></svg></a>';
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
                startDate: e.start(),
                endDate: e.end(),
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
  };

  constructor(private rosterService: RosterService, private notificationService: NotificationService,
    private loaderService: LoaderService) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.empModel !== undefined) {
      for (let i = 0; i < this.empModel.length; i++) {
        this.employeeModel = {};
        this.employeeModel.id = this.empModel[i].id;
        this.employeeModel.name = this.empModel[i].fullName;
        this.employeeArray.push(this.employeeModel);
      }
    }
  }

  ngOnInit(): void {
    this.dayVal = new DayPilot.Date().toString('dd/MM/yyyy');
  }

  ngAfterViewInit(): void {
    const fromDate = this.scheduler.control.visibleStart();
    const toDate = this.scheduler.control.visibleStart();
    this.getAllShiftsDateWise(fromDate, toDate);
  }

  schedulerViewChanged(args) {
    if (args.visibleRangeChanged) {
      const fromDate = this.scheduler.control.visibleStart();
      const toDate = this.scheduler.control.visibleStart();
      this.getAllShiftsDateWise(fromDate, toDate);
    }
  }

  previous() {
    this.config.startDate = new DayPilot.Date(this.config.startDate).addDays(-1);
    this.dayVal = this.config.startDate.toString('dd/MM/yyyy');
    this.getAllShiftsDateWise(this.config.startDate, this.config.startDate);
  }

  next() {
    this.config.startDate = new DayPilot.Date(this.config.startDate).addDays(1);
    this.dayVal = this.config.startDate.toString('dd/MM/yyyy');
    this.getAllShiftsDateWise(this.config.startDate, this.config.startDate);
  }

  showToday() {
    this.config.startDate = DayPilot.Date.today();
    this.dayVal = this.config.startDate.toString('dd/MM/yyyy');
    this.getAllShiftsDateWise(this.config.startDate, this.config.startDate);
  }

  searchDayShifts(data: searchFilters) {
    this.searchData.searchByClientName = data.searchByClientName;
    this.searchData.searchByEmpName = data.searchByEmpName;
    this.searchData.searchByShiftType = data.searchByShiftType;
    this.searchData.searchByStatus = data.searchByStatus;
    this.searchData.searchBylocation = data.searchBylocation;
    this.searchData.searchBymanualAddress = data.searchBymanualAddress;
    const args = { visibleRangeChanged: true }
    this.schedulerViewChanged(args);
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
            for (let j = 0; j < this.shiftModelArray[i].employeeShiftInfoViewModel.length; j++) {
              this.shiftMod = {};
              this.shiftMod.id = this.shiftModelArray[i].id + '_' + this.shiftModelArray[i].employeeShiftInfoViewModel[j].employeeId;
              this.shiftMod.resource = this.shiftModelArray[i].employeeShiftInfoViewModel[j].employeeId;
              this.shiftMod.resourceName = this.shiftModelArray[i].employeeShiftInfoViewModel[j].name;
              this.shiftMod.start = this.shiftModelArray[i].startDate;
              this.shiftMod.end = this.shiftModelArray[i].endDate;
              this.shiftMod.color = '#e69138';
              this.shiftMod.status = this.shiftModelArray[i].statusName;
              this.shiftMod.text = this.shiftModelArray[i].description;
              this.shiftMod.duration = this.shiftModelArray[i].duration;
              this.shiftMod.location = this.shiftModelArray[i].locationName;
              this.shiftMod.startTime = this.shiftModelArray[i].startTimeString;
              this.shiftMod.endTime = this.shiftModelArray[i].endTimeString;
              this.shiftMod.employeeShiftInfo = this.shiftModelArray[i].employeeShiftInfoViewModel;
              this.shiftMod.clientShiftInfo = this.shiftModelArray[i].clientShiftInfoViewModel;
              this.shiftsArray.push(this.shiftMod);
            }
          }

          for (let j = 0; j < this.shiftsArray.length; j++) {
            this.resource = { id: '', children: [{ id: '' }] };
            const index = this.checkLocation(this.shiftsArray[j].location, this.resources);
            if (index > -1) {
              this.resources[index].children.push({ id: this.shiftsArray[j].resource, name: this.shiftsArray[j].resourceName })
            } else {
              this.resource.id = j;
              this.resource.expanded = true;
              this.resource.name = this.shiftsArray[j].location;
              this.resource.children[0].id = this.shiftsArray[j].resource;
              this.resource.children[0].name = this.shiftsArray[j].resourceName;
              this.resources.push(this.resource);
            }
          }

          this.resource = { id: '', children: [{ id: '' }] };
          this.resource.id = 100;
          this.resource.expanded = true;
          this.resource.name = 'Other';

          for (let i = 0; i < this.empModel.length; i++) {
            this.resource = { id: '', children: [{ id: '' }] };
            const index = this.shiftsArray.findIndex(x => x.resource == this.empModel[i].id);
            if (index == -1) {
              const indices = this.checkLocation('Other', this.resources);
              if (indices > -1) {
                this.resources[indices].children.push({ id: this.empModel[i].id, name: this.empModel[i].fullName })
              } else {
                this.resource.id = this.resources.length + (i + 1);
                this.resource.expanded = true;
                this.resource.name = 'Other';
                this.resource.children[0].id = this.empModel[i].id;
                this.resource.children[0].name = this.empModel[i].fullName;
                this.resources.push(this.resource);
              }
            }
          }

          this.config.resources = this.resources;
          this.events = this.shiftsArray;
          this.runningDay.emit(this.dayVal);
          break;
        case 0:
          this.events = [];
          this.runningDay.emit(this.dayVal);
          this.notificationService.Warning({ message: this.responseModel.message, title: '' });
          break;

        default:
          break;
      }
    });
  }

  checkLocation(location: string, shifts: Resources[]) {
    const index = shifts.findIndex(x => x.name.toLowerCase() == location.toLowerCase());
    return index;
  }

}
