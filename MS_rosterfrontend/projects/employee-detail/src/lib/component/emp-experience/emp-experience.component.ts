import { Component, OnInit, Input, OnChanges, SimpleChanges, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { MatTableDataSource } from '@angular/material/table';
import { Constants } from '../../config/constants';
import { NotificationService } from 'projects/core/src/projects';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { merge, from } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { EmployeeWorkModel } from '../../view-models/employee-work-model';
import { EmpDetailService } from '../../services/emp-detail.service';

@Component({
  selector: 'lib-emp-experience',
  templateUrl: './emp-experience.component.html',
  styleUrls: ['./emp-experience.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class EmpExperienceComponent implements OnInit, OnChanges {
  @Input() employeeWorkInfo: EmployeeWorkModel[];
  rForm: FormGroup;
  rForm1: FormGroup;
  response: ResponseModel = {};
  dataSourceExperience: any;
  displayedColumnsExperience: string[] = ['company', 'jobTitle', 'startDate', 'endDate', 'jobDesc','duration' ,'action'];
  workId = 0;
  employeeWorkModel: EmployeeWorkModel = {};
  @ViewChild('btnAddWorkCancel') cancel: ElementRef;
  @ViewChild('btnEditWorkCancel') editCancel: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  orderBy: number;
  orderColumn: number;
  getErrorMessage: 'Please Enter Value';
  employeeId: number;
  @ViewChild('formDirective1') private formDirective1: NgForm;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  deleteexpId: any;
  totalCount: number;
  paging: Paging = {};
  trainingList = [];
  todayDate: Date = new Date();
  todate: any;
  diffInDays: number;
  fromDate: moment.Moment;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  @ViewChild('fileInputEdit', {static: false}) fileInputEdit: ElementRef;
  ReqImageName: any;
  ReqImageSize: any;
  EditReqImageName: any;
  EditReqImageSize: any;
  isShownReqBrowse: boolean = false ; 
  isShownReqUrl: boolean = false ;
  baseUrl : string = environment.baseUrl;
  Imageurl: string;
  constructor(private fb: FormBuilder, private empService: EmpDetailService, private notificationService: NotificationService,
    private route: ActivatedRoute) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.employeeWorkInfo.length > 0) {
      this.dataSourceExperience = new MatTableDataSource(this.employeeWorkInfo);
      this.createEditForm(0);
    }
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.employeeId = Number(params.params.id);
    });
    this.createForm();
    this.createEditForm(null);
    this.GetExperienceList();
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceExperience !== undefined ? this.dataSourceExperience.sort = this.sort : this.dataSourceExperience;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.GetExperienceList())
        )
        .subscribe();
    }, 2000);

  }
  createForm() {
    this.rForm1 = this.fb.group({
      addCompany: ['', Validators.required],
      addJobTitle: ['', Validators.required],
      addFromDate: ['', Validators.required],
      addToDate: ['', Validators.required],
      addDescription: ['', Validators.required],
      addDuration: ['', Validators.required],
    });
  }

  createEditForm(index) {
    if (index != null) {
     
      this.rForm = this.fb.group({
        company: [this.employeeWorkInfo[index].company, Validators.required],
        jobTitle: [this.employeeWorkInfo[index].jobTitle, Validators.required],
        fromDate: [this.employeeWorkInfo[index].startDate, Validators.required],
        toDate: [this.employeeWorkInfo[index].endDate, Validators.required],
        description: [this.employeeWorkInfo[index].jobDesc, Validators.required],
        Duration:[this.employeeWorkInfo[index].duration, Validators.required],
      });
    }
    else {
      this.rForm = this.fb.group({
        company: ['', Validators.required],
        jobTitle: ['', Validators.required],
        fromDate: ['', Validators.required],
        toDate: ['', Validators.required],
        description: ['', Validators.required],
        Duration: ['', Validators.required],
      });
    }
  }

  openModal(open: boolean) {
    document.getElementById("openAddWorkButton").click();
  }

  openEditModal(elem) {
   document.getElementById("openEditWorkButton").click();
    const index = this.employeeWorkInfo.findIndex(x => x.id == elem.id);
    this.createEditForm(index);
    this.workId = this.employeeWorkInfo[index].id;
    if(elem.fileName!="" && elem.documentPath!=null){
      // (this.Imageurl)=data.fileName;
       this.Imageurl = (this.baseUrl + elem.documentPath);
       this.isShownReqUrl=true;
       this.isShownReqBrowse=false;
     
     }
     else{
       this.isShownReqUrl=false;
       this.isShownReqBrowse=true;
     
     }
  }
  uploadRequireDocument(event: any) {
    let fileExtension = null;
    let extension = null;
    if (event.target.files !== undefined) {

      fileExtension = event.target.files[0].name.split('.');
      extension = fileExtension[fileExtension.length - 1].toLowerCase();
      this.ReqImageName =  event.target.files[0].name;
      this.ReqImageSize =  event.target.files[0].length;
      
    }
  }
  ReqBrowseImageDelete(){
    this.ReqImageName =  '';
    this.ReqImageSize =  '';
    this.fileInput.nativeElement.value = '';
  }
  uploadEditDocument(event: any) {
    let fileExtension = null;
    let extension = null;
    if (event.target.files !== undefined) {
      fileExtension = event.target.files[0].name.split('.');
      extension = fileExtension[fileExtension.length - 1].toLowerCase();
      this.EditReqImageName =  event.target.files[0].name;
      this.EditReqImageSize =  event.target.files[0].length;
       
    }
    
  }
  EditReqImageDelete(){
    this.EditReqImageName =  '';
    this.EditReqImageSize =  '';
    this.fileInputEdit.nativeElement.value = '';
  }
  UpdateWorkDetails() {
   if (this.rForm.valid) {
      const formData = new FormData(); 
      if(this.EditReqImageName==undefined){
      }
      else{
      const fileInput = this.fileInputEdit.nativeElement;
      let fileLength = fileInput.files.length;
      let file = fileInput.files[0];
      if(fileLength > 0)
      {
        var type =  file.type;
        var name =  file.name;
        formData.append('Files', fileInput.files[0]);
      }
    }
    formData.set('Id', this.workId.toString());
    formData.append('EmployeeId', this.employeeId.toString());
    formData.append('Company', this.rForm.get('company').value);
    formData.append('JobTitle', this.rForm.get('jobTitle').value);
    formData.append('StartDate', moment(this.rForm.get('fromDate').value).format("YYYY-MM-DD"));;
    formData.append('EndDate', moment(this.rForm.get('toDate').value).format("YYYY-MM-DD"));
    formData.append('JobDesc', this.rForm.get('description').value);
    formData.append('Duration', this.rForm.get('Duration').value);
      this.empService.updateWorkExperience(formData).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.GetExperienceList();
            this.editCancel.nativeElement.click();
            this.notificationService.Success({ message: 'Details updated successfully', title: '' })
            break;

          default:
            break;
        }
      });
    }
  }
  
  AddWorkDetails() {
    if (this.rForm1.valid) {
      const fileInput = this.fileInput.nativeElement;
      let fileLength = fileInput.files.length;
      let file = fileInput.files[0];
      const formData = new FormData(); 
      if(fileLength > 0)
      {
        var type =  file.type;
        var name =  file.name;
        formData.append('Files', fileInput.files[0]);
      }
     formData.append('EmployeeId', this.employeeId.toString());
      formData.append('Company', this.rForm1.get('addCompany').value);
      formData.append('JobTitle', this.rForm1.get('addJobTitle').value);
      formData.append('StartDate', moment(this.rForm1.get('addFromDate').value).format("YYYY-MM-DD"));
      formData.append('EndDate', moment(this.rForm1.get('addToDate').value).format("YYYY-MM-DD"));
      formData.append('JobDesc', this.rForm1.get('addDescription').value);
      formData.append('Duration', this.rForm1.get('addDuration').value);
      this.empService.updateWorkExperience(formData).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.employeeWorkModel = {};
            this.employeeWorkModel.id = this.response.responseData.id;
            this.employeeWorkModel.employeeId = this.employeeId;
            this.GetExperienceList();
            this.rForm1.reset();
            this.formDirective1.resetForm();
            this.cancel.nativeElement.click();
            this.ReqImageName="";
            this.ReqImageSize="";
            this.fileInput.nativeElement.value = ''
            this.notificationService.Success({ message: 'Details added successfully', title: '' });
            break;

          default:
            break;
        }
      });
    }
  }
  DeleteModal(educationID,_e)
  {

    this.deleteexpId = educationID;
  }

  deleteExperiencedetails(event) {
    const data = {
      id: this.deleteexpId
    }
    this.empService.DeleteEmployeeExperiencedetails(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
         // this.dataSourceEducation.data = this.dataSourceEducation.data.filter(i => i !== elm)
          this.notificationService.Success({ message: 'Experience details deleted successfully', title: '' })
         this.GetExperienceList();
          break;
        case 0:
          this.notificationService.Error({ message: this.response.message, title: '' });
        default:
          break;
      }
    });
  }
 
  
  
  cancelModal() {
    this.rForm1.reset();
    this.formDirective1.resetForm();
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetExperienceList();
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
   
    switch (sortColumn) {
      case 'company':
        this.orderColumn = 0;
        break;
      case 'jobTitle':
        this.orderColumn = 1;
        break;
      case 'startDate':
        this.orderColumn = 2;
        break;
        case 'endDate':
          this.orderColumn = 3;
          break;
          case 'jobDesc':
          this.orderColumn = 4;
          break;
      case 'createdDate':
        this.orderColumn = 5;
        break;

      default:
        break;
    }
  }
  GetExperienceList() {
    this.getSortingOrder();
    const data = {
      employeeId:this.employeeId,
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy

    };
    this.empService.GetEmployeeExperienceList(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      
      let fundtypearray = [];
      if (this.response.responseData) {
        this.employeeWorkInfo = this.response.responseData;
        this.dataSourceExperience = new MatTableDataSource(this.employeeWorkInfo);
         }
      else {
        this.dataSourceExperience = new MatTableDataSource(this.trainingList);
      }

    });
  }
 
  calculateDate() { 
  
    var fromDate = (this.rForm1.controls.addFromDate.value);
    var todate=(this.rForm1.controls.addToDate.value);
    //var diffDays:any = Math.floor((fromDate-todate));
    var date1:any = new Date(fromDate);
     var date2:any = new Date(todate);
     var message:any;
     var month=31;
     var days=1000*60*60*24;
     var diffDays:any=(date2-date1); 
     var day=(Math.floor(diffDays/days));   
     var years = (Math.floor(day/365));
     var months = (Math.round(day % 365)/month);
   // document.write(years+"year-"+months);
 
   if(years>0){
    message=years+" "+"year-"+(months).toFixed(0)+" "+"month";
   }
   else if((months>1)){
     message=(months).toFixed(0)+" "+"month";
   }
   else{
    message=day+" "+"day";
   }
    this.rForm1.controls.addDuration.setValue(message);
    this.rForm1.get('addDuration').disable();
  }

  calculateDateEdit() { 
   
    var fromDate = (this.rForm.controls.fromDate.value);
    var todate=(this.rForm.controls.toDate.value);
    //var diffDays:any = Math.floor((fromDate-todate));
    var date1:any = new Date(fromDate);
     var date2:any = new Date(todate);
     var message:any;
     var month=31;
     var days=1000*60*60*24;
     var diffDays:any=(date2-date1); 
     var day=(Math.floor(diffDays/days));   
     var years = (Math.floor(day/365));
     var months = (Math.round(day % 365)/month).toFixed(0);
   // document.write(years+"year-"+months);
   if(years>0){
    message=years+" "+"year-"+months+" "+"month";
   }
   else{
     message=months+" "+"month";
   }
    this.rForm.controls.Duration.setValue(message);
    this.rForm.get('Duration').disable();
  }
  FileSaver = require('file-saver');
  downloadPdf(docUrl) {
   
   this.FileSaver.saveAs(docUrl);
 }
 gridload(){
  this.GetExperienceList()
}
 ImageDelete(){
  this.employeeWorkModel.id = this.workId;
    this.empService.DeleteEmployeeExperienceDocument(this.employeeWorkModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.isShownReqUrl=false;
        this.isShownReqBrowse=true;
        this.ReqImageName =  "";
      this.ReqImageSize = "";
        
      }
      else {
        this.notificationService.Error({ message: "Some error occured", title: null });
      }

    })
 
}
}
