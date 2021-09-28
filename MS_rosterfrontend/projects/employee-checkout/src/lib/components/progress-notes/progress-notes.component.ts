import { Component, OnInit, ViewChild, ElementRef, ViewChildren } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { LogoutService } from '../../services/logout.service';
import { FormBuilder, FormGroup, Validators, FormControl, NgForm } from '@angular/forms';
import { NotificationService, MembershipService } from 'projects/core/src/projects';
import { Paging } from 'projects/viewmodels/paging';
import { PageEvent } from '@angular/material/paginator';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ClientProgressNotes, ClientProgressNotesFields } from '../../view-models/client-progress-notes';
import { ClientDetails } from '../../view-models/add-client-details';
import * as moment from 'moment';
import { LogoutComponent } from '../logout/logout.component';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';

export interface ProgressNotesList {
  clientId?: number;
  id?: number;
  clientProgressNoteId?: number;
  date?: string;
  progressNote?: string;
}

@Component({
  selector: 'lib-progress-notes',
  templateUrl: './progress-notes.component.html',
  styleUrls: ['./progress-notes.component.scss'],
  providers: [
    {
      provide: DateAdapter, useClass: AppDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
  ]
})

export class ProgressNotesComponent implements OnInit {

  getErrorMessage: 'Please Enter Value';
  clientProgressNotes: ClientProgressNotes = {};
  progressNotesList: ProgressNotesList[] = [];
  displayedColumnsProgressNotes: string[] = ['date', 'progressNote', 'createdDate', 'action'];
  dataSourceProgressNotes = new MatTableDataSource(this.progressNotesList);
  paging: Paging = {};
  clientId: number;
  shiftId: number;
  orderBy: number;
  orderColumn: number;
  responseModel: ResponseModel = {};
  totalCount: number;
  deleteProgressNoteItemId: number;
  clientForm: FormGroup;
  clientInfo: ClientDetails = {};
  isEnabled = true;
  progressNotesFields: ClientProgressNotesFields = {};
  rFormEditProgressNotes: FormGroup;
  EditProgressNoteItemID: number;
  progressNoteId: number = 0;
  rFormProgressNotes: FormGroup;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('btnAddfundtype') cancelAdd: ElementRef;
  @ViewChild('btnEditfundtype') cancelEdit: ElementRef;
  @ViewChildren(LogoutComponent) logoutComponent: LogoutComponent;
  employeeId: any;

  constructor(private route: ActivatedRoute, private logoutService: LogoutService,
    private fb: FormBuilder, private notificationService: NotificationService,private membershipService: MembershipService) {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = params.params.id;
      this.shiftId = Number(params.params.shiftId);
    });
  }

  ngOnInit(): void {
   this.employeeId = this.membershipService.getUserDetails('employeeId');
   this.dataSourceProgressNotes.sort = this.sort;
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.createForm();
    this.createEditForm();
    this.createFormProgressNotes();
    this.getClientPrimaryInfo();
    this.getList();
    this.disableFields();
  }

  createForm() {
    this.clientForm = this.fb.group({
      patientName: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
      dateOfBirth1: [null, Validators.required],
      medicalNo: [this.clientProgressNotes.medicalRecordNo, Validators.nullValidator],
    });
  }

  createEditForm() {
    this.rFormEditProgressNotes = this.fb.group({
      editProgressDate: [null, Validators.required],
      editNote09AMto11AM: [null, Validators.nullValidator],
      editNote11AMto13PM: [null, Validators.nullValidator],
      editNote13PMto15PM: [null, Validators.nullValidator],
      editNote15PMto17PM: [null, Validators.nullValidator],
      editNote17PMto19PM: [null, Validators.nullValidator],
      editNote19toPM21PM: [null, Validators.nullValidator],
      editNote21PMto23PM: [null, Validators.nullValidator],
      editNote23PMto01AM: [null, Validators.nullValidator],
      editNote01AMto03AM: [null, Validators.nullValidator],
      editNote03AMto05AM: [null, Validators.nullValidator],
      editNote05AMto07AM: [null, Validators.nullValidator],
      editNote07AMto09AM: [null, Validators.nullValidator],
      // editNDISGoals: [null, Validators.nullValidator],
      editOtherInfo: [null, Validators.nullValidator]
    });
  }

  createFormProgressNotes() {
    this.rFormProgressNotes = this.fb.group({
      progressDate: ['', Validators.required],
      Note09AMto11AM: [null, Validators.nullValidator],
      Note11AMto13PM: [null, Validators.nullValidator],
      Note13PMto15PM: [null, Validators.nullValidator],
      Note15PMto17PM: [null, Validators.nullValidator],
      Note17PMto19PM: [null, Validators.nullValidator],
      Note19toPM21PM: [null, Validators.nullValidator],
      Note21PMto23PM: [null, Validators.nullValidator],
      Note23PMto01AM: [null, Validators.nullValidator],
      Note01AMto03AM: [null, Validators.nullValidator],
      Note03AMto05AM: [null, Validators.nullValidator],
      Note05AMto07AM: [null, Validators.nullValidator],
      Note07AMto09AM: [null, Validators.nullValidator],
     // NDISGoals: [null, Validators.nullValidator],
      OtherInfo: [null, Validators.nullValidator],

    });
  }

  disableFields() {
    this.clientForm.get('patientName').disable();
    this.clientForm.get('dateOfBirth1').disable();
    this.clientForm.get('medicalNo').disable();
  }

  getClientPrimaryInfo() {
    const data = {
      id: Number(this.clientId)
    }
    this.logoutService.getClientPrimaryInfo(data).subscribe((res: any) => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          this.clientInfo = this.responseModel.responseData;
          this.clientForm.controls['patientName'].patchValue(this.clientInfo.fullName);
          this.clientForm.controls['dateOfBirth1'].patchValue(this.clientInfo.dateOfBirth);
          this.clientForm.controls['medicalNo'].patchValue(this.clientInfo.clientId);
          break;
        case 0:
          this.notificationService.Error({ message: 'some Error occured', title: null });
          break;
        default:

          break;
      }
    })
  }

  getList() {
    const data = {
      ClientId: Number(this.clientId),
      ShiftId: Number(this.shiftId),
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.logoutService.getProgressNotesList(data).subscribe(res => {
      this.responseModel = res;
      this.totalCount = this.responseModel.total;
      if (this.responseModel.responseData != null) {
        this.progressNotesList = this.responseModel.responseData;
        this.dataSourceProgressNotes = new MatTableDataSource(this.progressNotesList);
      }
    });
  }

  DeleteModal(fundingtypeId, _e) {
    this.deleteProgressNoteItemId = fundingtypeId.id;
  }

  openEditDetails(elem) {
    document.getElementById("openEditModalButton").click();
    this.EditProgressNoteItemID = elem.id;
    this.rFormEditProgressNotes.controls['editProgressDate'].setValue(elem.date);
    this.rFormEditProgressNotes.controls['editNote09AMto11AM'].setValue(elem.note9AMTo11AM);
    this.rFormEditProgressNotes.controls['editNote11AMto13PM'].setValue(elem.note11AMTo1PM);
    this.rFormEditProgressNotes.controls['editNote13PMto15PM'].setValue(elem.note1PMTo15PM);
    this.rFormEditProgressNotes.controls['editNote15PMto17PM'].setValue(elem.note15PMTo17PM);
    this.rFormEditProgressNotes.controls['editNote17PMto19PM'].setValue(elem.note17PMTo19PM);
    this.rFormEditProgressNotes.controls['editNote19toPM21PM'].setValue(elem.note19PMTo21PM);
    this.rFormEditProgressNotes.controls['editNote21PMto23PM'].setValue(elem.note21PMTo23PM);
    this.rFormEditProgressNotes.controls['editNote23PMto01AM'].setValue(elem.note23PMTo1AM);
    this.rFormEditProgressNotes.controls['editNote01AMto03AM'].setValue(elem.note1AMTo3AM);
    this.rFormEditProgressNotes.controls['editNote03AMto05AM'].setValue(elem.note3AMTo5AM);
    this.rFormEditProgressNotes.controls['editNote05AMto07AM'].setValue(elem.note5AMTo7AM);
    this.rFormEditProgressNotes.controls['editNote07AMto09AM'].setValue(elem.note7AMTo9AM);
    // this.rFormEditProgressNotes.controls['editNDISGoals'].setValue(elem.summary);
    this.rFormEditProgressNotes.controls['editOtherInfo'].setValue(elem.otherInfo);

  }

  UpdateProgressNotesItem() {
    if (this.rFormEditProgressNotes.valid) {
      const data = {
        Id: this.EditProgressNoteItemID,
        ClientId: Number(this.clientId),
        ClientProgressNoteId: this.progressNoteId,
        Date: moment(this.rFormEditProgressNotes.get('editProgressDate').value).toDate(),
        Note9AMTo11AM: this.rFormEditProgressNotes.get('editNote09AMto11AM').value,
        Note11AMTo1PM: this.rFormEditProgressNotes.get('editNote11AMto13PM').value,
        Note1PMTo15PM: this.rFormEditProgressNotes.get('editNote13PMto15PM').value,
        Note15PMTo17PM: this.rFormEditProgressNotes.get('editNote15PMto17PM').value,
        Note17PMTo19PM: this.rFormEditProgressNotes.get('editNote17PMto19PM').value,
        Note19PMTo21PM: this.rFormEditProgressNotes.get('editNote19toPM21PM').value,
        Note21PMTo23PM: this.rFormEditProgressNotes.get('editNote21PMto23PM').value,
        Note23PMTo1AM: this.rFormEditProgressNotes.get('editNote23PMto01AM').value,
        Note1AMTo3AM: this.rFormEditProgressNotes.get('editNote01AMto03AM').value,
        Note3AMTo5AM: this.rFormEditProgressNotes.get('editNote03AMto05AM').value,
        Note5AMTo7AM: this.rFormEditProgressNotes.get('editNote05AMto07AM').value,
        Note7AMTo9AM: this.rFormEditProgressNotes.get('editNote07AMto09AM').value,
        // Summary: this.rFormEditProgressNotes.get('editNDISGoals').value,
        OtherInfo: this.rFormEditProgressNotes.get('editOtherInfo').value,

      };
      this.logoutService.updateProgressNotesList(data).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: 'Progress notes updated successfully', title: '' });
            this.rFormEditProgressNotes.reset();
            this.cancelEdit.nativeElement.click();
            const index = this.progressNotesList.findIndex(x => x.id == this.responseModel.responseData.id);
            this.progressNotesList[index] = this.responseModel.responseData;
            this.dataSourceProgressNotes = new MatTableDataSource(this.progressNotesList);
            break;
          case 0:
            this.notificationService.Warning({ message: 'Some error occured', title: '' });
          default:
            break;
        }
      });
    }
  }

  AddProgressNotesItem() {
    if (this.rFormProgressNotes.get('progressDate').value == null || this.rFormProgressNotes.get('progressDate').value == "") {
      this.notificationService.Warning({ message: 'Please fill Date', title: null });
      return;
    }
    else if (this.rFormProgressNotes.valid) {
      const data = {

        ClientId: Number(this.clientId),
        clientProgressNoteId: this.progressNoteId,
        date: moment(this.rFormProgressNotes.get('progressDate').value).format('YYYY-MM-DD'),
        Note9AMTo11AM: this.rFormProgressNotes.get('Note09AMto11AM').value,
        Note11AMTo1PM: this.rFormProgressNotes.get('Note11AMto13PM').value,
        Note1PMTo15PM: this.rFormProgressNotes.get('Note13PMto15PM').value,
        Note15PMTo17PM: this.rFormProgressNotes.get('Note15PMto17PM').value,
        Note17PMTo19PM: this.rFormProgressNotes.get('Note17PMto19PM').value,
        Note19PMTo21PM: this.rFormProgressNotes.get('Note19toPM21PM').value,
        Note21PMTo23PM: this.rFormProgressNotes.get('Note21PMto23PM').value,
        Note23PMTo1AM: this.rFormProgressNotes.get('Note23PMto01AM').value,
        Note1AMTo3AM: this.rFormProgressNotes.get('Note01AMto03AM').value,
        Note3AMTo5AM: this.rFormProgressNotes.get('Note03AMto05AM').value,
        Note5AMTo7AM: this.rFormProgressNotes.get('Note05AMto07AM').value,
        Note7AMTo9AM: this.rFormProgressNotes.get('Note07AMto09AM').value,
       // Summary: this.rFormProgressNotes.get('NDISGoals').value,
        OtherInfo: this.rFormProgressNotes.get('OtherInfo').value,
        ShiftId:this.shiftId,
        EmployeeId: this.employeeId
      }

      this.logoutService.addClientProgressNotes(data).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.rFormProgressNotes.reset();
            this.cancelAdd.nativeElement.click();
            //this.progressNoteId = this.responseModel.responseData.id;
            this.getList();
            this.notificationService.Success({ message: this.responseModel.message, title: null });
            this.logoutComponent.isLogoutDisabled = false;
            break;

          default:
            this.notificationService.Error({ message: this.responseModel.message, title: null });
            break;
        }
      });
    }

  }

  DeleteProgressNotes(event) {
    this.logoutService.deleteClientProgressNotesItem({ Id: this.deleteProgressNoteItemId }).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.getList();
      }
      else {
        this.notificationService.Error({ message: data.message, title: null });
      }

    })
  }

  openEditviewDetails(data) {
    document.getElementById("openViewModalButton").click();
    this.progressNotesFields = data;
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getList();
  }

  cancelModal() {
    this.rFormProgressNotes.reset();
    this.formDirective.resetForm();
  }

}
