import { Component, OnInit } from '@angular/core';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Shiftinfo } from '../../viewmodels/shiftinfo';
import { ActivatedRoute } from '@angular/router';
import { EmpDashboardService } from '../../services/emp-dashboard.service';
import { NotificationService } from 'projects/core/src/projects';
import { ShiftHistoryViewModel, ShiftTodoListViewModel } from '../../viewmodels/shift-history-viewModel';
import { ToDoShiftViewModel } from 'projects/roster/src/lib/viewmodel/shift-history-viewModel';

@Component({
  selector: 'lib-shift-detail',
  templateUrl: './shift-detail.component.html',
  styleUrls: ['./shift-detail.component.scss']
})
export class ShiftDetailComponent implements OnInit {
  shifthistory: ShiftHistoryViewModel ={};
  todoshift: ToDoShiftViewModel ={};
  shifttodolist: ShiftTodoListViewModel  [];
  progressNotesFields: ClientProgressNotesFields = {};
  progressNotesInfo: ClientProgressNotesFields = {};
  response: ResponseModel = {};
  shiftId: number = 0;
  shiftInfo: Shiftinfo = {}
 
  constructor(private route: ActivatedRoute, private empService: EmpDashboardService, private notificationService: NotificationService) {
    this.route.paramMap.subscribe((params: any) => {
      this.shiftId = params.params.id;
    });
  }

  ngOnInit(): void {
    this.getShiftInfo();
    this.getShiftHistory();
  }

  getShiftInfo() {
    const data = {
      id: this.shiftId
    }
    this.empService.getShiftDetail(data).subscribe((res => {
      if (res) {
        this.response = res;
        this.shiftInfo = this.response.responseData || {}
      } else {
        this.notificationService.Error({ message: 'Something went wrong! Shift not found', title: null });
      }
    }));
  }
  getShiftHistory()
  {
    const data = {
      ShiftId: Number(this.shiftId)
    }
    this.empService.GetShiftHistory(data).subscribe((res=>{
      if(res){
        this.response = res;
       if(this.response.responseData!=null){
        this.shifthistory=this.response.responseData.shiftHistory;
        this.todoshift=this.response.responseData.toDoShift;
        this.shifttodolist=this.response.responseData.shiftToDoList;
        this.progressNotesInfo = this.response.responseData.progressNotesList;
      }
      }else{
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