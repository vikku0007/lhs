import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CommonService } from 'projects/lhs-service/src/projects';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { MembershipService, NotificationService } from 'projects/core/src/projects';
import * as moment from 'moment';
import { LogoutService } from '../../services/logout.service';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { ActivatedRoute } from '@angular/router';

// For Appraisal Type
interface AppraisalType {
  value: string;
  name: string;
}

//For Discussion
export interface PeriodicElementStandard {
  description: string;
}

export interface TimeList {
  id?: string;
  text?: string;
}

@Component({
  selector: 'lib-to-do-list',
  templateUrl: './to-do-list.component.html',
  styleUrls: ['./to-do-list.component.scss'],
  providers: [
    {
      provide: DateAdapter, useClass: AppDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
  ]
})
export class ToDoListComponent implements OnInit {
  getErrorMessage: 'Please Enter Value';
  responseModel: ResponseModel = {};
  ShiftItemList = [];
  displayedColumnsStandard: string[] = ['description', 'tick1', 'initials'];
  dataSourceStandard = new MatTableDataSource(this.ShiftItemList);
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  ShifttimeList: any;
  isShownAMShift: boolean = false;
  isShownPMShift: boolean = false;
  isShownActive: boolean = false;
  TimeDropdownList: TimeList[] = ELEMENT_DATA;
  todoStandardsArray: TodolistStandards[] = [];
  todoStandards: TodolistStandards = {};
  datasourcelistdata: checkList[];
  initialsArray: { description?: string, notes?: string }[] = [];
  receivesArray: { description?: string }[] = [];
  @ViewChild('formDirective') private formDirective: NgForm;
  EditTodoList = [];
  list: any;
  selectedType: any;
  selectedName: any;
  totalCount: any;
  rForm: FormGroup;
  employeeId: any;
  shiftId: any;
  clientId: number;
  allSelected: any;
  selecttextName: string;
  data: any[];

  constructor(private route: ActivatedRoute, private commonService: CommonService, private membershipService: MembershipService,
    private fb: FormBuilder, private notificationService: NotificationService, private logoutService: LogoutService) {
    this.route.paramMap.subscribe((params: any) => {
      // this.clientId = Number(params.params.id);
      this.shiftId = Number(params.params.shiftId);
    });
  }
  ngOnInit(): void {
    this.dataSourceStandard.sort = this.sort;
    this.employeeId = this.membershipService.getUserDetails('employeeId');
    this.createForm();
    this.getshiftTime();    
    this.selecttextName = "Select All";
  }
  createForm() {
    this.rForm = this.fb.group({
      TodoShift: ['', Validators.required],
      TodoDate: ['', Validators.required],
      TodoTime: ['', Validators.required],
      Isinitials: ['', Validators.nullValidator],
      initials: ['', Validators.nullValidator],
      selectbox: ['', Validators.nullValidator]
    });
  }
  getshiftTime() {
    this.commonService.GetShiftTime().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.ShifttimeList = this.responseModel.responseData || [];
      } else {

      }
    }));
  }
  SelectShift(event: any) {
    this.list = this.ShifttimeList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedName = this.list[index].codeDescription;
    this.getShiftITemList();
    this.isShownAMShift = true;
    this.rForm.get('initials').disable();

  }
  removeSpace(value) {

    let a = value.replace(/\s/g, '');
    return a.toLowerCase();
  }
  getShiftITemList() {
    const data = {
      'ShiftType': Number(this.selectedType),
    }
    this.commonService.GetShiftItemList(data).subscribe((res: any) => {
      this.totalCount = res.total;
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.ShiftItemList = this.responseModel.responseData;
          this.dataSourceStandard = new MatTableDataSource(this.ShiftItemList);
          break;
        default:
          this.dataSourceStandard = new MatTableDataSource(this.ShiftItemList);
          break;
      }
    });
  }

  getInitials(data: any, isChecked: any) {
    if (isChecked.currentTarget.checked) {
      const index = this.ShiftItemList.findIndex(x => x.description === data.description);
      if (index > -1) {

      }
      this.initialsArray.push({ description: data.description });
      (document.getElementById(this.removeSpace(data.description) + 'initials') as HTMLInputElement).disabled = false;
    }
    else {
      const index = this.initialsArray.findIndex(x => x.description === data.description);
      this.initialsArray.splice(index, 1);
    }
    console.log(this.initialsArray);
  }

  removeData(description: string) {
    const indexacheivesArray = this.initialsArray.findIndex(x => x.description === description);
    if (indexacheivesArray > -1) {
      this.initialsArray.splice(indexacheivesArray, 1);
    }
    const indexbelowArray = this.receivesArray.findIndex(x => x.description === description);
    if (indexbelowArray > -1) {
      this.initialsArray.splice(indexbelowArray, 1);
    }

  }
  AddToDoDetails() {
    if (this.rForm.valid) {
      for (let i = 0; i < this.initialsArray.length; i++) {
        this.todoStandards = {};
        const index = this.todoStandardsArray.findIndex(x => x.Description === this.initialsArray[i].description);
        if (index > -1) {
          this.todoStandardsArray[index].IsInitials = true;
        } else {
          this.todoStandards.Description = this.initialsArray[i].description;
          this.todoStandards.IsInitials = true;
          this.todoStandards.EmployeeId = this.employeeId;
          // this.todoStandards.ClientId = this.clientId;
          this.todoStandards.Initials = (document.getElementById(this.removeSpace(this.initialsArray[i].description) + 'initials') as HTMLInputElement).value;
          this.todoStandardsArray.push(this.todoStandards);
        }
      }

      const data = {
        EmployeeId: this.employeeId,
        ShiftId: this.shiftId,
        // ClientId: this.clientId,
        ShiftType: Number(this.rForm.get('TodoShift').value),
        DateTime: moment(this.rForm.get('TodoDate').value).format('YYYY-MM-DD'),
        ShiftTime: (this.rForm.get('TodoTime').value),
        ShiftToDoListItem: this.todoStandardsArray
      };

      this.logoutService.AddToDoList(data).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: 'To Do List added successfully.', title: '' });
            this.rForm.reset();
            this.formDirective.resetForm();
            break;
          default:
            break;
        }
      });
    }
  }
  toggleAllSelection() {
    this.data = this.dataSourceStandard.filteredData;
    if (this.rForm.controls.Isinitials.value == true) {
      this.rForm.controls.Isinitials.patchValue(false);
      this.rForm.get('initials').disable();

    }
    else {
      this.rForm.controls.Isinitials.patchValue(true);
      this.rForm.get('initials').enable();
      for (let i = 0; i < this.data.length; i++) {
        this.initialsArray.push({ description: this.data[i].description });
      }
    }

  }
  
}
export interface TodolistStandards {
  Id?: number;
  EmployeeId?: number;
  IsInitials?: boolean;
  Initials?: string;
  IsReceived?: boolean;
  Received?: string;
  Description?: string;
  ClientId?: number;
}
export interface checkList {
  Id?: number;
  Description?: string;
  ShiftType?: number;

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