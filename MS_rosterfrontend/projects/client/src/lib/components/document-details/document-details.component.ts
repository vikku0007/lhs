import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, NgForm, FormControl } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { DocType } from 'projects/lhs-service/src/lib/viewmodels/gender';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { ClientComplianceDetails } from '../../view-models/Client-compliance-details';
import { ClientService } from '../../services/client.service';
import * as moment from 'moment';
import { environment } from 'src/environments/environment';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { merge, Subject, ReplaySubject } from 'rxjs';
import { tap, takeUntil } from 'rxjs/operators';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';

// For Document Type
interface DocumentType {
  value: string;
  name: string;
}

export interface RequiredDocumentElement {
  documentName: string;
  documentType: string;
  description: string;
  dateOfIssue: string;
  dateOfExpiry: string;
}



@Component({
  selector: 'lib-document-details',
  templateUrl: './document-details.component.html',
  styleUrls: ['./document-details.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class DocumentDetailsComponent implements OnInit {
 getErrorMessage: 'Please Enter Value';
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  employeeId: number;
  responseModel: ResponseModel = {};
  rForm: FormGroup;
  paging: Paging = {};
  requiredComplianceModel: ClientComplianceDetails[] = [];
  empComplianceInfo: ClientComplianceDetails = {};
  requiredCompliancedata: ClientComplianceDetails[];
  @ViewChild('btnCancelAddReqPopUp') btnCancel: ElementRef;
  @ViewChild('btnCancelAddOtherPopUp') btnOtherCancel: ElementRef;
  @ViewChild('btnCancelEditOtherPopUp') btnEditOtherCancel: ElementRef;
  @ViewChild('btnOtherUpdateCancel') btnUpdateOtherCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  @ViewChild('fileInputEdit', {static: false}) fileInputEdit: ElementRef;
  @ViewChild('download', {static: false}) download: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  orderBy: number;
  orderColumn: number;
  isShownBrowse: boolean = false ; 
  isShownUrl: boolean = false ;
  isShowndocument: boolean=true;
  rFormEditCompliance: FormGroup;
  otherAddForm: FormGroup;
  rFormEditOtherCompliance: FormGroup;
  employeePrimaryInfo: {};
  RequireInfoModel: AllEmployeeRequireInfo = {};
  deleteRequireId: number;
  dtypes: DocType[] = [];
  displayedColumnsRequired: string[] = ['documentType','documentName',  'description', 'dateOfIssue', 'hasExpiry', 'dateOfExpiry', 'alert','createdDate' ,'action'];
  dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
  ClientId: number;
  Documentnamelist: any;
  Imageurl: any;
  deletedocId: any;
  ImageName: string;
  ImageSize: string;
  EditImageName: string;
  EditImageSize: string;
  baseUrl : string = environment.baseUrl;
  EditList: ClientComplianceDetails[] = [];
  EditId: number;
  totalCount: number;
  list: any;
  DocumentTypelist: any;
  selectedType: any;
  selectedName: any;
  selectedcompliance: any;
  
  constructor(private route: ActivatedRoute, private clientService: ClientService, private loaderService: LoaderService, private fb: FormBuilder,
    private notificationService: NotificationService, private commonService: CommonService) { }

  ngOnInit(): void {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.route.queryParams.subscribe(params => {
      this.ClientId = parseInt(params.Id);
      this.EditId = parseInt(params.EId);
    });
    if(this.EditId>0){
      this.getdocumentById();
     }
    this.ClientId > 0 ? this.getRequiredComplianceDetails() : null;
    this.dataSourceRequired.sort = this.sort;
    this.getDocuments();
    this.createRequiredForm();
    this.editRequiredForm();
    this.searchcompliances();
    this.rForm.get('addHasExpiry').setValue("2");
    this.rForm.get('addAlert').setValue("2");
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceRequired !== undefined ? this.dataSourceRequired.sort = this.sort : this.dataSourceRequired;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getRequiredComplianceDetails())
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
    if (!this.Documentnamelist) {
      return;
    }
    let search = this.control.value;
    if (!search) {
      this.filteredRecords.next(this.Documentnamelist.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecords.next(
      this.Documentnamelist.filter(department => department.codeDescription.toLowerCase().indexOf(search) > -1)
     );
    }
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
  getdocumentById(){
    const data = {
      Id: Number(this.EditId),
     
    };
    this.clientService.GetClientCompliancesDetails(data).subscribe(res => {
      this.responseModel = res;
         this.EditList = this.responseModel.responseData;
         document.getElementById("openEditModalButton").click();
          this.deletedocId=this.EditList[0]['id'];
         this.rFormEditCompliance.controls['editrequireId'].patchValue(this.EditList[0]['id']);
         this.rFormEditCompliance.controls['editReqDocName'].patchValue(this.EditList[0]['documentName']);
         this.rFormEditCompliance.controls['editReqDocType'].patchValue(this.EditList[0]['documentType']);
         this.rFormEditCompliance.controls['editReqIssueDate'].patchValue(this.EditList[0]['issueDate']);
         this.rFormEditCompliance.controls['editReqExpiryDate'].patchValue(this.EditList[0]['expiryDate']);
         this.rFormEditCompliance.controls['editReqDescription'].patchValue(this.EditList[0]['description']);
         this.rFormEditCompliance.controls['editReqHasExpiry'].patchValue(this.EditList[0]['hasExpiry']== true ? '1' : '2');
         this.rFormEditCompliance.controls['editReqAlert'].patchValue(this.EditList[0]['alert'] == true ? '1' : '2');
         if(this.EditList[0]['id']!=0 && this.EditList[0]['id']!=null){
         this.Imageurl = (this.baseUrl + this.EditList[0]['id']);
         this.isShownUrl=true;
         this.isShownBrowse=false;
     
     }
     else{
       this.isShownUrl=false;
       this.isShownBrowse=true;
     
     }
      })
  }
  getDocuments() {
    this.commonService.getDocumentType().subscribe(res => {
      this.responseModel = res;
      if (this.responseModel.status > 0) {
        this.DocumentTypelist=this.responseModel.responseData||[];
      }
    });
  }
 
  getOptionalDocument(){
    this.commonService.getOptionalDocument().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.Documentnamelist=this.responseModel.responseData||[];
        this.filteredRecords.next(this.Documentnamelist.slice());

      }else{

      }
    }));
  }
  createRequiredForm() {
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

  editRequiredForm() {
    this.rFormEditCompliance = this.fb.group({
      editrequireId: ['', null],
      editReqDocName: ['', Validators.required],
      editReqDocType: ['', Validators.required],
      editReqIssueDate: ['', Validators.nullValidator],
      editReqExpiryDate: ['', Validators.nullValidator],
      editReqDescription: ['', Validators.required],
      editReqHasExpiry: ['', null],
      editReqAlert: ['', null]
    });
  }
  cancelModal() {
    this.rForm.reset();
    this.formDirective.resetForm();
  }
  getRequiredComplianceDetails() {
    this.getSortingOrder();
    const data = {
      ClientId: this.ClientId,
      PageSize: this.paging.pageSize,
      pageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
 }
    this.clientService.GetClientCompliancesList(data).subscribe(res => {
      this.responseModel = res;
      this.totalCount = this.responseModel.total;
      switch (this.responseModel.status) {
        case 1:
          this.requiredComplianceModel = this.responseModel.responseData;
          this.dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
          break;
         default:
         this.dataSourceRequired = new MatTableDataSource(this.requiredCompliancedata);
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

      // const arrayExtensions = ['doc','docx','xlsx','pptx','ppt','xls','pdf','jpg','jpeg'];

      // if (arrayExtensions.lastIndexOf(extension) === -1) {
      //  this.notificationService.Warning({ message: 'Only doc,docx,xlsx,pptx,ppt,xls,jpg,jpeg and pdf file types are allowed!', title: null });
      //  return;
      // } else {
        this.ImageName =  event.target.files[0].name;
         this.ImageSize =  event.target.files[0].length;
       
      // }

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
    formData.append('DocumentName', this.rForm.get('addDocument').value);
    formData.append('DocumentType', (this.rForm.get('addDocType').value));
    formData.append('IssueDate', datecheck);
    formData.append('ExpiryDate', datecheckExpiry);
    formData.append('Description', this.rForm.get('addDescription').value);
    formData.append('HasExpiry', this.rForm.get('addHasExpiry').value);
    formData.append('Alert', this.rForm.get('addAlert').value);
    this.clientService.AddClientCompliancesInfo(formData).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:            
            this.empComplianceInfo = this.responseModel.responseData;
            this.notificationService.Success({ message: 'Details added successfully', title: '' });
            this.btnCancel.nativeElement.click();
            this.rForm.reset();
            this.formDirective.resetForm();
            this.fileInput.nativeElement.value="";
            this.rForm.get('addHasExpiry').setValue("2");
            this.rForm.get('addAlert').setValue("2");
            this.ImageName="";
            this.ImageSize="";
            this.requiredComplianceModel.unshift(this.empComplianceInfo);
            this.getRequiredComplianceDetails();
            break;
            default:
            this.notificationService.Warning({ message: this.responseModel.message, title: '' });
            break;
        }

      });

    }
  }
  gridload(){
    this.getRequiredComplianceDetails()
  }
  EditCompliance(data: any) {    
    this.selectChangeHandler(data.documentType);
    this.deletedocId=data.id;
    this.rFormEditCompliance.controls['editrequireId'].patchValue(data.id);
    this.rFormEditCompliance.controls['editReqDocName'].patchValue(data.documentName);
    this.rFormEditCompliance.controls['editReqDocType'].patchValue(data.documentType);
    this.rFormEditCompliance.controls['editReqIssueDate'].patchValue(data.issueDate);
    this.rFormEditCompliance.controls['editReqExpiryDate'].patchValue(data.expiryDate);
    this.rFormEditCompliance.controls['editReqDescription'].patchValue(data.description);
    this.rFormEditCompliance.controls['editReqHasExpiry'].patchValue(data.hasExpiry == true ? '1' : '2');
    this.rFormEditCompliance.controls['editReqAlert'].patchValue(data.alert == true ? '1' : '2');
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
    this.RequireInfoModel.Id = this.deletedocId;
      this.clientService.DeleteClientDocument(this.RequireInfoModel).subscribe((data: any) => {
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
  EditComplianceInfo() {
    if (this.rFormEditCompliance.valid) {
      var dateIssue=moment(this.rFormEditCompliance.get('editReqIssueDate').value).format("YYYY-MM-DD");
      var dateExpiry=moment(this.rFormEditCompliance.get('editReqExpiryDate').value).format("YYYY-MM-DD");
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
      formData.append('DocumentName', this.rFormEditCompliance.get('editReqDocName').value);
      formData.append('DocumentType', (this.rFormEditCompliance.get('editReqDocType').value));
      formData.append('IssueDate', dateIssue);
      formData.append('ExpiryDate', dateExpiry);
      formData.append('Description', this.rFormEditCompliance.get('editReqDescription').value);
      formData.append('HasExpiry', this.rFormEditCompliance.get('editReqHasExpiry').value);
      formData.append('Alert', this.rFormEditCompliance.get('editReqAlert').value);
      this.clientService.UpdateClientCompliancesInfo(formData).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: this.responseModel.message, title: '' });
            this.btnEditOtherCancel.nativeElement.click();
            this.empComplianceInfo = this.responseModel.responseData;
            const index = this.requiredComplianceModel.findIndex(x => x.id == this.empComplianceInfo.id);
            if (index > -1) {              
              this.requiredComplianceModel[index].documentName = this.empComplianceInfo.documentName;
              this.requiredComplianceModel[index].documentTypeName = this.empComplianceInfo.documentTypeName;
              this.requiredComplianceModel[index].description = this.empComplianceInfo.description;
              this.requiredComplianceModel[index].issueDate = this.empComplianceInfo.issueDate;
              this.requiredComplianceModel[index].hasExpiry = this.empComplianceInfo.hasExpiry;
              this.requiredComplianceModel[index].expiryDate = this.empComplianceInfo.expiryDate;
              this.requiredComplianceModel[index].alert = this.empComplianceInfo.alert;
            }
            break;
            default:
            this.notificationService.Error({ message: this.responseModel.message, title: '' });
            break;
        }

      });

    }
  }
  selectChangeHandler(event:any) {
    this.list=this.DocumentTypelist;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedcompliance= this.list[index].codeDescription;
     if(this.selectedcompliance=="Mandatory Document"){ 
      this.getDocumentName();
      
    }
    else if(this.selectedcompliance=="Optional Document"){ 
      this.getOptionalDocument();
    
    }
  
  }
  getDocumentName(){
    this.commonService.getClientDocuments().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.Documentnamelist=this.responseModel.responseData||[];
        this.filteredRecords.next(this.Documentnamelist.slice());

      }else{
        this.notificationService.Warning({ message: "No Documents Found", title: '' });
      }
    }));
  }
  DeleteModalRequire(CompRequireID, _e) {
   this.deleteRequireId = CompRequireID;
  }

  DeleteRequireComplianceInfo(event) {
    this.RequireInfoModel.Id = this.deleteRequireId;
    this.clientService.DeleteClientComplianceInfo(this.RequireInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.getRequiredComplianceDetails();
      }
      else {
        this.notificationService.Error({ message: "Some error occured", title: null });
      }

    })
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getRequiredComplianceDetails();
  }
  require: any
 FileSaver = require('file-saver');
 downloadPdf(docUrl) {
  
  this.FileSaver.saveAs(docUrl);
}
}
export interface AllEmployeeRequireInfo {
  Id?: number
}