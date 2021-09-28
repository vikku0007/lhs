import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ShiftInfoViewModel } from '../../viewmodel/roster-shift-info-viewModel';
import { ActivatedRoute } from '@angular/router';
import { RosterService } from '../../services/roster.service';
import { ShiftHistoryViewModel, ToDoShiftViewModel, ShiftTodoListViewModel } from '../../viewmodel/shift-history-viewModel';

@Component({
  selector: 'lib-view-shift',
  templateUrl: './view-shift.component.html',
  styleUrls: ['./view-shift.component.scss']
})
export class ViewShiftComponent implements OnInit {
  response: ResponseModel = {};
  shiftId: number = 0;
  shiftInfo: ShiftInfoViewModel = {}
  employeeId: number;
  shifthistory: ShiftHistoryViewModel = {};
  todoshift: ToDoShiftViewModel = {};
  shifttodolist: ShiftTodoListViewModel[] = [];
  progressNotesFields: ClientProgressNotesFields = {};
  progressNotesInfo: ClientProgressNotesFields = {};
  constructor(private route: ActivatedRoute, private rosterService: RosterService, private fb: FormBuilder, private commonService: CommonService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.shiftId = parseInt(params['Id']);
      if (this.shiftId && this.shiftId > 0) {
        this.getShiftInfo(this.shiftId);
        this.getShiftHistory();
      }
    });
  }

  getShiftInfo(Id) {
    this.rosterService.GetShiftDetail(Id).subscribe((res => {
      if (res) {
        this.response = res;
        this.shiftInfo = this.response.responseData || {}
        this.shiftInfo.locationName = this.shiftInfo.locationName === null ? this.shiftInfo.otherLocation : this.shiftInfo.locationName;
      } else {
        this.notificationService.Error({ message: 'Something went wrong! Shift not found', title: null });
      }
    }));
  }
  getShiftHistory() {
    const data = {
      ShiftId: this.shiftId
    }
    this.rosterService.GetShiftHistory(data).subscribe((res => {
      if (res) {
        this.response = res;
        if (this.response.responseData != null) {
          this.shifthistory = this.response.responseData.shiftHistory;
          this.todoshift = this.response.responseData.toDoShift;
          this.shifttodolist = this.response.responseData.shiftToDoList;
          this.progressNotesInfo = this.response.responseData.progressNotesList;
        }
      } else {
        this.notificationService.Error({ message: 'Something went wrong!', title: null });
      }
    }));
  }
  openEditviewDetails(data) {
    document.getElementById("openViewModalButton").click();
    this.progressNotesFields = data;
  }



}
export interface ClientProgressNotesFields {
  date?: Date;
  note9AMTo11AM?: string;
  note11AMTo1PM?: string;
  note1PMTo15PM?: string;
  note15PMTo17PM?: string;
  note17PMTo19PM?: string;
  note19PMTo21PM?: string;
  note21PMTo23PM?: string;
  note23PMTo1AM?: string;
  note1AMTo3AM?: string;
  note3AMTo5AM?: string;
  note5AMTo7AM?: string;
  note7AMTo9AM?: string;
  summary?: string;
  communityAccess?: string;
  culturalNeeds?: string;
  behaviourConcern?: string;
  medicationgiven?: string;
  anyFalls?: string;
  mobilitySafety?: string;
  nutritionalDetail?: string;
  exerciseDone?: string;
  otherInfo?: string;
  id?: number;
  clientId?: number;
  clientProgressNoteId?: number;
}