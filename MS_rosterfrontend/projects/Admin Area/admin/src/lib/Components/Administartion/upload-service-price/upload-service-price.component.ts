import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { CommonService } from 'projects/lhs-service/src/projects';
import { AdminService } from '../../../admin.service';
import { NotificationService } from 'projects/core/src/projects';
import { MatTableDataSource } from '@angular/material/table';
import { Paging } from 'projects/viewmodels/paging';
import { PageEvent } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-upload-service-price',
  templateUrl: './upload-service-price.component.html',
  styleUrls: ['./upload-service-price.component.scss']
})
export class UploadServicePriceComponent implements OnInit {
  ReqFileName: any;
  ReqFileSize: any;
  masterInfo: serviceDetails[];
  masterModel: serviceDetails = {};
  @ViewChild('fileInput', { static: false }) fileInput: ElementRef;
  response: ResponseModel = {};
  rForm: FormGroup;
  displayedColumnsLeave: string[] = ['itemnumber', 'itemname', 'rate'];
  dataSourcepayrate: any;
  totalCount: number;
  paging: Paging = {};
  baseUrl: string = environment.baseUrl;

  constructor(private commonService: CommonService, private adminService: AdminService, private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }
  ngOnInit(): void {
    this.GetList();
  }
  isImageType(type, name) {
    debugger
    return (name.match(/\.(xlsx|xls)$/i)) ? true : false;
  };
  uploadExcellfile(event: any) {
    const fileInput = this.fileInput.nativeElement;
    let fileLength = fileInput.files.length;
    let file = fileInput.files[0];
    let fileExtension = null;
    let extension = null;
    if (event.target.files !== undefined) {
      if (!this.isImageType(file.type, file.name)) {
        this.notificationService.Warning({ message: 'File should be .xlsx, .xls', title: null });
        this.fileInput.nativeElement.value = '';
        this.ReqFileName = "";
        this.ReqFileSize = "";
        return;
      }
      fileExtension = event.target.files[0].name.split('.');
      extension = fileExtension[fileExtension.length - 1].toLowerCase();
      this.ReqFileName = event.target.files[0].name;
      this.ReqFileSize = event.target.files[0].length;
    }
  }
  ReqBrowseImageDelete() {
    this.ReqFileName = '';
    this.ReqFileSize = '';
    this.fileInput.nativeElement.value = '';
  }
  uploadfile() {
    const fileInput = this.fileInput.nativeElement;
    let fileLength = fileInput.files.length;
    let file = fileInput.files[0];
    const formData = new FormData();
    if (fileLength > 0) {
      var type = file.type;
      var name = file.name;
      formData.append('Files', fileInput.files[0]);

      this.adminService.Uploadserviceprice(formData).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.notificationService.Success({ message: this.response.message, title: '' });
            this.GetList();
            this.ReqFileName = "";
            this.ReqFileSize = "";;
            break;
          default:
            this.notificationService.Warning({ message: this.response.message, title: '' });
            break;
        }

      });
    }
    else {
      this.notificationService.Warning({ message: "Please select file", title: '' });
    }
  }
  GetList() {
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,

    };
    this.adminService.GetServiceRate(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      if (this.response.responseData != null) {
        this.masterInfo = this.response.responseData;
        this.dataSourcepayrate = new MatTableDataSource(this.masterInfo);
      }
      else {
        this.dataSourcepayrate = new MatTableDataSource([]);
      }
    });
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetList();
  }

  FileSaver = require('file-saver');
  downloadPdf(docUrl) {   
   this.FileSaver.saveAs(docUrl);
 }
}

export interface serviceDetails {
  id?: number;
  supportItemName?: string;
  supportItemNumber?: string;
  rate?: number;
}