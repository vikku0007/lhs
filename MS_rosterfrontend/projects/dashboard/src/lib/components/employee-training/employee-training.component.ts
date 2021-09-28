import { Component, OnInit, Input, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm, FormControl } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { EmpServiceService } from '../../services/emp-service.service';
import { MatTableDataSource } from '@angular/material/table';
import { NotificationService } from 'projects/core/src/projects';
import { ActivatedRoute } from '@angular/router';
import { EmployeeTrainingModel } from '../../viewmodel/employee-training-model';
import { CommonService } from 'projects/lhs-service/src/projects';
import { Paging } from 'projects/viewmodels/paging';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import * as moment from 'moment';
import { merge, Subject, ReplaySubject } from 'rxjs';
import { tap, takeUntil } from 'rxjs/operators';
import { MatSort } from '@angular/material/sort';
import { environment } from 'src/environments/environment';
import { validateHorizontalPosition } from '@angular/cdk/overlay';
import { EmployeeComplianceDetails } from '../../viewmodel/employee-compliance-details';
import { AllEmployeeRequireInfo } from '../compliance-list/compliance-list.component';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
@Component({
  selector: 'lib-employee-training',
  templateUrl: './employee-training.component.html',
  styleUrls: ['./employee-training.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class EmployeeTrainingComponent implements OnInit, OnChanges {
  @Input() employeeTrainingInfo: EmployeeTrainingModel[];
  emptrainingModel: EmployeeTrainingModel = {};
  rForm: FormGroup;
  rForm1: FormGroup;
  response: ResponseModel = {};
  @ViewChild('btnAddEducationCancel') cancel: ElementRef;
  @ViewChild('btnEdittrainingCancel') editCancel: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  orderBy: number;
  orderColumn: number;
  dataSourceEducation: any;
  displayedColumnsEducation: string[] = ['trainingtype','training', 'coursetype', 'startdate', 'enddate', 'remarks', 'action'];
  educationId = 0;
  getErrorMessage: 'Please Enter Value';
  employeeId: number;
  @ViewChild('formDirective1') private formDirective1: NgForm;
  trainingId: any;
  OptionalList: any;
  MandatoryList: any;
  CoursetypeList: any;
  deletetrainingId: any;
  totalCount: number;
  paging: Paging = {};
  trainingList = [];
  trainingtypeList: any;
  isShownExpiry: boolean = false ;
  isShownotherIssue: boolean = false ;
  isShownExpiryEdit: boolean = false ;
  isShownotherIssueEdit: boolean = false ;
  isShownCourse: boolean = false ;
  isShownStardate: boolean = false ;
  isShownEnddate: boolean = false ;
  isShownCourseedit: boolean = false ;
  isShownStardateedit: boolean = false ;
  isShownEnddateedit: boolean = false ;
  ReqImageName: any;
  ReqImageSize: any;
  isShownReqUrl: boolean;
  isShownReqBrowse: boolean;
  EditReqImageName: any;
  @ViewChild('fileInputEdit', {static: false}) fileInputEdit: ElementRef;
  @ViewChild('fileInputEditcompliance', {static: false}) fileInputEditcompliance: ElementRef;
  EditReqImageSize: any;
  Imageurl: any;
  RequireInfoModel: AllEmployeeRequireInfo = {};
  baseUrl : string = environment.baseUrl;
  requiredComplianceModel: EmployeeComplianceDetails[] = [];
  requiredCompliancedata: EmployeeComplianceDetails[];
  rFormEditCompliance: FormGroup;
  displayedColumnsRequired: string[] = ['trainingType','documentName',  'dateOfIssue',  'dateOfExpiry', 'alert','description', 'action'];
  dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
  deleteRequireId: any;
  @ViewChild('btnCancelEditOtherPopUp') btnEditOtherCancel: ElementRef;
  empComplianceInfo: EmployeeComplianceDetails = {};
  deletedocId: any;
  EditReqImageName1: any;
  EditReqImageSize1: any;
  list: any;
  selectedType: Event;
  selectedName: any;
  selectedNameEdit: any;
  docList: DocumentList[];
  isShownOtherTraining: boolean = false ;
  selectedNameother: any;
  selectedother: string;
  constructor(private fb: FormBuilder, private empService: EmpServiceService, private notificationService: NotificationService,
    private route: ActivatedRoute,private commonservice:CommonService) {
      this.paging.pageNo = 1;
      this.paging.pageSize = 10;
     }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.employeeTrainingInfo.length > 0) {
      this.dataSourceEducation = new MatTableDataSource(this.employeeTrainingInfo);
      this.createEditForm(0);
    }

  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.employeeId = Number(params['Id']);
    });
    this.createForm();
    this.createEditForm(null);
    this.getCoursetype();
    this.getTrainingtype();
    this.getEditTrainingtype();
    this.GetEmployeeTrainingList();
    this.rForm1.get('addAlert').setValue("2");
    this.searchcompliances();
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceEducation !== undefined ? this.dataSourceEducation.sort = this.sort : this.dataSourceEducation;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.GetEmployeeTrainingList())
        )
        .subscribe();
    }, 2000);

  }
  searchcompliances(){
    this.control.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filtercompliances();
    });
  }


 private filtercompliances() {
    if (!this.MandatoryList) {
      return;
    }
    let search = this.control.value;
    if (!search) {
      this.filteredRecords.next(this.MandatoryList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecords.next(
      this.MandatoryList.filter(department => department.codeDescription.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  createForm() {
    this.rForm1 = this.fb.group({
      addMandatory: ['', Validators.required],
      addOptional: ['', Validators.required],
      addStartDate: ['', Validators.required],
      addEndDate: ['', Validators.nullValidator],
      addCoursType: ['', Validators.nullValidator],
      addRemarks: ['', Validators.nullValidator],
      addAlert: ['', null],
      othertraining: ['', Validators.nullValidator],

    });
  }
 

  createEditForm(index) {
    if (index != null) {
      this.rForm = this.fb.group({
        editMandatory: [this.employeeTrainingInfo[index].mandatoryTraining, Validators.required],
        editOptional: [this.employeeTrainingInfo[index].trainingType, Validators.required],
        editStartDate: [this.employeeTrainingInfo[index].startDate, Validators.required],
        editEndDate: [this.employeeTrainingInfo[index].endDate, Validators.nullValidator],
        editCoursType: [this.employeeTrainingInfo[index].courseType, Validators.required],
        editRemarks: [this.employeeTrainingInfo[index].remarks, Validators.required],
        editAlert: [this.employeeTrainingInfo[index].isAlert == true ? '1' : '2', Validators.required],
        othertrainingedit:[null, Validators.nullValidator],
      });
    }
    else {
      this.rForm = this.fb.group({
        editMandatory: ['', Validators.required],
        editOptional: ['', Validators.required],
        editStartDate: ['', Validators.nullValidator],
        editEndDate: ['', Validators.required],
        editCoursType: ['', Validators.required],
        editRemarks: ['', Validators.required],
        editAlert: ['', Validators.required],
        othertrainingedit: ['', Validators.nullValidator],
      });
    }
  }
 
  get institute() {
    return this.rForm.get('institute');
  }
 
  EditReqImageDelete(){
    this.EditReqImageName1 =  '';
    this.EditReqImageName1 =  '';
    this.fileInputEdit.nativeElement.value = '';
  }
  FileSaver = require('file-saver');
  downloadPdf(docUrl) {
   this.FileSaver.saveAs(docUrl);
 }
 ImageDelete(){
  this.emptrainingModel.id = this.trainingId;
    this.empService.DeleteEmployeeTrainingDocument(this.emptrainingModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.isShownReqUrl=false;
        this.isShownReqBrowse=true;
       }
      else {
        this.notificationService.Error({ message: "Some error occured", title: null });
      }
    })
}
  AddTrainingDetails() {
    var EndDatecheck=moment(this.rForm1.get('addEndDate').value).format("YYYY-MM-DD");
    if (EndDatecheck =="Invalid date") {
      EndDatecheck="";
     
    }
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
      formData.append('MandatoryTraining', this.rForm1.get('addMandatory').value);
      formData.append('TrainingType', this.rForm1.get('addOptional').value);
      formData.append('StartDate', moment(this.rForm1.get('addStartDate').value).format('YYYY-MM-DD').toString());
      formData.append('EndDate', EndDatecheck);
      formData.append('CourseType', this.rForm1.get('addCoursType').value);
      formData.append('Remarks', this.rForm1.get('addRemarks').value);
      formData.append('IsAlert', this.rForm1.get('addAlert').value);
      formData.append('OtherTraining', this.rForm1.get('othertraining').value);
      this.empService.AddEmployeeTraining(formData).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
           this.formDirective1.resetForm();
            this.cancel.nativeElement.click();
            this.GetEmployeeTrainingList();
            this.rForm1.get('addAlert').setValue("2");
            this.ReqImageName="";
            this.ReqImageSize="";
            this.fileInput.nativeElement.value = ''
            this.notificationService.Success({ message: 'Details added successfully', title: '' });
            break;

          default:
            this.notificationService.Warning({ message: this.response.message, title: '' });
            break;
        }
      });
    }
  }
 
  gridload(){
    this.GetEmployeeTrainingList()
  }
  openModaltraining(open: boolean) {
    document.getElementById("openModalButtontraining").click();
  }
 
  ReqBrowseImageDelete(){
    this.ReqImageName =  '';
    this.ReqImageSize =  '';
    this.fileInput.nativeElement.value = '';
  }
  openEditModal(elem) {
    document.getElementById("openEdittrainingModalButton").click();
    const index = this.employeeTrainingInfo.findIndex(x => x.id == elem.id);
    this.createEditForm(index);
    this.trainingId = this.employeeTrainingInfo[index].id;
    this.getEdittraining(elem.trainingType);
    if(elem.fileName!="" && elem.fileName!=null){
       this.Imageurl = (this.baseUrl + elem.fileName);
       this.isShownReqUrl=true;
       this.isShownReqBrowse=false;
     
     }
     else{
       this.isShownReqUrl=false;
       this.isShownReqBrowse=true;
     }
     if(elem.otherTraining!=""&&elem.otherTraining!=null){
     this.rForm.controls['othertrainingedit'].setValue(elem.otherTraining);
     this.isShownOtherTraining=true;
    }
    else {
    
      this.isShownOtherTraining=false;
    }
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
  UpdateTrainingDetails() {
    var EditEndDatecheck=moment(this.rForm.get('editEndDate').value).format("YYYY-MM-DD");
    if (EditEndDatecheck =="Invalid date") {
      EditEndDatecheck="";
     
    }
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
   
      formData.append('Id', this.trainingId.toString());
      formData.append('EmployeeId', this.employeeId.toString());
      formData.append('MandatoryTraining', this.rForm.get('editMandatory').value);
      formData.append('TrainingType', this.rForm.get('editOptional').value);
      formData.append('StartDate', moment(this.rForm.get('editStartDate').value).format('YYYY-MM-DD').toString());
      formData.append('EndDate', EditEndDatecheck);
      formData.append('CourseType', this.rForm.get('editCoursType').value);
      formData.append('Remarks', this.rForm.get('editRemarks').value);
      formData.append('IsAlert', this.rForm.get('editAlert').value);
      formData.append('OtherTraining', this.rForm.get('othertrainingedit').value);
      this.empService.UpdateEmployeeTraining(formData).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.GetEmployeeTrainingList();
            this.editCancel.nativeElement.click();
            this.notificationService.Success({ message: 'Details updated successfully', title: '' });
            break;

          default:
            break;
        }
      });
    }
  }

  delete(elm) {
    const data = {
      id: elm.id
    }
    this.empService.DeleteEmployeeTraining(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
         this.GetEmployeeTrainingList();
          this.notificationService.Success({ message: this.response.message, title: '' })
          break;
        case 0:
          this.notificationService.Error({ message: this.response.message, title: '' });
        default:
          break;
      }
    });
  }
  DeleteModalTraining(trainingID,_e)
  {

    this.deletetrainingId = trainingID;
  }

  deleteTrainingdetails(event) {
    const data = {
      id: this.deletetrainingId 
    }
    this.empService.DeleteEmployeeTraining(data).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
        case 1:
         this.GetEmployeeTrainingList();
          this.notificationService.Success({ message: this.response.message, title: '' })
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

  getMandatoryList(){
   this.commonservice.getMandatoryTraining().subscribe((res=>{
      if(res){
        this.response = res;
        this.MandatoryList=this.response.responseData||[];
        this.filteredRecords.next(this.MandatoryList.slice());

      }else{

      }
    }));
  }
  getOptionalList(){
    this.commonservice.getOptionalTraining().subscribe((res=>{
      if(res){
        this.response = res;
        this.MandatoryList=this.response.responseData||[];
        this.filteredRecords.next(this.MandatoryList.slice());

      }else{

      }
    }));
  }
  getCoursetype(){
    this.commonservice.getCourseype().subscribe((res=>{
      if(res){
        this.response = res;
        this.CoursetypeList=this.response.responseData||[];
       
      }else{

      }
    }));
  }
  getTrainingtype() {
    this.commonservice.getTrainingType().subscribe((res => {
      if (res) {
        this.response = res;
        this.trainingtypeList = this.response.responseData || [];
        const checkarray = [];
         
          this.trainingtypeList.forEach(element => {
            if(element.codeDescription=="Mandatory Qualification"||element.codeDescription=="Optional Qualification"){
                  const typedata = {
                    codeDescription: element.codeDescription,
                    id:element.id
                  };
                 checkarray.push(typedata);
                  this.docList = checkarray;
                }
          });
  
      }
    }));
  }
 
  getDocumentName(){
    this.commonservice.getDocumentName().subscribe((res=>{
      if(res){
        this.response = res;
        this.MandatoryList=this.response.responseData||[];
       
      }else{

      }
    }));
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    
    switch (sortColumn) {
      case 'trainingtype':
        this.orderColumn = 0;
        break;
      case 'training':
        this.orderColumn = 1;
        break;
      case 'coursetype':
        this.orderColumn = 2;
        break;
        case 'startdate':
          this.orderColumn = 3;
          break;
          case 'enddate':
          this.orderColumn = 4;
          break;
         
          case 'remarks':
          this.orderColumn = 5;
          break;
      case 'createdDate':
        this.orderColumn = 6;
        break;

      default:
        break;
    }
  }
  GetEmployeeTrainingList() {
    this.getSortingOrder();
    const data = {
      employeeId:this.employeeId,
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy

    };
     this.empService.GetEmployeeTrainingList(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      let fundtypearray = [];
      if (this.response.responseData) {
        this.employeeTrainingInfo = this.response.responseData;
        this.dataSourceEducation = new MatTableDataSource(this.employeeTrainingInfo);
         }
      else {
        this.dataSourceEducation = new MatTableDataSource(this.trainingList);
      }

    });
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetEmployeeTrainingList();
  }
  getOtherDocumentName(){
    this.commonservice.getOtherDocumentName().subscribe((res=>{
      if(res){
        this.response = res;
        this.MandatoryList=this.response.responseData||[];
       
      }else{

      }
    }));
  }
  gettraining(event) { 
    this.list=this.trainingtypeList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedName= this.list[index].codeDescription;
    if(this.selectedName=="Mandatory Qualification"){   
      this.getMandatoryList();
     
    }
    else if(this.selectedName=="Optional Qualification"){  
      this.getOptionalList();
    
    }
    
  }
  getEdittraining(event) {  
    this.list=this.trainingtypeList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedNameEdit= this.list[index].codeDescription;
    if(this.selectedNameEdit=="Mandatory Qualification"){   
     this.getMandatoryList();
    
   }
   else if(this.selectedNameEdit=="Optional Qualification"){  
     this.getOptionalList();
    
   }
  
 }


 getEditTrainingtype() {
  this.commonservice.getTrainingType().subscribe((res => {
    if (res) {
      this.response = res;
      this.trainingtypeList = this.response.responseData || [];
      const checkarray = [];
       
        this.trainingtypeList.forEach(element => {
          if(element.codeDescription=="Mandatory Qualification"||element.codeDescription=="Optional Qualification"){
                const typedata = {
                  codeDescription: element.codeDescription,
                  id:element.id
                };
               checkarray.push(typedata);
                this.docList = checkarray;
              }
        });

    }
  }));
}

SubmitClick(){
  this.AddTrainingDetails();
}

SubmitClickEdit(){
  this.UpdateTrainingDetails();
 
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


}
export interface DocumentList{
  id?: number;
  codeDescription:string;
}
