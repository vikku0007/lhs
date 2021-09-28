import {Component, OnInit, ViewChild, ElementRef} from '@angular/core';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { NotificationService } from 'projects/core/src/projects';
import { FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Paging } from 'projects/viewmodels/paging';
import { ClientService } from '../../services/client.service';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
export interface MedicalHistoryElement {
  clientName: string;
  subject: string;
  raisedBy: string;
  date: string;
}

const MEDICAL_HISTORY_DATA: MedicalHistoryElement[] = [
  {clientName: 'Mario Speedwagon', subject: 'John Doem', raisedBy: 'John Doem', date: '5-Mar-2020'},
  {clientName: 'Petey Cruiser', subject: 'Petey Cruiser', raisedBy: 'Petey Cruiser', date: '6-Jun-2019'},
  {clientName: 'Anna Sthesia', subject: 'Anna Sthesia', raisedBy: 'Anna Sthesia', date: '11-Nov-2019'},
  {clientName: 'Paul Molive', subject: 'Paul Molive', raisedBy: 'Paul Molive', date: '3-Apr-2020'},
  {clientName: 'Gail Forcewind', subject: 'Gail Forcewind', raisedBy: 'Gail Forcewind', date: '25-Jun-2019'},
];
@Component({
  selector: 'lib-medical-history-list',
  templateUrl: './medical-history-list.component.html',
  styleUrls: ['./medical-history-list.component.scss']
})

export class MedicalHistoryListComponent implements OnInit {
  getErrorMessage:'Please Enter Value';
  MedicalhistoryData:AllClientmedicalhistory[];
  MedicalInfoModel: AllClientmedicalhistory = {};
  paging: Paging = {};
  MedicalList=[];
  totalCount: number;
  deleteMedicalId : number;
  searchByName = null;
  searchByGender = null;
  orderBy: number;
  orderColumn: number;
  displayedColumns: string[] = ['sr','name', 'gender', 'mobileNo','createdDate' ,'action'];
  dataSource = new MatTableDataSource(this.MedicalList);

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('clientName') clientName: ElementRef;
  @ViewChild('gender') gender: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  constructor(private fb: FormBuilder,private clientservice:ClientService,private router:Router,private notification:NotificationService, private activatedRoute: ActivatedRoute,
    private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.GetAllClientMedicalHistory();
    this.dataSource.sort = this.sort;
    
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.GetAllClientMedicalHistory())
        )
        .subscribe();
    }, 2000);

  }
  Search() {
    this.searchByName = this.clientName.nativeElement.value;
    this.searchByGender = "";
    this.GetAllClientMedicalHistory();
  }
  OpenEditmodal(ClientId,MedicalId,_e)
  {

    this.router.navigate(['/client/medical-history-details'], { queryParams: { Id: ClientId,EId:MedicalId } });
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'name':
        this.orderColumn = 0;
        break;
      case 'gender':
        this.orderColumn = 1;
        break;
      case 'mobileNo':
        this.orderColumn = 2;
        break;
         case 'createdDate':
        this.orderColumn = 3;
        break;

      default:
        break;
    }
  }
  GetAllClientMedicalHistory() {
    this.getSortingOrder();
    const data = {
      searchTextByName: this.searchByName,
      SearchTextByGender: this.searchByGender,
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    
      this.clientservice.GetAllClientMedicalHistory(data).subscribe((res:any)=>{
        this.totalCount = res.total;
      if(res)
      {
        let Medicalarray = [];
       
        if(res.responseData!=null)
        {
        this.MedicalList=res.responseData;
        
        this.MedicalList.forEach(function(value){
          let Commdata={
          Id:value.requireComp['id'],
          clientId:value.requireComp['clientId'],
          name: 
          value['firstName'] +
          ((value['middleName'] ===undefined || value['middleName'] === null)?'':' '+value['middleName']) 
          +
          ((value['lastName'] ===undefined || value['lastName'] === null)?'':' '+value['lastName']) ,
          gender:value['genderName'],
          mobileNo:value.requireComp['mobileNo'],
          checkCondition:value.requireComp['checkCondition'],
          checkSymptoms:value.requireComp['checkSymptoms'],
          createdDate:value['createdDate'],
          ACTION:''
        }
        Medicalarray.push(Commdata);
        })
        this.MedicalhistoryData=Medicalarray;
        this.dataSource.data = this.MedicalhistoryData
      
        }
        else{
          this.dataSource.data=Medicalarray;
         
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
    this.GetAllClientMedicalHistory();
  }
  DeleteModal(medicalID,_e)
  {

    this.deleteMedicalId = medicalID;
  }
  DeleteMedicalhistoryInfo(event){
    this.MedicalInfoModel.Id=this.deleteMedicalId;
    this.clientservice.deleteClientMedicalHistory(this.MedicalInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notification.Success({ message: data.message, title: null });
        this.GetAllClientMedicalHistory();
      }
      else {
        this.notification.Error({ message: data.message, title: null });
      }

    })
  }

}

export interface AllClientmedicalhistory {
  Id?: number;
  clientId?: number;
  name?: string;
  mobileNo?: string;
  gender?: string;
  checkCondition?: Date;
  checkSymptoms?: string;
  isTakingMedication?: string;
  isMedicationAllergy?:string;
  isTakingTobacco?:string;
  isTakingIllegalDrug?:string;
  iakingAlcohol?:string;
}

