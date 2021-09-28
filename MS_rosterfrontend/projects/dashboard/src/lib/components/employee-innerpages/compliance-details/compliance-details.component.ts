import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { EmpServiceService } from '../../../services/emp-service.service';
import { FormBuilder, FormGroup, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { EmployeeComplianceDetails, EmployeeOtherComplianceDetails } from '../../../viewmodel/employee-compliance-details';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { DocType } from 'projects/lhs-service/src/lib/viewmodels/gender';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import * as moment from 'moment';
import { MatCommonModule } from '@angular/material/core';
import { Constants } from '../../../config/constants';
import { environment } from 'src/environments/environment';
import { PageEvent } from '@angular/material/paginator';



// For Document Type
// interface DocumentType {
//   value: string;
//   name: string;
// }

export interface OtherComplianceElement {
  employeeName: string;
  documentName: string;
  documentType: string;
  description: string;
  dateOfIssue: string;
  dateOfExpiry: string;
}

const OTHER_COMPLIANCE_DATA: OtherComplianceElement[] = [
  { employeeName: 'John Doe', documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020' },
  { employeeName: 'Petey Cruiser', documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020' },
  { employeeName: 'Anna Sthesia', documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020' },
  { employeeName: 'Paul MolivePaul Molive', documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020' },
  { employeeName: 'Gail Forcewind', documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020' },
];


@Component({
  selector: 'app-compliance-details',
  templateUrl: './compliance-details.component.html',
  styleUrls: ['./compliance-details.component.scss']
})
export class ComplianceDetailsComponent implements OnInit {

  getErrorMessage: 'Please Enter Value';
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  employeeId: number;
  responseModel: ResponseModel = {};
  rForm: FormGroup;
  paging: Paging = {};
  requiredComplianceModel: EmployeeComplianceDetails[] = [];
  otherComplianceModel: EmployeeOtherComplianceDetails[] = [];
  empComplianceInfo: EmployeeComplianceDetails = {};
  requiredCompliancedata: EmployeeComplianceDetails[];
  otherCompliancedata: EmployeeOtherComplianceDetails[];
  empOtherComplianceInfo: EmployeeOtherComplianceDetails = {};
  @ViewChild('btnCancelAddReqPopUp') btnCancel: ElementRef;
  @ViewChild('btnCancelAddOtherPopUp') btnOtherCancel: ElementRef;
  @ViewChild('btnCancelEditOtherPopUp') btnEditOtherCancel: ElementRef;
  @ViewChild('btnOtherUpdateCancel') btnUpdateOtherCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('formDirective1') private formDirective1: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  @ViewChild('fileInputEdit', {static: false}) fileInputEdit: ElementRef;
  @ViewChild('fileInputOther', {static: false}) fileInputOther: ElementRef;
  @ViewChild('fileInputEditOther', {static: false}) fileInputEditOther: ElementRef;
  isShownReqBrowse: boolean = false ; 
  isShownReqUrl: boolean = false ;
isShownOthBrowse: boolean = false ; 
  isShownOthUrl: boolean = false ;
  rFormEditCompliance: FormGroup;
  otherAddForm: FormGroup;
  rFormEditOtherCompliance: FormGroup;
  employeePrimaryInfo: {};
  RequireInfoModel: AllEmployeeRequireInfo = {};
  deleteOtherId: number;
  deleteRequireId: number;
  OtherInfoModel: AllEmployeeOtherInfo = {};
  //For Person
  // dtypes: DocumentType[] = [
  //   { value: '', name: 'Select Document Type' },
  //   { value: '1', name: 'Doc' },
  //   { value: '2', name: 'Excel' },
  //   { value: '3', name: 'Powerpoint' }
  // ];
  dtypes: DocType[] = [];
  // selectedDocumentType = '1';

  displayedColumnsRequired: string[] = ['documentName',  'description', 'dateOfIssue', 'hasExpiry', 'dateOfExpiry', 'alert','appliedDate', 'action'];
  dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);

  displayedColumnsOther: string[] = ['documentName',  'description', 'dateOfIssue', 'hasExpiry', 'dateOfExpiry', 'alert','otherappliedDate' ,'action'];
  dataSourceOther = new MatTableDataSource(this.otherComplianceModel);
  Documentnamelist: any;
  DocumentOthernamelist: any;
  Imageurl: any;
  deletedocId: number;
  ImageOthurl: any;
  deleteOtherdocId: any;
  EditReqImageName: any;
  EditReqImageSize: any;
  ReqImageSize: any;
  ReqImageName: any;
  OthImageName: any;
  OthImageSize: any;
  OthEditImageSize: any;
  OthEditImageName: string;
  baseUrl : string = environment.baseUrl;
  EditId: number;
  EDId: number;
  modelId: any;
  totalCount: number;
  EditList: AllEmployeeRequireInfo[] = [];
  OtherEditList: AllEmployeeOtherInfo[] = [];
  totalCountOther: number;
  delete(elm) {
    // this.dataSourceRequired.data = this.dataSourceRequired.data.filter(i => i !== elm)
    this.dataSourceOther.data = this.dataSourceOther.data.filter(i => i !== elm)
  }

  constructor(private route: ActivatedRoute, private empService: EmpServiceService, private loaderService: LoaderService, private fb: FormBuilder,
    private notificationService: NotificationService, private commonService: CommonService) { }

  ngOnInit(): void {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.route.queryParams.subscribe(params => {
      this.employeeId = parseInt(params.Id);
      this.EditId = parseInt(params.EId);
      this.modelId=parseInt(params.id);
      if(this.EditId>0 && this.modelId>0&& this.modelId==1){
        
        this.getRequiredetailById();
      
      }
      else if(this.EditId>0 && this.modelId>0 && this.modelId==2){
         this.getOtherdetailsById();
      }
    });
   
    this.employeeId > 0 ? this.getRequiredComplianceDetails() : null;
    this.employeeId > 0 ? this.getOtherComplianceDetails() : null;
    this.dataSourceRequired.sort = this.sort;
    this.dataSourceOther.sort = this.sort;
    this.getDocuments();
    this.createRequiredForm();
    this.editRequiredForm();
    this.createOtherForm();
    this.editOtherRequiredForm();
    this.getDocumentName();
    this.getOtherDocumentName();
    if (this.employeeId > 0) {
      this.getEmployeeDetails();
    }
      this.rForm.get('addHasExpiry').setValue("1");
      this.rForm.get('addAlert').setValue("1");
      this.otherAddForm.get('addOtherHasExpiry').setValue("1");
      this.otherAddForm.get('addOtherAlert').setValue("1");
  }
  getRequiredetailById(){
    const data = {
      Id: Number(this.EditId),
     
    };
   
    this.empService.getRequiredataById(data).subscribe(res => {
      this.responseModel = res;
         this.EditList = this.responseModel.responseData;
         document.getElementById("openEditModalButton").click();
         this.deletedocId=this.EditList[0]['id']; 
         this.rFormEditCompliance.controls['editrequireId'].patchValue(this.EditList[0]['id']);
         this.rFormEditCompliance.controls['editReqDocName'].patchValue(this.EditList[0]['documentName']);
        // this.rFormEditCompliance.controls['editReqDocType'].patchValue(this.EditList[0]['documentType']);
         this.rFormEditCompliance.controls['editReqIssueDate'].patchValue(this.EditList[0]['issueDate']);
         this.rFormEditCompliance.controls['editReqExpiryDate'].patchValue(this.EditList[0]['expiryDate']);
         this.rFormEditCompliance.controls['editReqDescription'].patchValue(this.EditList[0]['description']);
         this.rFormEditCompliance.controls['editReqHasExpiry'].patchValue(this.EditList[0]['hasExpiry'] == true ? '1' : '2');
         this.rFormEditCompliance.controls['editReqAlert'].patchValue(this.EditList[0]['alert'] == true ? '1' : '2');
         if(this.EditList[0]['fileName'] && this.EditList[0]['fileName']){
         
           this.Imageurl = (this.baseUrl + this.EditList[0]['fileName']);
           this.isShownReqUrl=true;
           this.isShownReqBrowse=false;
         
         }
         else{
           this.isShownReqUrl=false;
           this.isShownReqBrowse=true;
         
         }
       })
  }

  getOtherdetailsById(){
    const data = {
      Id: Number(this.EditId),
     
    };
 
    this.empService.getOtherDetailById(data).subscribe(res => {
      this.responseModel = res;
         this.EditList = this.responseModel.responseData;
         document.getElementById("openOtherEditModalButton").click();
         this.deleteOtherdocId=this.EditList[0]['id']; 
         this.rFormEditOtherCompliance.controls['editId'].patchValue(this.EditList[0]['id']);
         this.rFormEditOtherCompliance.controls['editOtherDocName'].patchValue(this.EditList[0]['otherDocumentName']);
      //   this.rFormEditOtherCompliance.controls['editOtherDocType'].patchValue(this.EditList[0]['otherDocumentType']);
         this.rFormEditOtherCompliance.controls['editOtherIssueDate'].patchValue(this.EditList[0]['otherIssueDate']);
         this.rFormEditOtherCompliance.controls['editOtherExpiryDate'].patchValue(this.EditList[0]['otherExpiryDate']);
         this.rFormEditOtherCompliance.controls['editOtherDescription'].patchValue(this.EditList[0]['otherDescription']);
         this.rFormEditOtherCompliance.controls['editOtherHasExpiry'].patchValue(this.EditList[0]['otherHasExpiry'] == true ? '1' : '2');
         this.rFormEditOtherCompliance.controls['editOtherAlert'].patchValue(this.EditList[0]['otherAlert'] == true ? '1' : '2');
         if(this.EditList[0]['otherFileName'] && this.EditList[0]['otherFileName']){
             this.ImageOthurl = (this.baseUrl + this.EditList[0]['otherFileName']);
            //(this.ImageOthurl)=data.otherFileName;
              this.isShownOthUrl=true;
              this.isShownOthBrowse=false;
   
    }
    else{
      this.isShownOthUrl=false;
      this.isShownOthBrowse=true;
    
    }
       })
  }
  getDocuments() {
    this.commonService.getDocumentType().subscribe(res => {
      this.responseModel = res;
      if (this.responseModel.status > 0) {
        this.dtypes = this.responseModel.responseData;
        this.dtypes.forEach(x => x.value = x.id);
      }
    });
  }
  getDocumentName(){
    this.commonService.getDocumentName().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.Documentnamelist=this.responseModel.responseData||[];
       
      }else{

      }
    }));
  }
  getOtherDocumentName(){
    this.commonService.getOtherDocumentName().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.DocumentOthernamelist=this.responseModel.responseData||[];
       
      }else{

      }
    }));
  }
  createRequiredForm() {
    this.rForm = this.fb.group({
      addDocument: ['', Validators.required],
     // addDocType: ['', Validators.required],
      addDateOfIssue: ['', Validators.required],
      addExpiryDate: ['', Validators.required],
      addDescription: ['', Validators.required],
      addHasExpiry: ['', null],
      addAlert: ['', null]
    });
  }

  createOtherForm() {
    this.otherAddForm = this.fb.group({
      addOtherDocument: ['', Validators.required],
    //  addOtherDocType: ['', Validators.required],
      addOtherDateOfIssue: ['', Validators.required],
      addOtherExpiryDate: ['', Validators.required],
      addOtherDescription: ['', Validators.required],
      addOtherHasExpiry: ['', null],
      addOtherAlert: ['', null]
    });
  }

  editRequiredForm() {
    this.rFormEditCompliance = this.fb.group({
      editrequireId: ['', null],
      editReqDocName: ['', Validators.required],
     // editReqDocType: ['', Validators.required],
      editReqIssueDate: ['', Validators.required],
      editReqExpiryDate: ['', Validators.required],
      editReqDescription: ['', Validators.required],
      editReqHasExpiry: ['', null],
      editReqAlert: ['', null]
    });
  }


  editOtherRequiredForm() {
    this.rFormEditOtherCompliance = this.fb.group({
      editId: ['', null],
      editOtherDocName: ['', Validators.required],
     // editOtherDocType: ['', Validators.required],
      editOtherIssueDate: ['', Validators.required],
      editOtherExpiryDate: ['', Validators.required],
      editOtherDescription: ['', Validators.required],
      editOtherHasExpiry: ['', null],
      editOtherAlert: ['', null]
    });
  }

  getRequiredComplianceDetails() {
    const data = {
      EmployeeId: this.employeeId,
      PageSize: this.paging.pageSize,
      pageNo: this.paging.pageNo,
      pageSizeoth:this.paging.pageSize,
      pageNooth: this.paging.pageNo
    }
    this.empService.getRequireComplianceList(data).subscribe(res => {
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

  getOtherComplianceDetails() {
    const data = {
      EmployeeId: this.employeeId,
      PageSize: this.paging.pageSize,
      pageNo: this.paging.pageNo
    }
    this.empService.GetOtherComplianceDetails(data).subscribe(res => {
      this.responseModel = res;
      this.totalCountOther = this.responseModel.total;
      switch (this.responseModel.status) {
        case 1:
          this.otherComplianceModel = this.responseModel.responseData;
          this.dataSourceOther = new MatTableDataSource(this.otherComplianceModel);
          break;

        default:
          this.dataSourceOther = new MatTableDataSource(this.otherCompliancedata);
          break;
      }

    });

  }
  uploadRequireDocument(event: any) {
    let fileExtension = null;
    let extension = null;
    if (event.target.files !== undefined) {

      fileExtension = event.target.files[0].name.split('.');
      extension = fileExtension[fileExtension.length - 1].toLowerCase();

      // const arrayExtensions = ['doc','docx','xlsx','pptx','ppt','xls','pdf','jpg','jpeg'];

      // if (arrayExtensions.lastIndexOf(extension) === -1) {
      //  this.notificationService.Warning({ message: 'Only doc,docx,xlsx,pptx,ppt,xls,jpeg,jpg and pdf file types are allowed!', title: null });
      //  return;
      // } else {
        this.ReqImageName =  event.target.files[0].name;
         this.ReqImageSize =  event.target.files[0].length;
       
      // }

    }
  }
  ReqBrowseImageDelete(){
    this.ReqImageName =  '';
    this.ReqImageSize =  '';
    this.fileInput.nativeElement.value = '';
  }
  EditReqImageDelete(){
    this.EditReqImageName =  '';
    this.EditReqImageSize =  '';
    this.fileInputEdit.nativeElement.value = '';
  }
  AddRequiredCompliance() {
    if (this.rForm.valid) {
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
    formData.set('EmployeeId', this.employeeId.toString());
    formData.append('DocumentName', this.rForm.get('addDocument').value);
   // formData.append('DocumentType', (this.rForm.get('addDocType').value));
    formData.append('IssueDate', moment(this.rForm.get('addDateOfIssue').value).format("YYYY-MM-DD"));
    formData.append('ExpiryDate', moment(this.rForm.get('addExpiryDate').value).format("YYYY-MM-DD"));
    formData.append('Description', this.rForm.get('addDescription').value);
    formData.append('HasExpiry', this.rForm.get('addHasExpiry').value);
    formData.append('Alert', this.rForm.get('addAlert').value);
      // const data = {
      //   EmployeeId: this.employeeId,
      //   DocumentName: this.rForm.get('addDocument').value,
      //   DocumentType: Number(this.rForm.get('addDocType').value),
      //   IssueDate: this.rForm.get('addDateOfIssue').value,
      //   ExpiryDate: this.rForm.get('addExpiryDate').value,
      //   Description: this.rForm.get('addDescription').value,
      //   HasExpiry: Boolean(this.rForm.get('addHasExpiry').value == '1' ? true : false),
      //   Alert: Boolean(this.rForm.get('addAlert').value == '1' ? true : false),
      //   FileName:this.empComplianceInfo.fileName
      // }
      this.empService.AddRequireCompliance(formData).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:            
            this.empComplianceInfo = this.responseModel.responseData;
            this.notificationService.Success({ message: 'Details added successfully', title: '' });
            this.btnCancel.nativeElement.click();
            this.rForm.reset();
            this.formDirective.resetForm();
            this.fileInput.nativeElement.value="";
            this.rForm.get('addHasExpiry').setValue("1");
            this.rForm.get('addAlert').setValue("1");
            this.ReqImageName="";
            this.ReqImageSize="";
            this.getRequiredComplianceDetails();
           this.requiredComplianceModel.unshift(this.empComplianceInfo);
            this.dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
            break;

          default:
            this.notificationService.Warning({ message: this.responseModel.message, title: '' });
            break;
        }

      });

    }
  }

  EditCompliance(data: any) {
    this.deletedocId=data.id;    
    this.rFormEditCompliance.controls['editrequireId'].patchValue(data.id);
    this.rFormEditCompliance.controls['editReqDocName'].patchValue(data.documentName);
   // this.rFormEditCompliance.controls['editReqDocType'].patchValue(data.documentType);
    this.rFormEditCompliance.controls['editReqIssueDate'].patchValue(data.issueDate);
    this.rFormEditCompliance.controls['editReqExpiryDate'].patchValue(data.expiryDate);
    this.rFormEditCompliance.controls['editReqDescription'].patchValue(data.description);
    this.rFormEditCompliance.controls['editReqHasExpiry'].patchValue(data.hasExpiry == true ? '1' : '2');
    this.rFormEditCompliance.controls['editReqAlert'].patchValue(data.alert == true ? '1' : '2');
    if(data.fileName!="" && data.fileName!=null){
     // (this.Imageurl)=data.fileName;
      this.Imageurl = (this.baseUrl + data.fileName);
      this.isShownReqUrl=true;
      this.isShownReqBrowse=false;
    
    }
    else{
      this.isShownReqUrl=false;
      this.isShownReqBrowse=true;
    
    }
      }
      ImageDelete(){
    
        this.RequireInfoModel.Id = this.deletedocId;
          this.empService.DeleteEmployeeRequireDocument(this.RequireInfoModel).subscribe((data: any) => {
            if (data.status == 1) {
              this.isShownReqUrl=false;
              this.isShownReqBrowse=true;
              
            }
            else {
              this.notificationService.Error({ message: "Some error occured", title: null });
            }
      
          })
       
      }
      gridload(){
        this.getRequiredComplianceDetails()
      }
      gridloadOther(){
        this.getOtherComplianceDetails()
      }
  uploadEditDocument(event: any) {
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
        this.EditReqImageName =  event.target.files[0].name;
         this.EditReqImageSize =  event.target.files[0].length;
       
      // }

    }
  }
  EditReqComplianceInfo() {
    if (this.rFormEditCompliance.valid) {
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
      formData.set('Id', this.rFormEditCompliance.get('editrequireId').value.toString());
      formData.append('DocumentName', this.rFormEditCompliance.get('editReqDocName').value);
      //formData.append('DocumentType', (this.rFormEditCompliance.get('editReqDocType').value));
      formData.append('IssueDate', moment(this.rFormEditCompliance.get('editReqIssueDate').value).format("YYYY-MM-DD"));
      formData.append('ExpiryDate', moment(this.rFormEditCompliance.get('editReqExpiryDate').value).format("YYYY-MM-DD"));
      formData.append('Description', this.rFormEditCompliance.get('editReqDescription').value);
      formData.append('HasExpiry', this.rFormEditCompliance.get('editReqHasExpiry').value);
      formData.append('Alert', this.rFormEditCompliance.get('editReqAlert').value);
      // const data = {
      //   Id: this.rFormEditCompliance.get('editrequireId').value,
      //   DocumentName: this.rFormEditCompliance.get('editReqDocName').value,
      //   DocumentType: Number(this.rFormEditCompliance.get('editReqDocType').value),
      //   IssueDate: this.rFormEditCompliance.get('editReqIssueDate').value,
      //   ExpiryDate: this.rFormEditCompliance.get('editReqExpiryDate').value,
      //   Description: this.rFormEditCompliance.get('editReqDescription').value,
      //   HasExpiry: Boolean(this.rFormEditCompliance.get('editReqHasExpiry').value == '1' ? true : false),
      //   Alert: Boolean(this.rFormEditCompliance.get('editReqAlert').value == '1' ? true : false)
      // }
      this.empService.EditRequireCompliance(formData).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: this.responseModel.message, title: '' });
            this.btnEditOtherCancel.nativeElement.click();
            this.empComplianceInfo = this.responseModel.responseData;
            this.getRequiredComplianceDetails();
            const index = this.requiredComplianceModel.findIndex(x => x.id == this.empComplianceInfo.id);
            if (index > -1) {              
              this.requiredComplianceModel[index].documentName = this.empComplianceInfo.documentName;
             // this.requiredComplianceModel[index].documentTypeName = this.empComplianceInfo.documentTypeName;
              this.requiredComplianceModel[index].description = this.empComplianceInfo.description;
              this.requiredComplianceModel[index].issueDate = this.empComplianceInfo.issueDate;
              this.requiredComplianceModel[index].hasExpiry = this.empComplianceInfo.hasExpiry;
              this.requiredComplianceModel[index].expiryDate = this.empComplianceInfo.expiryDate;
              this.requiredComplianceModel[index].alert = this.empComplianceInfo.alert;
            }
            break;

          default:
            break;
        }

      });

    }
  }
  uploadOtherDocument(event: any) {
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
        this.OthImageName =  event.target.files[0].name;
         this.OthImageSize =  event.target.files[0].length;
       
      // }

    }
  }
  OthBrowseImageDelete(){
    this.OthImageName =  '';
    this.OthImageSize =  '';
    this.fileInputEdit.nativeElement.value = '';
  }
  OthEditBrowseImageDelete(){
    this.OthEditImageName =  '';
    this.OthEditImageSize =  '';
    this.fileInputEditOther.nativeElement.value = '';
  }
  AddOtherRequiredCompliance() {
    if (this.otherAddForm.valid) {
      const fileInput = this.fileInputOther.nativeElement;
      let fileLength = fileInput.files.length;
      let file = fileInput.files[0];
      const formData = new FormData(); 
      if(fileLength > 0)
      {
        var type =  file.type;
        var name =  file.name;
           
          formData.append('Files', fileInput.files[0]);
      }
      formData.set('EmployeeId', this.employeeId.toString());
      formData.append('OtherDocumentName', this.otherAddForm.get('addOtherDocument').value);
     // formData.append('OtherDocumentType', (this.otherAddForm.get('addOtherDocType').value));
      formData.append('OtherIssueDate', moment(this.otherAddForm.get('addOtherDateOfIssue').value).format("YYYY-MM-DD"));
      formData.append('OtherExpiryDate', moment(this.otherAddForm.get('addOtherExpiryDate').value).format("YYYY-MM-DD"));
      formData.append('OtherDescription', this.otherAddForm.get('addOtherDescription').value);
      formData.append('OtherHasExpiry', this.otherAddForm.get('addOtherHasExpiry').value);
      formData.append('OtherAlert', this.otherAddForm.get('addOtherAlert').value);
      // const data = {
      //   EmployeeId: this.employeeId,
      //   OtherDocumentName: this.otherAddForm.get('addOtherDocument').value,
      //   OtherDocumentType: Number(this.otherAddForm.get('addOtherDocType').value),
      //   OtherIssueDate: this.otherAddForm.get('addOtherDateOfIssue').value,
      //   OtherExpiryDate: this.otherAddForm.get('addOtherExpiryDate').value,
      //   OtherDescription: this.otherAddForm.get('addOtherDescription').value,
      //   OtherHasExpiry: Boolean(this.otherAddForm.get('addOtherHasExpiry').value),
      //   OtherAlert: Boolean(this.otherAddForm.get('addOtherAlert').value),
        
      // }
      this.empService.AddOtherComplianceDetails(formData).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: 'Details added successfully', title: '' });
            this.btnOtherCancel.nativeElement.click();
            this.otherAddForm.reset();
            this.formDirective1.resetForm();
            this.fileInputOther.nativeElement.value="";
            this.otherAddForm.get('addOtherHasExpiry').setValue("1");
            this.otherAddForm.get('addOtherAlert').setValue("1");
            this.OthImageName="";
            this.OthImageSize="";
            this.getOtherComplianceDetails();
            break;

          default:
            this.notificationService.Success({ message: this.responseModel.message, title: '' });
            break;
        }

      });
    }
  }
  uploadEditOtherDocument(event: any) {
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
        this.OthEditImageName =  event.target.files[0].name;
         this.OthEditImageSize =  event.target.files[0].length;
       
      // }

    }
  }
  EditOtherCompliance(data: any) {
    this.deleteOtherdocId=data.id;
    this.rFormEditOtherCompliance.controls['editId'].patchValue(data.id);
    this.rFormEditOtherCompliance.controls['editOtherDocName'].patchValue(data.otherDocumentName);
   // this.rFormEditOtherCompliance.controls['editOtherDocType'].patchValue(data.otherDocumentType);
    this.rFormEditOtherCompliance.controls['editOtherIssueDate'].patchValue(data.otherIssueDate);
    this.rFormEditOtherCompliance.controls['editOtherExpiryDate'].patchValue(data.otherExpiryDate);
    this.rFormEditOtherCompliance.controls['editOtherDescription'].patchValue(data.otherDescription);
    this.rFormEditOtherCompliance.controls['editOtherHasExpiry'].patchValue(data.otherHasExpiry == true ? '1' : '2');
    this.rFormEditOtherCompliance.controls['editOtherAlert'].patchValue(data.otherAlert == true ? '1' : '2');
    if(data.otherFileName!="" && data.otherFileName!=null){
      this.ImageOthurl = (this.baseUrl + data.otherFileName);
      //(this.ImageOthurl)=data.otherFileName;
      this.isShownOthUrl=true;
      this.isShownOthBrowse=false;
   
    }
    else{
      this.isShownOthUrl=false;
      this.isShownOthBrowse=true;
    
    }
  }
  ImageOthDelete(){
    this.RequireInfoModel.Id = this.deleteOtherdocId;
      this.empService.DeleteEmployeeOtherDocument(this.RequireInfoModel).subscribe((data: any) => {
        if (data.status == 1) {
          this.isShownOthUrl=false;
          this.isShownOthBrowse=true;
          
        }
        else {
          this.notificationService.Error({ message: "Some error occured", title: null });
        }
  
      })
   
  }
  EditOtherComplianceInfo() {
    if (this.rFormEditOtherCompliance.valid) {
      const formData = new FormData(); 
      if(this.OthEditImageName==undefined){

      }
      else{
      const fileInput = this.fileInputEditOther.nativeElement;
      let fileLength = fileInput.files.length;
      let file = fileInput.files[0];
      
      if(fileLength > 0)
      {
        var type =  file.type;
        var name =  file.name;
           
          formData.append('Files', fileInput.files[0]);
      }
    }
      formData.set('Id', this.rFormEditOtherCompliance.get('editId').value.toString());
      formData.append('OtherDocumentName', this.rFormEditOtherCompliance.get('editOtherDocName').value);
    //  formData.append('OtherDocumentType', (this.rFormEditOtherCompliance.get('editOtherDocType').value));
      formData.append('OtherIssueDate', moment(this.rFormEditOtherCompliance.get('editOtherIssueDate').value).format("YYYY-MM-DD"));
      formData.append('OtherExpiryDate', moment(this.rFormEditOtherCompliance.get('editOtherExpiryDate').value).format("YYYY-MM-DD"));
      formData.append('OtherDescription', this.rFormEditOtherCompliance.get('editOtherDescription').value);
      formData.append('OtherHasExpiry', this.rFormEditOtherCompliance.get('editOtherHasExpiry').value);
      formData.append('OtherAlert', this.rFormEditOtherCompliance.get('editOtherAlert').value);
      // const data = {
      //   Id: this.rFormEditOtherCompliance.get('editId').value,
      //   EmployeeId: this.employeeId,
      //   OtherDocumentName: this.rFormEditOtherCompliance.get('editOtherDocName').value,
      //   OtherDocumentType: Number(this.rFormEditOtherCompliance.get('editOtherDocType').value),
      //   OtherIssueDate: this.rFormEditOtherCompliance.get('editOtherIssueDate').value,
      //   OtherExpiryDate: this.rFormEditOtherCompliance.get('editOtherExpiryDate').value,
      //   OtherDescription: this.rFormEditOtherCompliance.get('editOtherDescription').value,
      //   OtherHasExpiry: Boolean(this.rFormEditOtherCompliance.get('editOtherHasExpiry').value),
      //   OtherAlert: Boolean(this.rFormEditOtherCompliance.get('editOtherAlert').value),
      //   OtherFileName:this.empOtherComplianceInfo.otherFileName
      // }
      this.empService.UpdateOtherComplianceDetails(formData).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: 'Details updated successfully', title: '' });
            this.btnUpdateOtherCancel.nativeElement.click();
            this.getOtherComplianceDetails();
            break;

          default:
            break;
        }

      });

    }
  }

  getEmployeeDetails() {

    this.empService.getEmployeeDetails(this.employeeId).subscribe(res => {

      this.responseModel = res;
      console.log('response', this.responseModel);

    });
  }
  DeleteModalRequire(CompRequireID, _e) {

    this.deleteRequireId = CompRequireID;
  }

  DeleteRequireComplianceInfo(event) {
    this.RequireInfoModel.Id = this.deleteRequireId;
    this.empService.DeleteRequireComplianceDetails(this.RequireInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.getRequiredComplianceDetails();
      }
      else {
        this.notificationService.Error({ message: "Some error occured", title: null });
      }

    })
  }
  DeleteModalOther(CompOtherID, _e) {

    this.deleteOtherId = CompOtherID;
  }

  DeleteOtherComplianceInfo(event) {
    this.OtherInfoModel.Id = this.deleteOtherId;
    this.empService.DeleteOtherComplianceDetails(this.OtherInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.getOtherComplianceDetails();
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
  PageIndexEventOther(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getOtherComplianceDetails();
  }
  FileSaver = require('file-saver');
 downloadPdf(docUrl) {
  
  this.FileSaver.saveAs(docUrl);
}

}
export interface AllEmployeeRequireInfo {
  Id?: number
}
export interface AllEmployeeOtherInfo {
  Id?: number
}