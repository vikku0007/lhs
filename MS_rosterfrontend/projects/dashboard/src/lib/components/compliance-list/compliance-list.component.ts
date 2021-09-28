import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import {  FormBuilder } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { EmpServiceService } from '../../services/emp-service.service';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
@Component({
  selector: 'lib-compliance-list',
  templateUrl: './compliance-list.component.html',
  styleUrls: ['./compliance-list.component.scss']
})
export class ComplianceListComponent implements OnInit {
  RequireComplianceData:AllEmployeeRequireCompliance[];
  OtherComplianceData:AllEmployeeOther[];
  deleteRequireId : number;
  RequireInfoModel: AllEmployeeRequireInfo = {};
  deleteOtherId : number;
  OtherInfoModel: AllEmployeeOtherInfo = {};
  paging: Paging = {};
  RequireList=[];
  OtherList=[];
  totalCountReq: number;
  totalCountOth: number;
  orderBy: number;
  orderColumn: number;
  displayedColumnsRequired: string[] = ['sr','clientName', 'document',  'description', 'dateOfIssue', 'hasExpiry', 'dateOfExpiry','uploadDate', 'alert', 'action'];
  dataSourceRequired = new MatTableDataSource(this.RequireList);

  displayedColumnsOther: string[] = ['sr','clientName', 'document',  'description', 'dateOfIssue', 'hasExpiry', 'dateOfExpiry','otheruploadDate', 'alert', 'action'];
  dataSourceOther = new MatTableDataSource(this.OtherList);
  searchByNameOther = null;
  searchByDocNameOther = null;
  searchByNameRequire = null;
  searchByDocNameRequire = null;
  @ViewChild('empNamerequire') employeeNamerequire: ElementRef;
  @ViewChild('docnamerequire') docnamerequire: ElementRef;
  @ViewChild('empNameother') employeeNameother: ElementRef;
  @ViewChild('docnameother') docnameother: ElementRef;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  baseUrl : string = environment.baseUrl;
 
  constructor(private fb: FormBuilder, private empService: EmpServiceService,private router:Router,private notification:NotificationService, private activatedRoute: ActivatedRoute,
    private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }
  applyFilterRequire(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSourceRequired.filter = filterValue.trim().toLowerCase();
  }
  applyFilterOther(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSourceOther.filter = filterValue.trim().toLowerCase();
  }
  ngOnInit(): void {
    this.getRequireComplianceList();
    this.getOtherComplianceList();
    this.dataSourceRequired.sort = this.sort;
    this.dataSourceOther.sort = this.sort;
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceRequired !== undefined ? this.dataSourceRequired.sort = this.sort : this.dataSourceRequired;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getRequireComplianceList())
        )
        .subscribe();
    }, 2000);

  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'fullName':
        this.orderColumn = 0;
        break;
      case 'document':
        this.orderColumn = 1;
        break;
      case 'description':
        this.orderColumn = 2;
        break;
        case 'hasExpiry':
          this.orderColumn = 3;
          break;
          case 'dateOfExpiry':
            this.orderColumn = 4;
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
  getSortingOrderother() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'fullName':
        this.orderColumn = 0;
        break;
      case 'document':
        this.orderColumn = 1;
        break;
      case 'description':
        this.orderColumn = 2;
        break;
        case 'hasExpiry':
          this.orderColumn = 3;
          break;
          case 'dateOfExpiry':
            this.orderColumn = 4;
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
  SearchRequire() {
    this.searchByNameRequire = this.employeeNamerequire.nativeElement.value;
    this.searchByDocNameRequire = "";
    this.getRequireComplianceList();
  }

  getRequireComplianceList() {
    this.getSortingOrder();
    const data = {
      SearchTextByName: this.searchByNameRequire,
      SearchTextBydoc: this.searchByDocNameRequire,
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
      this.empService.getAllRequireComplianceList(data).subscribe((res:any)=>{
        this.totalCountReq = res.total;
      if(res)
      {
        let Requirearray = [];
       
        if(res.responseData!=null)
        {
        this.RequireList=res.responseData;
        
        this.RequireList.forEach(function(value){
          let Commdata={
          Id:value['id'],
          employeeId:value['employeeId'],
          clientName: 
          value['fullName'] ,
          documentName:value['documentName'],
          description:value['description'],
          documentType:value['documentType'],
          dateOfIssue:value['issueDate'],
          dateOfExpiry:value['expiryDate'],
          hasExpiry:value['hasExpiry'],
          alert:value['alert'],
          documentTypeName:value['documentTypeName'],
          document:value['document'],
          fileName:value['fileName'],
          createdDate:value['createdDate'],
         ACTION:''
        }
        Requirearray.push(Commdata);
        })
        this.RequireComplianceData=Requirearray;
        this.dataSourceRequired.data = this.RequireComplianceData
        
        }
        else{
          this.dataSourceRequired.data=Requirearray;
          
         // this.noData = this.dataSource.connect().pipe(map(data => data.length === 0));
        }
    }
      else{
        
      }return this.dataSourceRequired.data;
      
    })
  }
  SearchOther() {
    this.searchByNameOther = this.employeeNameother.nativeElement.value;
    this.searchByDocNameOther = "";
    this.getOtherComplianceList();
  }
  getOtherComplianceList() {
    const data = {
      searchTextByName: this.searchByNameOther,
      SearchTextBydoc: this.searchByDocNameOther,
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo
    };
   
      this.empService.getAllOtherComplianceList(data).subscribe((res:any)=>{
        this.totalCountOth = res.total;
      if(res)
      {
        let Otherarray = [];
       
        if(res.responseData!=null)
        {
        this.OtherList=res.responseData;
        
        this.OtherList.forEach(function(value){
          let Commdata={
          Id:value.requireComp['id'],
          employeeId:value.requireComp['employeeId'],
          clientName: 
          value['firstName'] +
          ((value['middleName'] ===undefined || value['middleName'] === null)?'':' '+value['middleName']) 
          +
          ((value['lastName'] ===undefined || value['lastName'] === null)?'':' '+value['lastName']) ,
          documentName:value.requireComp['otherDocumentName'],
          description:value.requireComp['otherDescription'],
          documentType:value.requireComp['otherDocumentType'],
          dateOfIssue:value.requireComp['otherIssueDate'],
          dateOfExpiry:value.requireComp['otherExpiryDate'],
          hasExpiry:value.requireComp['otherHasExpiry'],
          alert:value.requireComp['otherAlert'],
          documentTypeName:value['documentTypeName'],
          document:value['document'],
          otherFileName:value['otherFileName'],
          createdDate:value['createdDate'],
         ACTION:''
        }
        Otherarray.push(Commdata);
        })
        this.OtherComplianceData=Otherarray;
        this.dataSourceOther.data = this.OtherComplianceData
        }
        else{
          this.dataSourceOther.data=Otherarray;
          
         // this.noData = this.dataSource.connect().pipe(map(data => data.length === 0));
        }
    }
      else{
        
      }return this.dataSourceRequired.data;
      
    })
  }
  PageIndexEventRequire(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getRequireComplianceList();
  }
  PageIndexEventOther(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getOtherComplianceList();
  }
  DeleteModalRequire(CompRequireID,_e)
  {

    this.deleteRequireId = CompRequireID;
  }
  
DeleteRequireComplianceInfo(event){
    this.RequireInfoModel.Id=this.deleteRequireId;
    this.empService.DeleteRequireComplianceDetails(this.RequireInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notification.Success({ message: data.message, title: null });
        this.getRequireComplianceList();
      }
      else {
        this.notification.Error({ message: "Some error occured", title: null });
      }

    })
  }
  DeleteModalOther(CompOtherID,_e)
  {

    this.deleteOtherId = CompOtherID;
  }
  
DeleteOtherComplianceInfo(event){
    this.OtherInfoModel.Id=this.deleteOtherId;
    this.empService.DeleteOtherComplianceDetails(this.OtherInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notification.Success({ message: data.message, title: null });
        this.getOtherComplianceList();
      }
      else {
        this.notification.Error({ message: "Some error occured", title: null });
      }

    })
  }
  OpenEditmodalRequire(employeeId,RequireId,_e)
  {

    this.router.navigate(['/employee/compliance-detail'], { queryParams: { Id: employeeId,EId:RequireId,id:1 } });
  }
  OpenEditmodalother(employeeId,OtherId,_e)
  {

    this.router.navigate(['/employee/compliance-detail'], { queryParams: { Id: employeeId,EId:OtherId,id:2 } });
  }
  FileSaver = require('file-saver');
  downloadPdf(docUrl) {
   
   this.FileSaver.saveAs(docUrl);
 }
 
 
}

export interface AllEmployeeRequireCompliance {
  Id?:number
  employeeId?:number,
  clientName: string;
  documentName: string;
  documentType: number;
  description: string;
  dateOfIssue: string;
  dateOfExpiry: string;
  hasExpiry:string;
  alert:string;
  document: string;
  documentTypeName: string;
  fileName:string;
}

export interface AllEmployeeOther {
  Id?:number
  employeeId?:number,
  clientName: string;
  documentName: string;
  documentType: string;
  description: string;
  dateOfIssue: string;
  dateOfExpiry: string;
  hasExpiry:string;
  alert:string;
  document: string;
  documentTypeName: string;
  otherFileName:string;
}
export interface AllEmployeeRequireInfo {
  Id?:number
}
export interface AllEmployeeOtherInfo {
  Id?:number
}