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
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface AppraisalElement {
  employeeName: string;
  employeeID: number;
  department: string;
  typesOfAppraisal: string;
}

const APPRAISAL_DATA: AppraisalElement[] = [
  {employeeName: 'John Doe', employeeID: 5421358, department: 'Admin', typesOfAppraisal: 'Annual'},
  {employeeName: 'Petey Cruiser', employeeID: 5421456, department: 'IT', typesOfAppraisal: 'Half Yearly'},
  {employeeName: 'Anna Sthesia', employeeID: 5421856, department: 'Admin', typesOfAppraisal: 'Annual'},
  {employeeName: 'Paul MolivePaul Molive', employeeID: 5421325, department: 'Heathcare ', typesOfAppraisal: 'Half Yearly'},
  {employeeName: 'Gail Forcewind', employeeID: 5421754, department: 'Admin', typesOfAppraisal: 'Annual'},
];

@Component({
  selector: 'lib-appraisal-list',
  templateUrl: './appraisal-list.component.html',
  styleUrls: ['./appraisal-list.component.scss']
})
export class AppraisalListComponent implements OnInit {

  getErrorMessage:'Please Enter Value';
  AppraisalData:AllEmployeeAppraisalInfo[];
  AppraisalInfoModel: AllEmployeeAppraisalInfo = {};
  paging: Paging = {};
  AppraisalList=[];
  deleteAppraisalId : number;
  totalCount: number;
  searchByName = null;
  searchByEmpId = null;
  displayedColumns: string[] = ['sr','employeeName', 'employeeId',  'appraisalType','appraisalDateFrom','appraisalDateTo','appliedDate' ,'action'];
  dataSource = new MatTableDataSource(this.AppraisalList);
  orderBy: number;
  orderColumn: number;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('empName') employeeName: ElementRef;
  @ViewChild('empId') Subject: ElementRef;
  constructor(private fb: FormBuilder, private empService: EmpServiceService,private router:Router,private notification:NotificationService, private activatedRoute: ActivatedRoute,
    private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.getAllEmployeeAppraisalList();
    this.dataSource.sort = this.sort;
    
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getAllEmployeeAppraisalList())
        )
        .subscribe();
    }, 2000);

  }
  Search() {
    this.searchByName = this.employeeName.nativeElement.value;
    this.searchByEmpId = "";
    this.getAllEmployeeAppraisalList();
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'employeeName':
        this.orderColumn = 0;
        break;
       case 'employeeId':
        this.orderColumn = 1;
        break;
        case 'appraisalType':
          this.orderColumn = 2;
          break;
          case 'appraisalDateFrom':
          this.orderColumn = 3;
          break;
          case 'appraisalDateTo':
          this.orderColumn = 4;
          break;
         case 'createdDate':
         this.orderColumn = 5;
         break;

      default:
        break;
    }
  }
  getAllEmployeeAppraisalList() {
    this.getSortingOrder();
    const data = {
      searchTextByName: this.searchByName,
      SearchTextByEmpId: this.searchByEmpId,
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy

    };
      
      this.empService.getAllEmployeeAppraisalList(data).subscribe((res:any)=>{
        this.totalCount = res.total;
      if(res)
      {
        let Communicationarray = [];
       
        if(res.responseData!=null)
        {
        this.AppraisalList=res.responseData;
        
        this.AppraisalList.forEach(function(value){
          let Commdata={
          Id:value.requireComp['id'],
         
          employeeName: 
          value['firstName'] +
          ((value['middleName'] ===undefined || value['middleName'] === null)?'':' '+value['middleName']) 
          +
          ((value['lastName'] ===undefined || value['lastName'] === null)?'':' '+value['lastName']) ,
          employeeId:value.requireComp['employeeId'],
          departmentName:value['departmentId'],
         // appraisalType:value.requireComp['appraisalType'],
          appraisalType:value['appraisalTypeName'],
          EmployeedetailId:value['employeeId'],
          appraisalDateFrom:value.requireComp['appraisalDateFrom'],
          appraisalDateTo:value.requireComp['appraisalDateTo'],
          createdDate:value.requireComp['createdDate'],
          ACTION:''
        }
        Communicationarray.push(Commdata);
        })
        this.AppraisalData=Communicationarray;
        this.dataSource.data = this.AppraisalData
       
       
        }
        else{
          this.dataSource.data=Communicationarray;
          
         // this.noData = this.dataSource.connect().pipe(map(data => data.length === 0));
        }
    }
      else{
        
      }return this.dataSource.data;
      
    })
  }

 

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getAllEmployeeAppraisalList();
  }

  DeleteModal(appraisalID,_e)
  {

    this.deleteAppraisalId = appraisalID;
  }
  
DeleteAppraisalInfo(event){
    this.AppraisalInfoModel.Id=this.deleteAppraisalId;
    this.empService.DeleteAppraisalDetails(this.AppraisalInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notification.Success({ message: data.message, title: null });
        this.getAllEmployeeAppraisalList();
      }
      else {
        this.notification.Error({ message: data.message, title: null });
      }

    })
  }
  OpenEditmodal(employeeId,appraisalId,_e)
  {

    this.router.navigate(['/employee/appraisal-detail'], { queryParams: { Id: employeeId,EId:appraisalId } });
  }
}

export interface AllEmployeeAppraisalInfo {
  Id?: number;
  employeeId?: number;
  departmentName?: string;
  appraisalType?: string;
  EmployeeName?:string;
  EmployeedetailId?:number;
  appraisalDateFrom?:string;
  appraisalDateTo?:string;
}
