import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NotificationService, MembershipService } from 'projects/core/src/projects';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import * as moment from 'moment';
import { APP_DATE_FORMATS, AppDateAdapter } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { environment } from 'src/environments/environment';
import { LogoutService } from '../../services/logout.service';
import { AccidentIncidents, IncidentAttachment } from '../../view-models/accident-incident';

@Component({
  selector: 'lib-incident-attachment',
  templateUrl: './incident-attachment.component.html',
  styleUrls: ['./incident-attachment.component.scss']
})

export class IncidentAttachmentComponent implements OnInit {
  
  getErrorMessage:'Please Enter Value';
  response: ResponseModel = {};
  rFormattach: FormGroup;
  requiredComplianceModel: IncidentAttachment[] = [];
  requiredCompliancedata: IncidentAttachment[];
  displayedColumnsRequired: string[] = ['documentName', 'action'];
  dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
  orderBy: number;
  orderColumn: number;
  totalCount: number;
  todayDatemax = new Date();
  @ViewChild('btnEditaccidentCancel') editCancel: ElementRef;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild('fileInput', {static: false}) fileInput: ElementRef;
  clientId: number;
  txtAttachButton: string;
  ImageName: any;
  ImageSize: any;
  EditdocId: any;
  isShownBrowse: boolean = true ; 
  isShownUrl: boolean = false ;
  Imageurl: string;
  isShownReport: boolean;
  AccidentInfoModel: AttachmentInfo = {};
  baseUrl : string = environment.baseUrl;
  shiftId: number;
  employeeId: any;
  constructor(private fb: FormBuilder,private membershipService: MembershipService,private notification:NotificationService,private route: ActivatedRoute,private logoutService:LogoutService,private commonservice:CommonService) {
    this.route.paramMap.subscribe((params: any) => {
      this.clientId = Number(params.params.id);
      this.shiftId = Number(params.params.shiftId);
    });
  }
   ngOnInit(): void {
    this.employeeId = this.membershipService.getUserDetails('employeeId');
     this.CreateFormAttach();
     this.GetAttachDocument();
     this.txtAttachButton="Submit";
  }
  CreateFormAttach() {
    this.rFormattach = this.fb.group({
      AddDocument: ['', Validators.required],
     });
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
    BrowseImageDelete(){
      this.ImageName =  '';
      this.ImageSize =  '';
      this.fileInput.nativeElement.value = '';
    }
    AddAttachment() {
      if (this.rFormattach.valid) {
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
         formData.set('ClientId', this.clientId.toString());
         formData.set('ShiftId', this.shiftId.toString());
         formData.set('EmployeeId', this.employeeId.toString());
         formData.append('DocumentName', this.rFormattach.get('AddDocument').value);
          this.logoutService.AddIncidentDocuments(formData).subscribe(res => {
          this.response = res;
          switch (this.response.status) {
            case 1:  
            this.notification.Success({ message: this.response.message, title: '' });          
            this.rFormattach.reset();
            this.formDirective.resetForm();
            this.ImageName="";
            this.ImageSize="";
            this.fileInput.nativeElement.value = ''
            this.GetAttachDocument();
            break;
            default:
            this.notification.Warning({ message: this.response.message, title: '' });
            break;
          }
  
        });
  
      }
    }
    UpdateAttachment() {
      if (this.rFormattach.valid) {
        const formData = new FormData(); 
        if(this.ImageName==undefined||this.ImageName==""){
       }
        else{
          const fileInput = this.fileInput.nativeElement;
          let fileLength = fileInput.files.length;
          let file = fileInput.files[0];
          if(fileLength > 0)
          {
            var type =  file.type;
            var name =  file.name;
            formData.append('Files', fileInput.files[0]);
          }
        }
      formData.set('Id', this.EditdocId.toString());
      formData.append('DocumentName', this.rFormattach.get('AddDocument').value);
      this.logoutService.UpdateIncidentDocuments(formData).subscribe(res => {
      this.response = res;
      switch (this.response.status) {
      case 1:  
      this.notification.Success({ message: this.response.message, title: '' });          
      this.rFormattach.reset();
      this.formDirective.resetForm();
      this.ImageName="";
      this.ImageSize="";
      this.txtAttachButton="Submit";
      this.isShownBrowse=true;
      this.isShownUrl=false;
      this.GetAttachDocument();
      break;
      default:
      this.notification.Warning({ message: this.response.message, title: '' });
      break;
    }
  });
   }
}
SaveAttachment(){
  if(this.txtAttachButton=="Submit"){
  this.AddAttachment();
   }
  else{
  this.UpdateAttachment();
  }
  }
FileSaver = require('file-saver');
downloadPdf(docUrl) 
{
this.FileSaver.saveAs(docUrl);
}
EditCompliance(data: any) {    
  this.EditdocId=data.id;
  this.txtAttachButton="Update";
  this.rFormattach.controls['AddDocument'].patchValue(data.documentName);
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
showReport(){
this.isShownReport=true;
}
ImageDelete(){
 this.AccidentInfoModel.Id = this.EditdocId;
    this.logoutService.DeleteIncidentDocument(this.AccidentInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.isShownUrl=false;
        this.isShownBrowse=true;
        this.GetAttachDocument();
      }
      else {
        this.notification.Error({ message: "Some error occured", title: null });
      }

    })
 
  }
 
  GetAttachDocument() {
   const data = {
   Id : this.clientId,
   ShiftId:this.shiftId
 };
  this.logoutService.GetIncidentDocumentDetail(data).subscribe(res => {
  this.response = res;
  if (this.response.status > 0) {
  this.totalCount = this.response.total;
  this.requiredComplianceModel = this.response.responseData.incidentDocumentDetailModel;
  if(this.requiredComplianceModel!=null){
  this.dataSourceRequired = new MatTableDataSource(this.requiredComplianceModel);
  }
  else{
  this.dataSourceRequired = new MatTableDataSource(this.requiredCompliancedata);
  }
 }
 
});
  }
}
export interface AttachmentInfo {
  Id?: number;
}