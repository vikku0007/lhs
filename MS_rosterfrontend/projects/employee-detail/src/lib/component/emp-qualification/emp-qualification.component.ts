import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { MatTableDataSource } from '@angular/material/table';
import { NotificationService } from 'projects/core/src/projects';
import { ActivatedRoute } from '@angular/router';
import { Paging } from 'projects/viewmodels/paging';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { environment } from 'src/environments/environment';
import * as moment from 'moment';
import { CommonService } from 'projects/lhs-service/src/projects';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { EmployeeEducationModel } from '../../view-models/employee-education-model';
import { EmpDetailService } from '../../services/emp-detail.service';
@Component({
  selector: 'lib-emp-qualification',
  templateUrl: './emp-qualification.component.html',
  styleUrls: ['./emp-qualification.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})
export class EmpQualificationComponent implements OnInit, OnChanges {
  @Input() employeeEducationInfo: EmployeeEducationModel[];
  empEduModel: EmployeeEducationModel = {};
  rForm: FormGroup;
  rForm1: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnAddEducationCancel') cancel: ElementRef;
  @ViewChild('btnEditEducationCancel') editCancel: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  @ViewChild('fileInputEdit', {static: false}) fileInputEdit: ElementRef;
  ReqImageName: any;
  ReqImageSize: any;
  EditReqImageName: any;
  EditReqImageSize: any;
  isShownReqBrowse: boolean = false ; 
  isShownReqUrl: boolean = false ;
  baseUrl : string = environment.baseUrl;
  orderBy: number;
  orderColumn: number;
  dataSourceEducation: any;
  displayedColumnsEducation: string[] = ['qualificationtype','institute', 'degree', 'fieldStudy', 'completionDate', 'additionalNotes', 'action'];
  educationId = 0;
  getErrorMessage: 'Please Enter Value';
  employeeId: number;
  @ViewChild('formDirective1') private formDirective1: NgForm;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  deleteEducationId: any;
  totalCount: number;
  paging: Paging = {};
  trainingList = [];
  Imageurl: any;
  QualificationTypeList: [];
  constructor(private fb: FormBuilder,private commonservice:CommonService, private empService: EmpDetailService, private notificationService: NotificationService,
    private route: ActivatedRoute) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.employeeEducationInfo.length > 0) {
      this.dataSourceEducation = new MatTableDataSource(this.employeeEducationInfo);
      this.createEditForm(0);
    }

  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.employeeId = Number(params.params.id);
    });
    this.createForm();
    this.createEditForm(null);
    this.GetEmployeeEducationList();
    this.getqualificationtype();
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceEducation !== undefined ? this.dataSourceEducation.sort = this.sort : this.dataSourceEducation;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.GetEmployeeEducationList())
        )
        .subscribe();
    }, 2000);

  }
  createForm() {
    this.rForm1 = this.fb.group({
      addInstitute: ['', Validators.required],
      addDegree: ['', Validators.required],
      addField: ['', Validators.required],
      addDate: ['', Validators.required],
      addNotes: ['', Validators.required],
      qualificationType: ['', Validators.required],
    });
  }

  get addInstitute() {
    return this.rForm1.get('addInstitute');
  }

  createEditForm(index) {
    if (index != null) {
      this.rForm = this.fb.group({
        institute: [this.employeeEducationInfo[index].institute, Validators.required],
        degree: [this.employeeEducationInfo[index].degree, Validators.required],
        field: [this.employeeEducationInfo[index].fieldStudy, Validators.required],
        completionDate: [this.employeeEducationInfo[index].completionDate, Validators.required],
        notes: [this.employeeEducationInfo[index].additionalNotes, Validators.nullValidator],
        qualificationTypeEdit: [this.employeeEducationInfo[index].qualificationType, Validators.required],
     });
    }
    else {
      this.rForm = this.fb.group({
        institute: ['', Validators.required],
        degree: ['', Validators.required],
        field: ['', Validators.required],
        completionDate: ['', Validators.required],
        notes: ['', Validators.nullValidator],
        qualificationTypeEdit: ['', Validators.required],
      });
    }
  }

  get institute() {
    return this.rForm.get('institute');
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetEmployeeEducationList();
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
  AddEducationDetails() {
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
      formData.append('Institute', this.rForm1.get('addInstitute').value);
      formData.append('FieldStudy', this.rForm1.get('addField').value);
      formData.append('Degree', this.rForm1.get('addDegree').value);
      formData.append('CompletionDate', moment(this.rForm1.get('addDate').value).format("YYYY-MM-DD"));
      formData.append('AdditionalNotes', this.rForm1.get('addNotes').value);
      formData.append('QualificationType', this.rForm1.get('qualificationType').value);
      this.empService.AddEmpEducation(formData).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
           this.rForm1.reset();
            this.formDirective1.resetForm();
            this.cancel.nativeElement.click();
            this.GetEmployeeEducationList();
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

  openModal(open: boolean) {
    document.getElementById("openModalButton").click();
  }

  openEditModal(elem) {
    // console.log(elem);
    document.getElementById("openEditModalButton").click();
    const index = this.employeeEducationInfo.findIndex(x => x.id == elem.id);
    this.createEditForm(index);
    this.educationId = this.employeeEducationInfo[index].id;
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

  UpdateEducationDetails() {
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
    formData.set('Id', this.educationId.toString());
    formData.append('EmployeeId', this.employeeId.toString());
    formData.append('Institute', this.rForm.get('institute').value);
    formData.append('Degree', this.rForm.get('degree').value);
    formData.append('FieldStudy', this.rForm.get('field').value);
    formData.append('CompletionDate', moment(this.rForm.get('completionDate').value).format("YYYY-MM-DD"));
    formData.append('AdditionalNotes', this.rForm.get('notes').value);
    formData.append('QualificationType', this.rForm.get('qualificationTypeEdit').value);
      this.empService.AddEmpEducation(formData).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.GetEmployeeEducationList();
            this.editCancel.nativeElement.click();
            this.notificationService.Success({ message: 'Details updated successfully', title: '' });
            break;
            default:
            break;
        }
      });
    }
  }
  DeleteModal(educationID,_e)
  {

    this.deleteEducationId = educationID;
  }

  deleteEducation(event) {
    const data = {
      id: this.deleteEducationId
    }
    this.empService.DeleteEmployeeEducationdetails(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
         // this.dataSourceEducation.data = this.dataSourceEducation.data.filter(i => i !== elm)
          this.notificationService.Success({ message: 'Education details deleted successfully', title: '' })
         this.GetEmployeeEducationList();
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
  getSortingOrder() {
    debugger;
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
     switch (sortColumn) {
      case 'qualificationtype':
        this.orderColumn = 0;
        break;
      case 'institute':
        this.orderColumn = 1;
        break;
      case 'degree':
        this.orderColumn = 2;
        break;
      case 'fieldStudy':
        this.orderColumn = 3;
        break;
        case 'completionDate':
          this.orderColumn = 4;
          break;
          case 'additionalNotes':
          this.orderColumn = 5;
          break;
      case 'createdDate':
        this.orderColumn = 6;
        break;

      default:
        break;
    }
  }
  GetEmployeeEducationList() {
    this.getSortingOrder();
    const data = {
      employeeId:this.employeeId,
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy

    };
   
    this.empService.GetEmployeeEducationList(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      let fundtypearray = [];
      if (this.response.responseData) {
        this.employeeEducationInfo = this.response.responseData;
        this.dataSourceEducation = new MatTableDataSource(this.employeeEducationInfo);
         }
      else {
        this.dataSourceEducation = new MatTableDataSource(this.trainingList);
      }

    });
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
  FileSaver = require('file-saver');
  downloadPdf(docUrl) {
   
   this.FileSaver.saveAs(docUrl);
 }
 gridload(){
  this.GetEmployeeEducationList()
}
 ImageDelete(){
    
  this.empEduModel.id = this.educationId;
    this.empService.DeleteEmployeeEducationDocument(this.empEduModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.isShownReqUrl=false;
        this.isShownReqBrowse=true;
        this.ReqImageName="";
        this.ReqImageSize="";
        this.GetEmployeeEducationList();
        
      }
      else {
        this.notificationService.Error({ message: "Some error occured", title: null });
      }

    })
 
}
getqualificationtype(){
  this.commonservice.getqualificationType().subscribe((res=>{
    if(res){
      this.response = res;
      this.QualificationTypeList=this.response.responseData||[];
     
    }else{

    }
  }));
}
}
