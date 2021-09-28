import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { map } from 'rxjs/operators';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { EmpServiceService } from 'projects/dashboard/src/lib/services/emp-service.service';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { ClientService } from '../../services/client.service';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
export interface AccidentsIncidentsElement {
  name: string;
  eventType: string;
  location: string;
  raisedBy: string;
  reportedBy: string;
  date: string;
}

const ACCIDENTS_INCIDENTS_DATA: AccidentsIncidentsElement[] = [
  {name: 'Mario Speedwagon', eventType: 'Accident', location: 'Sydney', raisedBy: 'John Doem', reportedBy: 'John Doem', date: '5-Mar-2020'},
  {name: 'Petey Cruiser', eventType: 'Incident', location: 'Sydney', raisedBy: 'Petey Cruiser', reportedBy: 'Petey Cruiser', date: '6-Jun-2019'},
  {name: 'Anna Sthesia', eventType: 'Accident', location: 'Sydney', raisedBy: 'Anna Sthesia', reportedBy: 'Anna Sthesia', date: '11-Nov-2019'},
  {name: 'Paul Molive', eventType: 'Mishandling', location: 'Sydney', raisedBy: 'Paul Molive', reportedBy: 'Paul Molive', date: '3-Apr-2020'},
  {name: 'Gail Forcewind', eventType: 'Accident', location: 'Sydney', raisedBy: 'Gail Forcewind', reportedBy: 'Gail Forcewind', date: '25-Jun-2019'},
];

@Component({
  selector: 'lib-accident-incident-list',
  templateUrl: './accident-incident-list.component.html',
  styleUrls: ['./accident-incident-list.component.scss']
})
export class AccidentIncidentListComponent implements OnInit {

  getErrorMessage:'Please Enter Value';
  AccidentData:AllEmployeeAccidentInfo[];
  AccidentInfoModel: AllEmployeeAccidentInfo = {};
  paging: Paging = {};
  AccidentList=[];
  totalCount: number;
  deleteAccidentId : number;
  searchByName = null;
  searchBylocation = null;
  orderBy: number;
  orderColumn: number;
  displayedColumns: string[] = ['sr','clientName','reportedByName','departmentName' ,'eventType','locationtype', 'locationName',  'accidentDate','createdDate','action'];
  dataSource = new MatTableDataSource(this.AccidentList);

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('empName') employeeName: ElementRef;
  @ViewChild('location') location: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  constructor(private fb: FormBuilder,private loaderService: LoaderService, private clientService: ClientService,private router:Router,private notification:NotificationService, private activatedRoute: ActivatedRoute,
    private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.getAllClientAccidentList();
    this.dataSource.sort = this.sort;
    
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getAllClientAccidentList())
        )
        .subscribe();
    }, 2000);

  }
  Search() {
    this.searchByName = this.employeeName.nativeElement.value;
    this.searchBylocation = this.location.nativeElement.value;
    this.getAllClientAccidentList();
  }
  OpenAccidentEditmodal(CommunicationId,AccidentId,_e)
  {

    this.router.navigate(['/client/accident-details'], { queryParams: { Id: CommunicationId,EId:AccidentId } });
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'clientName':
        this.orderColumn = 0;
        break;
      case 'reportedByName':
        this.orderColumn = 1;
        break;
      case 'departmentName':
        this.orderColumn = 2;
        break;
        case 'eventType':
          this.orderColumn = 3;
          break; 
          case 'locationName':
          this.orderColumn = 4;
          break;
          case 'accidentDate':
            this.orderColumn = 5;
            break;
      case 'createdDate':
        this.orderColumn = 6;
        break;

      default:
        break;
    }
  }
  getAllClientAccidentList() {
    this.getSortingOrder();
    const data = {
      searchTextByName: this.searchByName,
      SearchTextBylocation: this.searchBylocation,
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    
      this.clientService.GetAllClientAccidentList(data).subscribe((res:any)=>{
        this.totalCount = res.total;
      if(res)
      {
        let Communicationarray = [];
       
        if(res.responseData!=null)
        {
        this.AccidentList=res.responseData;
        
        this.AccidentList.forEach(function(value){
          let Commdata={
          Id:value['id'],
          clientId:value['clientId'],
          employeeId:value['employeeId'],
          clientName: 
          value['fullName'] ,
          eventType:value['eventTypeName'],
          locationName:value['locationName'],
          employeeName:value['employeeName'],
          accidentDate:value['accidentDate'],
          reportedByName:value['reportedByName'],
          departmentName:value['departmentName'],
          createdDate:value['createdDate'],
          locationType:value['locationTypeName'],
          otherLocation:value['otherLocation'],
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
    this.getAllClientAccidentList();
  }
  DeleteModal(accidentID,_e)
  {

    this.deleteAccidentId = accidentID;
  }
  DeleteAccidentInfo(event){
    this.AccidentInfoModel.Id=this.deleteAccidentId;
    this.clientService.DeleteClientAccidentIncident(this.AccidentInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notification.Success({ message: data.message, title: null });
        this.getAllClientAccidentList();
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