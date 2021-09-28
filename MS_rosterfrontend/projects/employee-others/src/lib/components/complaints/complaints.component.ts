import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EmployeeComplianceInfo } from '../../view-models/employee-compliance-info';
import { ActivatedRoute } from '@angular/router';
import { Paging } from 'projects/viewmodels/paging';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { NotificationService } from 'projects/core/src/projects';
import { FormGroup, FormBuilder, NgForm, Validators } from '@angular/forms';
import { CommonService } from 'projects/lhs-service/src/projects';
import * as moment from 'moment';
import { OtherService } from '../../services/other.service';
import { environment } from 'src/environments/environment';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { APP_DATE_FORMATS, AppDateAdapter } from 'projects/lhs-directives/src/lib/directives/date-format.directive';

@Component({
  selector: 'lib-complaints',
  templateUrl: './complaints.component.html',
  styleUrls: ['./complaints.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class ComplaintsComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumnsRequired: string[] = ['trainingType', 'documentName', 'dateOfIssue', 'dateOfExpiry', 'alert', 'description', 'action'];
  requiredComplianceModel: EmployeeComplianceInfo[] = [];
  dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
  employeeId: number;
  paging: Paging = {};
  response: ResponseModel;
  totalCount: number;
  rForm1: FormGroup;
  @ViewChild('btnAddEducationCancel') cancel: ElementRef;
  rFormEditCompliance: FormGroup;
  @ViewChild('formDirective1') private formDirective1: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  MandatoryList: [] = [];
  trainingtypeList: any;
  selectedType: Event;
  selectedName: any;
  isShownExpiryEdit: boolean = false;
  isShownotherIssueEdit: boolean = false;
  isShownCourseedit = false;
  isShownEnddateedit = false;
  isShownStardateedit = false;
  EditReqImageName1: any;
  EditReqImageSize1: any;
  @ViewChild('fileInputEditcompliance', { static: false }) fileInputEditcompliance: ElementRef;
  @ViewChild('fileInputEdit', { static: false }) fileInputEdit: ElementRef;
  @ViewChild('btnCancelEditOtherPopUp') btnEditOtherCancel: ElementRef;
  baseUrl: string = environment.baseUrl;
  Imageurl: any;
  isShownReqUrl: boolean;
  isShownReqBrowse: boolean;
  deletedocId: any;
  FileSaver = require('file-saver');
  deleteRequireId: any;
  ReqImageName: string;
  ReqImageSize: string;
  list: any;
  selectedcompliance: string;
  docList: DocumentList[];
  constructor(private route: ActivatedRoute, private otherService: OtherService,
    private notificationService: NotificationService, private fb: FormBuilder, private commonService: CommonService) {
    this.route.paramMap.subscribe((params: any) => {
      this.employeeId = params.params.id;
    });
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
    this.getTrainingtype();
    this.getEmployeeComplianceList();
    this.createEditForm();
    this.createForm();
    this.rForm1.get('addAlert').setValue("2");
  }
  gettraining(event) { 
    this.list=this.trainingtypeList;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.selectedcompliance= this.list[index].codeDescription;
     if(this.selectedcompliance=="Mandatory Compliance"){ 
      this.getDocumentName();
      
    }
    else if(this.selectedcompliance=="Other Requirements"){ 
      this.getOtherDocumentName();
    
    }
  }
  getEmployeeComplianceList() {
    const data = {
      employeeId: Number(this.employeeId),
      pageSize: this.paging.pageSize,
      pageNo: this.paging.pageNo
    };
    this.otherService.getComplianceList(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      switch (this.response.status) {
        case 1:
          this.requiredComplianceModel = this.response.responseData;
          this.dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
          break;
        case 0:
          this.notificationService.Warning({ message: this.response.message, title: '' });
          break;

        default:
          break;
      }
    });
  }
  createForm() {
    this.rForm1 = this.fb.group({
      addMandatory: ['', Validators.required],
      addOptional: ['', Validators.required],
      addRemarks: ['', Validators.nullValidator],
      addAlert: ['', null],
      addDateOfIssue: ['', Validators.nullValidator],
      addExpiryDate: ['', Validators.nullValidator]
    });
  }
  createEditForm() {
    this.rFormEditCompliance = this.fb.group({
      editrequireId: ['', null],
      editReqDocName: ['', Validators.required],
      editReqDocType: ['', Validators.required],
      editReqIssueDate: ['', Validators.nullValidator],
      editReqExpiryDate: ['', Validators.nullValidator],
      editReqDescription: ['', Validators.required],
      editReqAlert: ['', null]
    });
  }

  DeleteModalRequire(CompRequireID) {
    this.deleteRequireId = CompRequireID;
  }

  DeleteRequireComplianceInfo(event) {
    const data = {
      id: this.deleteRequireId
    };
    this.otherService.DeleteRequireComplianceDetails(data).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.getEmployeeComplianceList();
      }
      else {
        this.notificationService.Error({ message: "Some error occured", title: null });
      }
    })
  }

  EditCompliance(data) {
    this.getDocumentName();
    this.deletedocId = data.id;
    this.rFormEditCompliance.controls['editrequireId'].patchValue(data.id);
    this.rFormEditCompliance.controls['editReqDocType'].patchValue(data.documentType);
    this.getEdittraining(data.documentType);
    this.rFormEditCompliance.controls['editReqDocName'].patchValue(data.documentName);
    this.rFormEditCompliance.controls['editReqIssueDate'].patchValue(data.issueDate);
    this.rFormEditCompliance.controls['editReqExpiryDate'].patchValue(data.expiryDate);
    this.rFormEditCompliance.controls['editReqDescription'].patchValue(data.description);
    this.rFormEditCompliance.controls['editReqAlert'].patchValue(data.alert == true ? '1' : '2');
    if (data.fileName != "" && data.fileName != null) {
      this.Imageurl = (this.baseUrl + data.fileName);
      this.isShownReqUrl = true;
      this.isShownReqBrowse = false;
    }
    else {
      this.isShownReqUrl = false;
      this.isShownReqBrowse = true;
    }
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getEmployeeComplianceList();
  }

  downloadPdf(docUrl) {
    this.FileSaver.saveAs(docUrl);
  }

  getMandatoryList() {
    this.commonService.getMandatoryTraining().subscribe((res => {
      if (res) {
        this.response = res;
        this.MandatoryList = this.response.responseData || [];
      }
    }));
  }

  getTrainingtype() {
    this.commonService.getcomplianceType().subscribe((res => {
      if (res) {
        this.response = res;
        this.trainingtypeList = this.response.responseData || [];
       
   }
    }));
  }

  EditReqComplianceInfo() {
    if (this.rFormEditCompliance.valid) {
      var datecheckissue=moment(this.rFormEditCompliance.get('editReqIssueDate').value).format("YYYY-MM-DD");
      var datecheckexpiry=moment(this.rFormEditCompliance.get('editReqExpiryDate').value).format("YYYY-MM-DD");
      if (datecheckissue =="Invalid date") {
        datecheckissue=null;
       
      }
       if(datecheckexpiry=="Invalid date") {
       
        datecheckexpiry=null;
      }
      const formData = new FormData();
      if (this.EditReqImageName1 !== undefined) {
        const fileInput1 = this.fileInputEditcompliance.nativeElement;
        let fileLength = fileInput1.files.length;
        let file = fileInput1.files[0];
        if (fileLength > 0) {
          var type = file.type;
          var name = file.name;
          formData.append('Files', fileInput1.files[0]);
        }
      }
      formData.set('Id', this.rFormEditCompliance.get('editrequireId').value.toString());
      formData.append('DocumentName', this.rFormEditCompliance.get('editReqDocName').value);
      formData.append('TrainingType', (this.rFormEditCompliance.get('editReqDocType').value));
      formData.append('IssueDate', datecheckissue);
      formData.append('ExpiryDate', datecheckexpiry);
      formData.append('Description', this.rFormEditCompliance.get('editReqDescription').value);
      formData.append('Alert', this.rFormEditCompliance.get('editReqAlert').value);
      this.otherService.editComplianceInfo(formData).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.notificationService.Success({ message: this.response.message, title: '' });
            this.btnEditOtherCancel.nativeElement.click();
            this.requiredComplianceModel = this.response.responseData;
            this.getEmployeeComplianceList();
        }
      });
    }
  }

  ImageDeleteEdit() {
    const data = {
      id: this.deletedocId
    }
    this.otherService.deleteComplianceDocument(data).subscribe((data: any) => {
      if (data.status == 1) {
        this.isShownReqUrl = false;
        this.isShownReqBrowse = true;
      }
      else {
        this.notificationService.Error({ message: "Some error occured", title: null });
      }
    })
  }

  EditReqImageDelete() {
    this.EditReqImageName1 = '';
    this.EditReqImageName1 = '';
    this.fileInputEdit.nativeElement.value = '';
  }

  getEdittraining(event) {
    debugger;
    this.selectedType = event;
    const index = this.trainingtypeList.findIndex(x => x.id == this.selectedType);
    this.selectedName = this.trainingtypeList[index].codeDescription;
     if (this.selectedName == "Mandatory Compliance") {
      this.getDocumentName();
      this.isShownExpiryEdit = true;
      this.isShownotherIssueEdit = true;
      this.isShownCourseedit = false;
      this.isShownEnddateedit = false;
      this.isShownStardateedit = false;
    }
    else if (this.selectedName == "Other Compliance") {
      this.getOtherDocumentName();
      this.isShownExpiryEdit = true;
      this.isShownotherIssueEdit = true;
      this.isShownCourseedit = false;
      this.isShownEnddateedit = false;
      this.isShownStardateedit = false;
    }
  }

  gridloadcompliances() {
  }

  uploadEditcomplianceDocument(event: any) {
    let fileExtension = null;
    let extension = null;
    if (event.target.files !== undefined) {
      fileExtension = event.target.files[0].name.split('.');
      extension = fileExtension[fileExtension.length - 1].toLowerCase();
      this.EditReqImageName1 = event.target.files[0].name;
      this.EditReqImageSize1 = event.target.files[0].length;
    }
  }

  getOptionalList() {
    this.commonService.getOptionalTraining().subscribe((res => {
      if (res) {
        this.response = res;
        this.MandatoryList = this.response.responseData || [];
      }
    }));
  }
  getDocumentName() {
    this.commonService.getDocumentName().subscribe((res => {
      if (res) {
        this.response = res;
        this.MandatoryList = this.response.responseData || [];
      }
    }));
  }

  getOtherDocumentName() {
    this.commonService.getOtherDocumentName().subscribe((res => {
      if (res) {
        this.response = res;
        this.MandatoryList = this.response.responseData || [];
      }
    }));
  }

  openModaltraining(open: boolean) {
  document.getElementById("openModalButtontraining").click();
 }

AddEmployeeCompliance() {
  if (this.rForm1.valid) {
  var dateIssue=moment(this.rForm1.get('addDateOfIssue').value).format("YYYY-MM-DD");
  var dateExpiry=moment(this.rForm1.get('addExpiryDate').value).format("YYYY-MM-DD");
  if (dateIssue =="Invalid date") {
     dateIssue=null;
   }
   if (dateExpiry=="Invalid date") {
     dateExpiry=null;
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
  formData.set('EmployeeId', this.employeeId.toString());
  formData.append('DocumentName', this.rForm1.get('addMandatory').value);
  formData.append('TrainingType', (this.rForm1.get('addOptional').value));
  formData.append('IssueDate', dateIssue);
  formData.append('ExpiryDate', dateExpiry);
  formData.append('Description', this.rForm1.get('addRemarks').value);
  formData.append('Alert', this.rForm1.get('addAlert').value);
   this.otherService.AddRequireCompliance(formData).subscribe(res => {
      this.response = res;
       switch (this.response.status) {
        case 1:
          this.formDirective1.resetForm();
          this.cancel.nativeElement.click();
          this.ReqImageName="";
          this.ReqImageSize="";
          this.getEmployeeComplianceList();
          this.rForm1.get('addAlert').setValue("2");
          break;
         default:
          this.notificationService.Warning({ message: this.response.message, title: '' });
          break;
      }

    });
  }

}
cancelModal() {
  this.rForm1.reset();
  this.formDirective1.resetForm();
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
openModal(open: boolean) {
  document.getElementById("openModalButton").click();
}
}
export interface DocumentList{
  id?: number;
  codeDescription:string;
}
