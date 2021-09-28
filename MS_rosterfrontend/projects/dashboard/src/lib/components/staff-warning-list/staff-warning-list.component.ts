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
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';


@Component({
  selector: 'app-staff-warning-list',
  templateUrl: './staff-warning-list.component.html',
  styleUrls: ['./staff-warning-list.component.scss']
})
export class StaffWarningListComponent implements OnInit {

  getErrorMessage:'Please Enter Value';
  staffwarningData:StaffWarningElement[];
  totalCount: number;
  paging: Paging = {};
  staffwarningList=[];
   deleteStaffId : number;
   searchByName = null;
   searchBywarning = null;
  StaffInfoModel: StaffWarningElementInfo = {};
  orderBy: number;
  orderColumn: number;
  displayedColumns: string[] = ['sr', 'employeeName', 'warningType', 'description', 'improvementPlan','offensestype','otheroffenses','createdDate','action'];
  dataSource = new MatTableDataSource(this.staffwarningList);

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('empName') employeeName: ElementRef;
  @ViewChild('warning') warning: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  constructor(private fb: FormBuilder, private empService: EmpServiceService,private router:Router,private notification:NotificationService, private activatedRoute: ActivatedRoute,
    private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.getAllEmployeestaffwarningList();
    //this.dataSource.sort = this.sort;
    
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getAllEmployeestaffwarningList())
        )
        .subscribe();
    }, 2000);

  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'employeeName':
        this.orderColumn = 0;
        break;
      case 'warningType':
        this.orderColumn = 1;
        break;
      case 'description':
        this.orderColumn = 2;
        break;
        case 'improvementPlan':
          this.orderColumn = 3;
          break;
          case 'offensestype':
            this.orderColumn = 4;
            break;
          case 'otheroffenses':
            this.orderColumn = 5;
            break;
           
      case 'createdDate':
        this.orderColumn = 6;
        break;

      default:
        break;
    }
  }
  Search() {
    this.searchByName = this.employeeName.nativeElement.value;
    this.searchBywarning = this.warning.nativeElement.value;
    this.getAllEmployeestaffwarningList();
  }
  getAllEmployeestaffwarningList() {
    this.getSortingOrder();
   const data = {
      searchTextByName: this.searchByName,
      searchBywarning: this.searchBywarning,
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    
      this.empService.getAllEmployeestaffwarningList(data).subscribe((res:any)=>{
        this.totalCount = res.total;
      if(res)
      {
        let staffarray = [];
       
        if(res.responseData!=null)
        {
        this.staffwarningList=res.responseData;
        
        this.staffwarningList.forEach(function(value){
          let Commdata={
          Id:value.requireComp['id'],
          employeeId:value.requireComp['employeeId'],
          employeeName: 
          value['firstName'] +
          ((value['middleName'] ===undefined || value['middleName'] === null)?'':' '+value['middleName']) 
          +
          ((value['lastName'] ===undefined || value['lastName'] === null)?'':' '+value['lastName']) ,
          warningType:value['warning'],
          description:value.requireComp['description'],
          improvementPlan:value.requireComp['improvementPlan'],
           createdDate:value['createdDate'],
           otherOffenses:value.requireComp['otherOffenses'],
           offenseTypeName:value['offenseTypeName'],
          ACTION:''
        }
        staffarray.push(Commdata);
        })
        this.staffwarningData=staffarray;
        this.dataSource.data = this.staffwarningData;
       }
        else{
          this.dataSource.data=staffarray;
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
    this.getAllEmployeestaffwarningList();
  }
  DeleteModal(StaffID,_e)
  {

    this.deleteStaffId = StaffID;
  }
  
DeletestaffInfo(event){
    this.StaffInfoModel.Id=this.deleteStaffId;
    this.empService.DeleteStaffDetails(this.StaffInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notification.Success({ message: data.message, title: null });
        this.getAllEmployeestaffwarningList();
      }
      else {
        this.notification.Error({ message: data.message, title: null });
      }

    })
  }
  OpenEditmodal(employeeId,staffId,_e)
  {

    this.router.navigate(['/employee/staffwarning-detail'], { queryParams: { Id: employeeId,EId:staffId } });
  }


}
export interface StaffWarningElementInfo {
  Id?:number
}
export interface StaffWarningElement {
  Id?:number
  employeeId?:number,
  employeeName: string;
  warningType: number;
  managerName: string;
  JobTitle: string;
  date: string
}
