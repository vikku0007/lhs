import { Component, OnInit, ViewChild , ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ClientService } from '../../services/client.service';
import { NotificationService } from 'projects/core/src/projects';
import { Paging } from 'projects/viewmodels/paging';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { ActivatedRoute } from '@angular/router';
import { ProgressNotesList } from '../../view-models/client-progress-notes-list';
import { ClientProgressNotes, ClientProgressNotesFields } from '../../view-models/client-progress-notes';
import { AddClientDetails } from '../../view-models/add-client-details';
import * as moment from 'moment';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { CommonService } from 'projects/lhs-service/src/projects';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
// For Progress Notes
export interface PeriodicElementProgressNotes {
  date: string;
  progressNote: string;
}

// For Education
const ELEMENT_DATA_PROGRESS_NOTES: PeriodicElementProgressNotes[] = [
  {date: 'Lorem Ipsum', progressNote: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.'},
  {date: 'Pretium Nunc', progressNote: 'Donec sit amet ligula posuere, malesuada turpis nec.'},
];

@Component({
  selector: 'lib-progress-notes-details',
  templateUrl: './progress-notes-details.component.html',
  styleUrls: ['./progress-notes-details.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})
export class ProgressNotesDetailsComponent implements OnInit , OnChanges {
  progressNotesFields: ClientProgressNotesFields = {};
  getErrorMessage:'Please Enter Value';
  clientForm: FormGroup;
  rFormProgressNotes: FormGroup;
  rFormEditProgressNotes: FormGroup;
  deleteProgressNoteItemId: number;
  clientInfo: AddClientDetails = {};
  // For Progress Notes
  displayedColumnsProgressNotes: string[] = ['date', 'progressNote','createdDate','action'];
  // dataSourceProgressNotes = new MatTableDataSource(ELEMENT_DATA_PROGRESS_NOTES);
  dataSourceProgressNotes : any;
  paging: Paging = {};
  totalCount: number;
  ClientId: number;
  EditProgressNoteItemID : number;
  progressNotesList: ProgressNotesList[];
  progressNotesdata:[];
  clientProgressNotes: ClientProgressNotes = {};
  EditList: ClientProgressNotes[] = [];
  progressNoteId: number = 0;
  responseModel: ResponseModel = {};
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  @ViewChild('btnAddfundtype') cancelAdd: ElementRef;
  @ViewChild('btnEditfundtype') cancelEdit: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('formDirective') private formDirective: NgForm;
  EditId: number;
  orderBy: number;
  orderColumn: number;
  ProgresstimeList = [];
  Appraisalcrieteriadata = [];
  timimgStandards: TimingStandards = {};
  appraisalStandardsArray: TimingStandards[] = [];
  displayedColumnsStandard: string[] = ['description', 'progressnotetext'];
  dataSourceStandard = new MatTableDataSource(this.ProgresstimeList);
  ShiftId: number;
  EmpId: number;
  constructor(private route: ActivatedRoute, private clientService: ClientService, private notificationService: NotificationService,
     private fb: FormBuilder,private commonService: CommonService) {
      this.paging.pageNo = 1;
      this.paging.pageSize = 10;
     }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params['Id']);
      this.EditId = parseInt(params['EId']);
      this.ShiftId = parseInt(params['ShiftId']); 
      this.EmpId = parseInt(params['EmpId']);
      if(this.EditId>0){
        this.getprogressnotesinfo();
       }
   
    });
     this.createForm();
     this.createFormProgressNotes();
     this.createFormEditProgressNotes();
     this.getProgressNote();
     this.createForm();
  }
  disableFields() {
    this.clientForm.get('patientName').disable();
    this.clientForm.get('dateOfBirth1').disable();
    this.clientForm.get('medicalNo').disable();
  }
 
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceProgressNotes !== undefined ? this.dataSourceProgressNotes.sort = this.sort : this.dataSourceProgressNotes;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getList())
        )
        .subscribe();
    }, 2000);
    this.getPrimaryInfo();
    this.getList();
   
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (this.progressNotesList.length > 0) {
      this.dataSourceProgressNotes = new MatTableDataSource(this.progressNotesList);
    }

  }
 
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'date':
        this.orderColumn = 0;
        break;
      case 'progressNote':
        this.orderColumn = 1;
        break;
      case 'createdDate':
        this.orderColumn = 2;
        break;
      default:
        break;
    }
  }
  cancelModal() {
    this.rFormProgressNotes.reset();
    this.formDirective.resetForm();
  }
  DeleteModal(fundingtypeId,_e)
  {
    this.deleteProgressNoteItemId = fundingtypeId.id;
  }
  getprogressnotesinfo(){
        const data = {
        Id: Number(this.EditId),
         };
         this.clientService.GetProgressNotesDetails(data).subscribe(res => {
         this.responseModel = res;
         this.EditList = this.responseModel.responseData;
         document.getElementById("openEditModalButton").click();
         this.EditProgressNoteItemID=this.EditList[0]['id'];
         this.rFormEditProgressNotes.controls['editProgressDate'].setValue(this.EditList[0]['date']);
         this.rFormEditProgressNotes.controls['editNote09AMto11AM'].setValue(this.EditList[0]['note9AMTo11AM']);
         this.rFormEditProgressNotes.controls['editNote11AMto13PM'].setValue(this.EditList[0]['note11AMTo1PM']);
         this.rFormEditProgressNotes.controls['editNote13PMto15PM'].setValue(this.EditList[0]['note1PMTo15PM']);
         this.rFormEditProgressNotes.controls['editNote15PMto17PM'].setValue(this.EditList[0]['note15PMTo17PM']);
         this.rFormEditProgressNotes.controls['editNote17PMto19PM'].setValue(this.EditList[0]['note17PMTo19PM']);
         this.rFormEditProgressNotes.controls['editNote19toPM21PM'].setValue(this.EditList[0]['note19PMTo21PM']);
         this.rFormEditProgressNotes.controls['editNote21PMto23PM'].setValue(this.EditList[0]['note21PMTo23PM']);
         this.rFormEditProgressNotes.controls['editNote23PMto01AM'].setValue(this.EditList[0]['note23PMTo1AM']);
         this.rFormEditProgressNotes.controls['editNote01AMto03AM'].setValue(this.EditList[0]['note1AMTo3AM']);
         this.rFormEditProgressNotes.controls['editNote03AMto05AM'].setValue(this.EditList[0]['note3AMTo5AM']);
         this.rFormEditProgressNotes.controls['editNote05AMto07AM'].setValue(this.EditList[0]['note5AMTo7AM']);
         this.rFormEditProgressNotes.controls['editNote07AMto09AM'].setValue(this.EditList[0]['note7AMTo9AM']);
         this.rFormEditProgressNotes.controls['editNDISGoals'].setValue(this.EditList[0]['summary']);
         this.rFormEditProgressNotes.controls['editOtherInfo'].setValue(this.EditList[0]['otherInfo']);
        })
  }
  openEditDetails(elem) {
    document.getElementById("openEditModalButton").click();
    this.EditProgressNoteItemID=elem.id;
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
    this.rFormEditProgressNotes.controls['editNDISGoals'].setValue(elem.summary);
    this.rFormEditProgressNotes.controls['editOtherInfo'].setValue(elem.otherInfo);
   
  }
 
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getList();
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
      NDISGoals: [null, Validators.nullValidator],
      OtherInfo: [null, Validators.nullValidator],
      
    });
  }
  get progressDate()
  {
    return this.rFormProgressNotes.get('progressDate');
  }
  get Note09AMto11AM()
  {
    return this.rFormProgressNotes.get('Note09AMto11AM');
  }
  get Note11AMto13PM()
  {
    return this.rFormProgressNotes.get('Note11AMto13PM');
  } 
  get Note13PMto15PM()
  {
    return this.rFormProgressNotes.get('Note13PMto15PM');
  }
  get Note15PMto17PM()
  {
    return this.rFormProgressNotes.get('Note15PMto17PM');
  }
  get Note17PMto19PM()
  {
    return this.rFormProgressNotes.get('Note17PMto19PM');
  }
  get Note19toPM21PM()
  {
    return this.rFormProgressNotes.get('Note19toPM21PM');
  }
  get Note21PMto23PM()
  {
    return this.rFormProgressNotes.get('Note21PMto23PM');
  }
  get Note23PMto01AM()
  {
    return this.rFormProgressNotes.get('Note23PMto01AM');
  }
  get Note01AMto03AM()
  {
    return this.rFormProgressNotes.get('Note01AMto03AM');
  }
  get Note03AMto05AM()
  {
    return this.rFormProgressNotes.get('Note23PMto01AM');
  }
  get Note05AMto07AM()
  {
    return this.rFormProgressNotes.get('Note05AMto07AM');
  }
  get NDISGoals()
  {
    return this.rFormProgressNotes.get('NDISGoals');
  }

  get OtherInfo()
  {
    return this.rFormProgressNotes.get('OtherInfo');
  }
  
  
  createFormEditProgressNotes() {
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
      editNDISGoals: [null, Validators.nullValidator],
      editOtherInfo: [null, Validators.nullValidator],
      
    });
  }
  get editProgressDate()
  {
    return this.rFormEditProgressNotes.get('editProgressDate');
  }
  get editNote09AMto11AM()
  {
    return this.rFormEditProgressNotes.get('editNote09AMto11AM');
  }
  get editNote11AMto13PM()
  {
    return this.rFormEditProgressNotes.get('editNote11AMto13PM');
  } 
  get editNote13PMto15PM()
  {
    return this.rFormEditProgressNotes.get('editNote13PMto15PM');
  }
  get editNote15PMto17PM()
  {
    return this.rFormEditProgressNotes.get('editNote15PMto17PM');
  }
  get editNote17PMto19PM()
  {
    return this.rFormEditProgressNotes.get('editNote17PMto19PM');
  }
  get editNote19toPM21PM()
  {
    return this.rFormEditProgressNotes.get('editNote19toPM21PM');
  }
  get editNote21PMto23PM()
  {
    return this.rFormEditProgressNotes.get('editNote21PMto23PM');
  }
  get editNote23PMto01AM()
  {
    return this.rFormEditProgressNotes.get('editNote23PMto01AM');
  }
  get editNote01AMto03AM()
  {
    return this.rFormEditProgressNotes.get('editNote01AMto03AM');
  }
  get editNote03AMto05AM()
  {
    return this.rFormEditProgressNotes.get('editNote23PMto01AM');
  }
  get editNote05AMto07AM()
  {
    return this.rFormEditProgressNotes.get('editNote05AMto07AM');
  }
  get editNote07AMto09AM()
  {
    return this.rFormEditProgressNotes.get('editNote07AMto09AM');
  }
  get editNDISGoals()
  {
    return this.rFormEditProgressNotes.get('editNDISGoals');
  }
 
  get editOtherInfo()
  {
    return this.rFormEditProgressNotes.get('editOtherInfo');
  }
 
  createForm() {
    this.clientForm = this.fb.group({
      patientName: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
      dateOfBirth1: [null, Validators.required],
      medicalNo: [this.clientProgressNotes.medicalRecordNo, Validators.nullValidator],
      
    });
  }
  get patientName() {
    return this.clientForm.get('patientName');
  }
  get dateOfBirth1() {
    return this.clientForm.get('dateOfBirth1');
  }
  get medicalNo() {
  return this.clientForm.get('medicalNo');
  }
 
  getPrimaryInfo() {
    const data = {
      id: this.ClientId,
    }
    this.clientService.getClientPrimaryInfo(data).subscribe((res: any) => {
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
  AddProgressNotes(){
    if (this.clientForm.valid) {
      const data = {
        ClientId: this.ClientId,
        patientName: this.clientForm.get('patientName').value,
        dateOfBirth : moment(this.clientForm.get('dateOfBirth1').value).format('YYYY-MM-DD'),
        medicalRecordNo : String(this.clientForm.get('medicalNo').value),
        
      }
      this.clientService.addClientProgressNotes(data).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
           this.clientProgressNotes = this.responseModel.responseData;
           this.progressNoteId = this.responseModel.responseData.id;
            this.notificationService.Success({ message: this.responseModel.message, title: null });
            break;
           default:
            this.notificationService.Error({ message: this.responseModel.message, title: null });
            break;
        }
      });
    }

  }
  AddProgressNotesItem(){
    if(this.rFormProgressNotes.get('progressDate').value==null||this.rFormProgressNotes.get('progressDate').value==""){
        this.notificationService.Warning({ message: 'Please fill Date', title: null });
        return;
      }
   else if (this.rFormProgressNotes.valid) {
       const data = {
        ClientId: this.ClientId,
        ShiftId: this.ShiftId,
        EmployeeId: this.EmpId,
       clientProgressNoteId : this.progressNoteId,
       date : moment(this.rFormProgressNotes.get('progressDate').value).format('YYYY-MM-DD'),
       Note9AMTo11AM :this.rFormProgressNotes.get('Note09AMto11AM').value,
       Note11AMTo1PM :this.rFormProgressNotes.get('Note11AMto13PM').value,
       Note1PMTo15PM :this.rFormProgressNotes.get('Note13PMto15PM').value,
       Note15PMTo17PM:this.rFormProgressNotes.get('Note15PMto17PM').value,
       Note17PMTo19PM:this.rFormProgressNotes.get('Note17PMto19PM').value,
       Note19PMTo21PM:this.rFormProgressNotes.get('Note19toPM21PM').value,
       Note21PMTo23PM:this.rFormProgressNotes.get('Note21PMto23PM').value,
       Note23PMTo1AM  :this.rFormProgressNotes.get('Note23PMto01AM').value,
       Note1AMTo3AM :this.rFormProgressNotes.get('Note01AMto03AM').value,
       Note3AMTo5AM :this.rFormProgressNotes.get('Note03AMto05AM').value,
       Note5AMTo7AM :this.rFormProgressNotes.get('Note05AMto07AM').value,
       Note7AMTo9AM :this.rFormProgressNotes.get('Note07AMto09AM').value,
       Summary :this.rFormProgressNotes.get('NDISGoals').value,
       OtherInfo :this.rFormProgressNotes.get('OtherInfo').value,
       
      }
       
      this.clientService.addClientrogressNotesItem(data).subscribe(res => {
        this.responseModel = res;
         switch (this.responseModel.status) {
          case 1:
           this.rFormProgressNotes.reset();
           this.cancelAdd.nativeElement.click();
           //this.progressNoteId = this.responseModel.responseData.id;
           this.getList();
            this.notificationService.Success({ message: this.responseModel.message, title: null });
            break;

          default:
            this.notificationService.Error({ message: this.responseModel.message, title: null });
            break;
        }
      });
    }
 
  }
  UpdateProgressNotesItem()
  {
    if (this.rFormEditProgressNotes.valid) {
      const data = {
       Id : this.EditProgressNoteItemID,
       ClientId: this.ClientId,
       ClientProgressNoteId : this.progressNoteId,
       Date : moment(this.rFormEditProgressNotes.get('editProgressDate').value).format('YYYY-MM-DD'),
       Note9AMTo11AM :this.rFormEditProgressNotes.get('editNote09AMto11AM').value,
       Note11AMTo1PM :this.rFormEditProgressNotes.get('editNote11AMto13PM').value,
       Note1PMTo15PM :this.rFormEditProgressNotes.get('editNote13PMto15PM').value,
       Note15PMTo17PM:this.rFormEditProgressNotes.get('editNote15PMto17PM').value,
       Note17PMTo19PM:this.rFormEditProgressNotes.get('editNote17PMto19PM').value,
       Note19PMTo21PM:this.rFormEditProgressNotes.get('editNote19toPM21PM').value,
       Note21PMTo23PM:this.rFormEditProgressNotes.get('editNote21PMto23PM').value,
       Note23PMTo1AM  :this.rFormEditProgressNotes.get('editNote23PMto01AM').value,
       Note1AMTo3AM :this.rFormEditProgressNotes.get('editNote01AMto03AM').value,
       Note3AMTo5AM :this.rFormEditProgressNotes.get('editNote03AMto05AM').value,
       Note5AMTo7AM :this.rFormEditProgressNotes.get('editNote05AMto07AM').value,
       Note7AMTo9AM :this.rFormEditProgressNotes.get('editNote07AMto09AM').value,
       Summary :this.rFormEditProgressNotes.get('editNDISGoals').value,
       OtherInfo :this.rFormEditProgressNotes.get('editOtherInfo').value,
       
      };
      this.clientService.updateClientrogressNotesItem(data).subscribe(res => {
       
        this.responseModel = res;
        switch (this.responseModel.status) {
        case 1:
          this.rFormEditProgressNotes.reset();
          this.cancelEdit.nativeElement.click();
          this.getList();
          this.notificationService.Success({ message: this.responseModel.message, title: null });
          break;

        default:
          this.notificationService.Error({ message: this.responseModel.message, title: null });
          break;
      }
    });
  }
}
openEditviewDetails(data) {
  document.getElementById("openViewModalButton").click();
  this.progressNotesFields = data;
}
getList(){
  this.getSortingOrder();
  const data = {
    ClientId: this.ClientId,
    ShiftId: this.ShiftId,
    pageNo: this.paging.pageNo,
    pageSize: this.paging.pageSize,
    OrderBy: this.orderColumn,
    SortOrder: this.orderBy
  };
  this.clientService.getClientrogressNotesItemList(data).subscribe(res => {
    this.responseModel = res;
    this.totalCount = this.responseModel.total;
    if (this.responseModel.responseData!=null) {
      this.progressNotesList=this.responseModel.responseData;
      this.dataSourceProgressNotes = new MatTableDataSource(this.progressNotesList);
      }
      else{
        this.dataSourceProgressNotes = new MatTableDataSource(this.progressNotesdata);
       
      }
      
  });
}
getProgressNote(){
  const data = {
    ClientId: this.ClientId,
  };
  this.clientService.GetAllClientProgressSingleNote(data).subscribe(res => {
    this.responseModel = res;
      if (this.responseModel.responseData!=null) {
      this.clientProgressNotes = this.responseModel.responseData;
      this.progressNoteId = this.clientProgressNotes.id;
      this.clientForm.controls['medicalNo'].patchValue(this.clientProgressNotes.medicalRecordNo);
      // this.clientForm.controls['nextAppointment'].patchValue(this.clientProgressNotes.scheduleFor);
      // this.clientForm.controls['appointmentTo'].patchValue(this.clientProgressNotes.appointmentTo);
      // this.clientForm.controls['discharge'].patchValue(this.clientProgressNotes.forDescharge);
      // this.clientForm.controls['other'].patchValue(this.clientProgressNotes.other);
      // this.clientForm.controls['reviewDate'].patchValue(this.clientProgressNotes.reviewDate);
      // this.clientForm.controls['signedDate'].patchValue(this.clientProgressNotes.signedDate);
      }
      else{
       
      }
      
  });
}


DeleteProgressNotes(event){
 
  this.clientService.DeleteClientProgressNotesItem({Id : this.deleteProgressNoteItemId}).subscribe((data: any) => {
    if (data.status == 1) {
      this.notificationService.Success({ message: data.message, title: null });
      this.getList();
    }
    else {
      this.notificationService.Error({ message: data.message, title: null });
    }

  })
}

}
export interface TimingStandards {
  Id?: number;
  Timing?: string;
  ProgressNote?:string;
}