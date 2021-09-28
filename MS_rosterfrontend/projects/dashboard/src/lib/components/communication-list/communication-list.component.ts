import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { EmpServiceService } from '../../services/emp-service.service';
import { EmployeeCommunicationInfo } from '../../viewmodel/employee-communication-info';
import { map } from 'rxjs/operators';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
export interface CommunicationElement {
  employeeName: string;
  subject: string;
  assignedTo: string;
  dateTime: string;
}

@Component({
  selector: 'lib-communication-list',
  templateUrl: './communication-list.component.html',
  styleUrls: ['./communication-list.component.scss']
})
export class CommunicationListComponent implements OnInit {
  response: ResponseModel = {};
  getErrorMessage: 'Please Enter Value';
  deleteCommunicationId : number;
   CommunicationData:AllEmployeeCommunicationInfo[];
  CommunicationInfoModel: AllEmployeeCommunication = {};
  paging: Paging = {};
  communicationList=[];
  totalCount: number;
  searchByName = null;
  searchBySubject = null;
  orderBy: number;
  orderColumn: number;
  communicationdata: EmployeeCommunicationInfo[];
  displayedColumns: string[] = ['sr','assignedToName', 'subject',  'createdDate', 'action'];
  dataSource = new MatTableDataSource(this.communicationList);

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('empName') employeeName: ElementRef;
  @ViewChild('subject') Subject: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  constructor(private fb: FormBuilder, private empService: EmpServiceService,private router:Router,private notification:NotificationService, private activatedRoute: ActivatedRoute,
    private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.getAllEmployeeCommunicationList();
    this.dataSource.sort = this.sort;
    
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getAllEmployeeCommunicationList())
        )
        .subscribe();
    }, 2000);

  }
  
  Search() {
    this.searchByName = this.employeeName.nativeElement.value;
    //console.log(this.searchByName);
    this.searchBySubject = this.Subject.nativeElement.value;
    console.log(this.searchBySubject);
    this.getAllEmployeeCommunicationList();
  }
  getAllEmployeeCommunicationList() {
   this.getSortingOrder();
    const data = {
      searchTextByName: this.searchByName,
      searchTextBySubject: this.searchBySubject,
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
      
    };
    
      this.empService.getAllEmployeeCommunication(data).subscribe((res:any)=>{
        this.totalCount = res.total;
        this.response = res;
        switch (this.response.status) {
          case 1:
           this.communicationList = this.response.responseData;
            this.dataSource = new MatTableDataSource(this.communicationList);
            break;
  
          default:
            this.dataSource = new MatTableDataSource(this.communicationdata);
            break;
        }
      
    })
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getAllEmployeeCommunicationList();
  }
  DeleteModal(CommunicationId,_e)
  {

    this.deleteCommunicationId = CommunicationId;
  }
  
DeleteCommunicationInfo(event){
    this.CommunicationInfoModel.Id=this.deleteCommunicationId;
    this.empService.DeleteCommunicationDetails(this.CommunicationInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notification.Success({ message: data.message, title: null });
        this.getAllEmployeeCommunicationList
        ();
      }
      else {
        this.notification.Error({ message: data.message, title: null });
      }

    })
  }
  OpenEditmodal(empid,CommunicationId,_e)
  {
   
    this.router.navigate(['/employee/communication-detail'], { queryParams: { Id: empid,EId:CommunicationId } });
  }

  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'assignedToName':
        this.orderColumn = 0;
        break;
      case 'subject':
        this.orderColumn = 1;
        break;
      case 'createdDate':
        this.orderColumn = 3;
        break;
       case 'createdDate':
        this.orderColumn = 4;
        break;

      default:
        break;
    }
  }

  
}
export interface AllEmployeeCommunicationInfo {
  Id?:number;
  employeeId?: number;
  subject?: string
  assignedTo?: number;
  message?: string;
  createdDate?: Date;
  employeeName:string;
  assignedToName:string;
}
export interface AllEmployeeCommunication {
  Id?:number;
  
}