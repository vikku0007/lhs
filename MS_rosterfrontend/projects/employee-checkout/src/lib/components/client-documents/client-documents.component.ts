import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { DocType } from 'projects/lhs-service/src/lib/viewmodels/gender';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import * as moment from 'moment';
import { environment } from 'src/environments/environment';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DocumentDetails } from '../../view-models/document-details';
import { LogoutService } from '../../services/logout.service';


@Component({
  selector: 'lib-client-documents',
  templateUrl: './client-documents.component.html',
  styleUrls: ['./client-documents.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class ClientDocumentsComponent implements OnInit {
  getErrorMessage: 'Please Enter Value';
   @ViewChild(MatSort, { static: true }) sort: MatSort;
   employeeId: number;
   responseModel: ResponseModel = {};
   rForm: FormGroup;
   paging: Paging = {};
   ComplianceModel: DocumentDetails[] = [];
   ComplianceInfo: DocumentDetails = {};
   Compliancedata: DocumentDetails[];
   @ViewChild('btnCancelAddReqPopUp') btnCancel: ElementRef;
   @ViewChild('btnCancelAddOtherPopUp') btnOtherCancel: ElementRef;
   @ViewChild('btnCancelEditOtherPopUp') btnEditOtherCancel: ElementRef;
   @ViewChild('btnOtherUpdateCancel') btnUpdateOtherCancel: ElementRef;
   @ViewChild('formDirective') private formDirective: NgForm;
   @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
   @ViewChild('fileInputEdit', {static: false}) fileInputEdit: ElementRef;
   @ViewChild('download', {static: false}) download: ElementRef;
   @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
   orderBy: number;
   orderColumn: number;
   isShownBrowse: boolean = false ; 
   isShownUrl: boolean = false ;
   isShowndocument: boolean=true;
   rFormEditCompliance: FormGroup;
   otherAddForm: FormGroup;
   rFormEditOtherCompliance: FormGroup;
   employeePrimaryInfo: {};
   docInfoModel: DocumentInfo = {};
   deleteRequireId: number;
   dtypes: DocType[] = [];
   displayedColumnsRequired: string[] = ['documentType','documentName',  'description', 'dateOfIssue', 'hasExpiry', 'dateOfExpiry', 'alert','createdDate' ,'action'];
   dataSourceRequired = new MatTableDataSource(this.ComplianceModel);
   ClientId: number;
   Documentnamelist: any;
   Imageurl: any;
   deletedocId: any;
   ImageName: string;
   ImageSize: string;
   EditImageName: string;
   EditImageSize: string;
   baseUrl : string = environment.baseUrl;
   EditList: DocumentDetails[] = [];
   EditId: number;
   totalCount: number;
   list: any;
   DocumentTypelist: any;
   selectedType: any;
   selectedName: any;
  shiftId: any;
   
   constructor(private route: ActivatedRoute, private logoutservice: LogoutService, private fb: FormBuilder,
     private notificationService: NotificationService, private commonService: CommonService) { 
      this.route.paramMap.subscribe((params: any) => {
        this.ClientId = Number(params.params.id);
        this.shiftId = Number(params.params.shiftId);
       });
     }
 
   ngOnInit(): void {
     this.paging.pageNo = 1;
     this.paging.pageSize = 10;
     this.ClientId > 0 ? this.getClientDocumentDetails() : null;
     this.dataSourceRequired.sort = this.sort;
     this.getDocuments();
     this.createForm();
     this.editForm();
     this.rForm.get('addHasExpiry').setValue("2");
     this.rForm.get('addAlert').setValue("2");
   }
   ngAfterViewInit(): void {
     setTimeout(() => {
       this.dataSourceRequired !== undefined ? this.dataSourceRequired.sort = this.sort : this.dataSourceRequired;
       this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
       merge(this.sort.sortChange, this.paginator.page)
         .pipe(
           tap(() => this.getClientDocumentDetails())
         )
         .subscribe();
     }, 2000);
   
   }
   getSortingOrder() {
     const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
     this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
     switch (sortColumn) {
     case 'documentType':
     this.orderColumn = 0;
     break;
     case 'documentName':
     this.orderColumn = 1;
     break;
     case 'description':
     this.orderColumn = 2;
     break;
     case 'dateOfIssue':
     this.orderColumn = 3;
     break;
     case 'hasExpiry':
     this.orderColumn = 4;
     break;
     case 'dateOfExpiry':
     this.orderColumn = 5;
     break;
     case 'alert':
     this.orderColumn = 6;
     break;
     case 'createdDate':
     this.orderColumn = 7;
     break;
     default:
     break;
     }
   }
  
   getDocuments() {
     this.commonService.getDocumentType().subscribe(res => {
       this.responseModel = res;
       if (this.responseModel.status > 0) {
         this.DocumentTypelist=this.responseModel.responseData||[];
       }
     });
   }
   
   createForm() {
     this.rForm = this.fb.group({
       addDocType: ['', Validators.required],
       addDocument: ['', Validators.required],
       addDateOfIssue: ['', Validators.nullValidator],
       addExpiryDate: ['', Validators.nullValidator],
       addDescription: ['', Validators.required],
       addHasExpiry: ['', null],
       addAlert: ['', null]
     });
   }
 
   editForm() {
     this.rFormEditCompliance = this.fb.group({
       editrequireId: ['', null],
       editDocName: ['', Validators.required],
       editDocType: ['', Validators.required],
       editIssueDate: ['', Validators.nullValidator],
       editExpiryDate: ['', Validators.nullValidator],
       editDescription: ['', Validators.required],
       editHasExpiry: ['', null],
       editAlert: ['', null]
     });
   }
   cancelModal() {
     this.rForm.reset();
     this.formDirective.resetForm();
   }
   getClientDocumentDetails() {
     this.getSortingOrder();
     const data = {
       ClientId: this.ClientId,
       ShiftId:this.shiftId,
       PageSize: this.paging.pageSize,
       pageNo: this.paging.pageNo,
       OrderBy: this.orderColumn,
       SortOrder: this.orderBy
 
     }
     this.logoutservice.GetClientCompliancesList(data).subscribe(res => {
       this.responseModel = res;
       this.totalCount = this.responseModel.total;
       switch (this.responseModel.status) {
         case 1:
           this.ComplianceModel = this.responseModel.responseData;
           this.dataSourceRequired = new MatTableDataSource(this.ComplianceModel);
           break;
          default:
          this.dataSourceRequired = new MatTableDataSource(this.Compliancedata);
           break;
       }
 
     });
   }
   BrowseImageDelete(){
     this.ImageName =  '';
     this.ImageSize =  '';
     this.fileInput.nativeElement.value = '';
   }
   BrowseEditImageDelete(){
     this.EditImageName =  '';
     this.EditImageSize =  '';
     this.fileInputEdit.nativeElement.value = '';
   }
   uploadDocument(event: any) {
     let fileExtension = null;
     let extension = null;
     if (event.target.files !== undefined) {
       fileExtension = event.target.files[0].name.split('.');
       extension = fileExtension[fileExtension.length - 1].toLowerCase();
       this.ImageName =  event.target.files[0].name;
       this.ImageSize =  event.target.files[0].length;
      }
   }
    AddClientCompliance() {
     if (this.rForm.valid) {
      var datecheck=moment(this.rForm.get('addDateOfIssue').value).format("YYYY-MM-DD");
      var datecheckExpiry=moment(this.rForm.get('addExpiryDate').value).format("YYYY-MM-DD");
      if (datecheck =="Invalid date") {
      datecheck=null;
      }
      if (datecheckExpiry=="Invalid date") {
       datecheckExpiry=null;
      }
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
      formData.set('ClientId', this.ClientId.toString());
      formData.set('ShiftId', this.shiftId.toString());
      formData.append('DocumentName', this.rForm.get('addDocument').value);
      formData.append('DocumentType', (this.rForm.get('addDocType').value));
      formData.append('IssueDate', datecheck);
      formData.append('ExpiryDate', datecheckExpiry);
      formData.append('Description', this.rForm.get('addDescription').value);
      formData.append('HasExpiry', this.rForm.get('addHasExpiry').value);
      formData.append('Alert', this.rForm.get('addAlert').value);
      this.logoutservice.AddClientCompliancesInfo(formData).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
      case 1:            
      this.ComplianceInfo = this.responseModel.responseData;
      this.notificationService.Success({ message: 'Details added successfully', title: '' });
      this.btnCancel.nativeElement.click();
      this.rForm.reset();
      this.formDirective.resetForm();
      this.fileInput.nativeElement.value="";
      this.rForm.get('addHasExpiry').setValue("2");
      this.rForm.get('addAlert').setValue("2");
      this.ImageName="";
      this.ImageSize="";
      this.ComplianceModel.unshift(this.ComplianceInfo);
      this.getClientDocumentDetails();
      break;
      default:
      this.notificationService.Warning({ message: this.responseModel.message, title: '' });
      break;
      }
 
       });
 
     }
   }
   gridload(){
     this.getClientDocumentDetails()
   }
   EditCompliance(data: any) {    
   this.deletedocId=data.id;
   this.rFormEditCompliance.controls['editrequireId'].patchValue(data.id);
   this.rFormEditCompliance.controls['editDocName'].patchValue(data.documentName);
   this.rFormEditCompliance.controls['editDocType'].patchValue(data.documentType);
   this.selectChangeHandler(data.documentType);
   this.rFormEditCompliance.controls['editIssueDate'].patchValue(data.issueDate);
   this.rFormEditCompliance.controls['editExpiryDate'].patchValue(data.expiryDate);
   this.rFormEditCompliance.controls['editDescription'].patchValue(data.description);
   this.rFormEditCompliance.controls['editHasExpiry'].patchValue(data.hasExpiry == true ? '1' : '2');
   this.rFormEditCompliance.controls['editAlert'].patchValue(data.alert == true ? '1' : '2');
   if(data.fileName!="" && data.fileName!=null){
   this.Imageurl = (this.baseUrl + data.fileName);
   this.isShownUrl=true;
   this.isShownBrowse=false;
  }
  else{
   this.isShownUrl=false;
   this.isShownBrowse=true;
  }
   }
   downloaddoc(){
    this.Imageurl;
   }
   ImageDelete(){
         this.docInfoModel.Id = this.deletedocId;
         this.logoutservice.DeleteClientDocument(this.docInfoModel).subscribe((data: any) => {
         if (data.status == 1) {
           this.isShownUrl=false;
           this.isShownBrowse=true;
         }
         else {
           this.notificationService.Error({ message: "Some error occured", title: null });
         }
     })
    
   }
   uploadEditDocument(event: any) {
     let fileExtension = null;
     let extension = null;
     if (event.target.files !== undefined) {
     fileExtension = event.target.files[0].name.split('.');
     extension = fileExtension[fileExtension.length - 1].toLowerCase();
     this.EditImageName =  event.target.files[0].name;
     this.EditImageSize =  event.target.files[0].length;
     }
   }
   EditDocumentInfo() {
     if(this.rFormEditCompliance.valid) {
      var dateIssue=moment(this.rFormEditCompliance.get('editIssueDate').value).format("YYYY-MM-DD");
      var dateExpiry=moment(this.rFormEditCompliance.get('editExpiryDate').value).format("YYYY-MM-DD");
       if (dateIssue =="Invalid date") {
        dateIssue=null;
       }
       if (dateExpiry=="Invalid date") {
          dateExpiry=null;
       }
       const formData = new FormData(); 
       if(this.EditImageName!=undefined){
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
      formData.set('Id', this.rFormEditCompliance.get('editrequireId').value.toString());
      formData.append('DocumentName', this.rFormEditCompliance.get('editDocName').value);
      formData.append('DocumentType', (this.rFormEditCompliance.get('editDocType').value));
      formData.append('IssueDate', dateIssue);
      formData.append('ExpiryDate', dateExpiry);
      formData.append('Description', this.rFormEditCompliance.get('editDescription').value);
      formData.append('HasExpiry', this.rFormEditCompliance.get('editHasExpiry').value);
      formData.append('Alert', this.rFormEditCompliance.get('editAlert').value);
      this.logoutservice.UpdateClientCompliancesInfo(formData).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
      case 1:
      this.notificationService.Success({ message: this.responseModel.message, title: '' });
      this.btnEditOtherCancel.nativeElement.click();
      this.ComplianceInfo = this.responseModel.responseData;
      const index = this.ComplianceModel.findIndex(x => x.id == this.ComplianceInfo.id);
      if (index > -1) {              
      this.ComplianceModel[index].documentName = this.ComplianceInfo.documentName;
      this.ComplianceModel[index].documentTypeName = this.ComplianceInfo.documentTypeName;
      this.ComplianceModel[index].description = this.ComplianceInfo.description;
      this.ComplianceModel[index].issueDate = this.ComplianceInfo.issueDate;
      this.ComplianceModel[index].hasExpiry = this.ComplianceInfo.hasExpiry;
      this.ComplianceModel[index].expiryDate = this.ComplianceInfo.expiryDate;
      this.ComplianceModel[index].alert = this.ComplianceInfo.alert;
      }
      break;
      default:
      break;
      }
     });
    }
   }
   selectChangeHandler(event:any) {
    this.selectedType = event;
   // this.getDocumentName(this.selectedType);
   }
   getDocumentName(){
     this.commonService.getClientDocuments().subscribe((res=>{
       if(res){
         this.responseModel = res;
         this.Documentnamelist=this.responseModel.responseData||[];
       }else{
         this.notificationService.Warning({ message: "No Documents Found", title: '' });
       }
     }));
   }
   DeleteModalRequire(documentID, _e) {
    this.deleteRequireId = documentID;
   }
 
   DeleteDoumentInfo(event) {
     this.docInfoModel.Id = this.deleteRequireId;
     this.logoutservice.DeleteClientComplianceInfo(this.docInfoModel).subscribe((data: any) => {
       if (data.status == 1) {
         this.notificationService.Success({ message: data.message, title: null });
         this.getClientDocumentDetails();
       }
       else {
         this.notificationService.Error({ message: data.message, title: null });
       }
 
     })
   }
   PageIndexEvent(event: PageEvent) {
     this.paging.pageNo = event.pageIndex + 1;
     this.paging.pageSize = event.pageSize;
     this.getClientDocumentDetails();
   }
  require: any
  FileSaver = require('file-saver');
  downloadPdf(docUrl) {
   this.FileSaver.saveAs(docUrl);
 }
 }
 export interface DocumentInfo {
   Id?: number
 }
