import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import {  FormBuilder } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { EmpServiceService } from 'projects/dashboard/src/lib/services/emp-service.service';
import { ClientService } from '../../services/client.service';
import { environment } from 'src/environments/environment';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
@Component({
  selector: 'lib-document-list',
  templateUrl: './document-list.component.html',
  styleUrls: ['./document-list.component.scss']
})
export class DocumentListComponent implements OnInit {
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
  displayedColumnsRequired: string[] = ['sr','clientName','documentTypeName', 'document',  'description', 'dateOfIssue', 'hasExpiry', 'dateOfExpiry', 'alert','createdDate', 'action'];
  dataSourceRequired = new MatTableDataSource(this.RequireList);

  displayedColumnsOther: string[] = ['sr','employeeName', 'documentName',  'description', 'dateOfIssue', 'hasExpiry', 'dateOfExpiry','createdDate','alert', 'action'];
  dataSourceOther = new MatTableDataSource(this.OtherList);
  searchByNameOther = null;
  searchByDocNameOther = null;
  searchByNameRequire = null;
  searchByDocNameRequire = null;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('empNamerequire') employeeNamerequire: ElementRef;
  @ViewChild('docnamerequire') docnamerequire: ElementRef;
  @ViewChild('empNameother') employeeNameother: ElementRef;
  @ViewChild('docnameother') docnameother: ElementRef;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  baseUrl : string = environment.baseUrl;
  orderBy: number;
  orderColumn: number;
  constructor(private fb: FormBuilder, private clientservice: ClientService,private router:Router,private notification:NotificationService, private activatedRoute: ActivatedRoute,
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
  SearchRequire() {
    this.searchByNameRequire = this.employeeNamerequire.nativeElement.value;
    this.searchByDocNameRequire = "";
    this.getRequireComplianceList();
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'clientName':
        this.orderColumn = 0;
        break;
        case 'documentTypeName':
          this.orderColumn = 1;
          break;
        case 'document':
          this.orderColumn = 2;
          break;
        case 'description':
        this.orderColumn = 3;
        break;
        case 'dateOfIssue':
          this.orderColumn = 4;
          break;
        case 'hasExpiry':
          this.orderColumn = 5;
          break;
          case 'dateOfExpiry':
            this.orderColumn = 6;
            break;
            case 'alert':
              this.orderColumn = 7;
              break;
      case 'createdDate':
        this.orderColumn = 8;
        break;
  
      default:
        break;
    }
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
      this.clientservice.GetClientAlldocumentList(data).subscribe((res:any)=>{
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
          clientId:value['clientId'],
          employeeId:value['employeeId'],
          clientName: 
          value['firstName'] +
          ((value['middleName'] ===undefined || value['middleName'] === null)?'':' '+value['middleName']) 
          +
          ((value['lastName'] ===undefined || value['lastName'] === null)?'':' '+value['lastName']) ,
          documentName:value['documentName'],
          description:value['description'],
          documentType:value['documentType'],
          dateOfIssue:value['issueDate'],
          dateOfExpiry:value['expiryDate'],
          hasExpiry:value['hasExpiry'],
          alert:value['alert'],
          document:value['document'],
          documentTypeName:value['documentTypeName'],
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
  
  
  PageIndexEventRequire(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getRequireComplianceList();
  }
 
  DeleteModalRequire(CompRequireID,_e)
  {

    this.deleteRequireId = CompRequireID;
  }
 
DeleteRequireComplianceInfo(event){
    this.RequireInfoModel.Id=this.deleteRequireId;
    this.clientservice.DeleteClientComplianceInfo(this.RequireInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notification.Success({ message: data.message, title: null });
        this.getRequireComplianceList();
      }
      else {
        this.notification.Error({ message: "Some error occured", title: null });
      }

    })
  }
  
  OpenEditmodal(empId,docId,_e)
  {

    this.router.navigate(['/client/document-details'], { queryParams: { Id: empId,EId:docId } });
  }

 require: any
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
  documentTypeName: number;
}

export interface AllEmployeeOther {
  Id?:number
  employeeId?:number,
  employeeName: string;
  documentName: string;
  documentType: string;
  description: string;
  dateOfIssue: string;
  dateOfExpiry: string;
  hasExpiry:string;
  alert:string;
}
export interface AllEmployeeRequireInfo {
  Id?:number
}
export interface AllEmployeeOtherInfo {
  Id?:number
}