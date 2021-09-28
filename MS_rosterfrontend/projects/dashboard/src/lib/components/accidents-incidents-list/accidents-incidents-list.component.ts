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


@Component({
  selector: 'app-accidents-incidents-list',
  templateUrl: './accidents-incidents-list.component.html',
  styleUrls: ['./accidents-incidents-list.component.scss']
})
export class AccidentsIncidentsListComponent implements OnInit {

  getErrorMessage:'Please Enter Value';
  AccidentData:AllEmployeeAccidentInfo[];
  AccidentInfoModel: AllEmployeeAccidentInfo = {};
  paging: Paging = {};
  AccidentList=[];
  totalCount: number;
  deleteAccidentId : number;
  searchByName = null;
  searchBylocation = null;
  displayedColumns: string[] = ['sr','employeeName', 'eventType', 'accidentDate','locationtype', 'location','reportedTo','actionTaken','createdDate' ,'action'];
  dataSource = new MatTableDataSource(this.AccidentList);

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
 @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('empName') employeeName: ElementRef;
  @ViewChild('location') location: ElementRef;
  orderBy: number;
  orderColumn: number;
  constructor(private fb: FormBuilder, private empService: EmpServiceService,private router:Router,private notification:NotificationService, private activatedRoute: ActivatedRoute,
    private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.getAllEmployeeAccidentList();
    this.dataSource.sort = this.sort;
    
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getAllEmployeeAccidentList())
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
      case 'eventType':
        this.orderColumn = 1;
        break;
      case 'accidentDate':
        this.orderColumn = 2;
        break;
        case 'locationtype':
          this.orderColumn = 3;
          break;
        case 'location':
          this.orderColumn = 4;
          break;
          case 'reportedTo':
          this.orderColumn = 5;
          break;
          case 'actionTaken':
            this.orderColumn = 6;
            break;
      case 'createdDate':
        this.orderColumn = 7;
        break;

      default:
        break;
    }
  }

  Search() {
    this.searchByName = this.employeeName.nativeElement.value;
    this.searchBylocation = "";
    this.getAllEmployeeAccidentList();
  }
  OpenEditmodal(empId,accidentId,_e)
  {

    this.router.navigate(['/employee/accidents-incidents-details'], { queryParams: { Id: empId,EId:accidentId } });
  }
  getAllEmployeeAccidentList() {
    this.getSortingOrder();
    const data = {
      searchTextByName: this.searchByName,
      SearchTextBylocation: this.searchBylocation,
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy

    };
    
      this.empService.getAllEmployeeAccident(data).subscribe((res:any)=>{
        this.totalCount = res.total;
      if(res)
      {
        let Communicationarray = [];
       
        if(res.responseData!=null)
        {
        this.AccidentList=res.responseData;
        
        this.AccidentList.forEach(function(value){
          let Commdata={
          Id:value.requireComp['id'],
          employeeId:value.requireComp['employeeId'],
          employeeName: 
          value['firstName'] +
          ((value['middleName'] ===undefined || value['middleName'] === null)?'':' '+value['middleName']) 
          +
          ((value['lastName'] ===undefined || value['lastName'] === null)?'':' '+value['lastName']) ,
          eventType:value['eventTypeName'],
          location:value['locationName'],
          reportedTo:value['reportedToName'],
          accidentDate:value.requireComp['accidentDate'],
          briefDescription:value.requireComp['briefDescription'],
          detailedDescription:value.requireComp['detailedDescription'],
          createdDate:value['createdDate'],
          locationType:value['locationTypeName'],
          otherLocation:value['otherLocation'],
          actionTaken:value['actionTaken'],
          ACTION:''
        }
        Communicationarray.push(Commdata);
        })
        this.AccidentData=Communicationarray;
        this.dataSource.data = this.AccidentData
      
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
    this.getAllEmployeeAccidentList();
  }
  DeleteModal(accidentID,_e)
  {

    this.deleteAccidentId = accidentID;
  }
  DeleteAccidentInfo(event){
    this.AccidentInfoModel.Id=this.deleteAccidentId;
    this.empService.DeleteAccidentDetails(this.AccidentInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notification.Success({ message: data.message, title: null });
        this.getAllEmployeeAccidentList();
      }
      else {
        this.notification.Error({ message: data.message, title: null });
      }

    })
  }

}
export interface AllEmployeeAccidentInfo {
  Id?: number;
  employeeId?: number;
  eventType?: string;
  location?: string;
  reportedTo?: string;
  accidentDate?: Date;
  briefDescription?: string;
  detailedDescription?: string;
  EmployeeName?:string
}
