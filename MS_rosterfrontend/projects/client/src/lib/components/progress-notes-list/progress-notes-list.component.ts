import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Paging } from 'projects/viewmodels/paging';
import { FormBuilder } from '@angular/forms';
import { ClientService } from '../../services/client.service';
import { NotificationService } from 'projects/core/src/projects';
import { Router, ActivatedRoute } from '@angular/router';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
export interface ProgressNotesElement {
  clientName: string;
  mrn: string;
  raisedBy: string;
  date: string;
}

const PROGRESS_NOTES_DATA: ProgressNotesElement[] = [
  { clientName: 'Mario Speedwagon', mrn: '456454', raisedBy: 'John Doem', date: '5-Mar-2020' },
  { clientName: 'Petey Cruiser', mrn: '789756', raisedBy: 'Petey Cruiser', date: '6-Jun-2019' },
  { clientName: 'Anna Sthesia', mrn: '123545', raisedBy: 'Anna Sthesia', date: '11-Nov-2019' },
  { clientName: 'Paul Molive', mrn: '874565', raisedBy: 'Paul Molive', date: '3-Apr-2020' },
  { clientName: 'Gail Forcewind', mrn: '894564', raisedBy: 'Gail Forcewind', date: '25-Jun-2019' },
];
@Component({
  selector: 'lib-progress-notes-list',
  templateUrl: './progress-notes-list.component.html',
  styleUrls: ['./progress-notes-list.component.scss']
})

export class ProgressNotesListComponent implements OnInit {
  getErrorMessage: 'Please Enter Value';
  ProgressNotesData: AllClientProgressNotes[];
  ProgressNotesInfoModel: AllClientProgressNotes = {};
  paging: Paging = {};
  MedicalList = [];
  totalCount: number;
  deleteProgressId: number;
  searchByName = null;
  searchByMRN = null;
  orderBy: number;
  orderColumn: number;
  displayedColumns: string[] = ['sr', 'name',  'date','createdDate' ,'action'];
  dataSource = new MatTableDataSource(this.MedicalList);

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('clientName') clientName: ElementRef;
  @ViewChild('Mrn') MRNNo: ElementRef; 
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  constructor(private fb: FormBuilder, private clientservice: ClientService, private router: Router, private notification: NotificationService, private activatedRoute: ActivatedRoute,
    private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.GetAllClientProgressNotes();
    this.dataSource.sort = this.sort;

  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.GetAllClientProgressNotes())
        )
        .subscribe();
    }, 2000);

  }
  Search() {
    this.searchByName = this.clientName.nativeElement.value;
    this.searchByMRN = "";
    this.GetAllClientProgressNotes();
  }
  OpenEditmodal(clientId,progressId, _e) {

    this.router.navigate(['/client/progress-notes-details'], { queryParams: { Id: clientId,EId:progressId } });
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'name':
        this.orderColumn = 0;
        break;
      case 'progressNote':
        this.orderColumn = 1;
        break;
      case 'date':
        this.orderColumn = 2;
        break;
         case 'createdDate':
        this.orderColumn = 3;
        break;

      default:
        break;
    }
  }
  GetAllClientProgressNotes() {
    this.getSortingOrder();
    const data = {
      searchTextByName: this.searchByName,
      SearchTextByMRN: this.searchByMRN,
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };

    this.clientservice.GetAllClientProgressNotes(data).subscribe((res: any) => {
      this.totalCount = res.total;
      if (res) {
        let Medicalarray = [];

        if (res.responseData != null) {
          this.MedicalList = res.responseData;

          this.MedicalList.forEach(function (value) {
            let Commdata = {
              Id: value.requireComp['id'],
              clientId: value.requireComp['clientId'],
              name:
                value['firstName'] +
                ((value['middleName'] === undefined || value['middleName'] === null) ? '' : ' ' + value['middleName'])
                +
                ((value['lastName'] === undefined || value['lastName'] === null) ? '' : ' ' + value['lastName']),
              date: value.requireComp['date'],
              progressNote: value.requireComp['progressNote'],
              createdDate:value['createdDate'],
              ACTION: ''
            }
            Medicalarray.push(Commdata);
          })
          this.ProgressNotesData = Medicalarray;
          this.dataSource.data = this.ProgressNotesData

        }
        else {
          this.dataSource.data = Medicalarray;

          // this.noData = this.dataSource.connect().pipe(map(data => data.length === 0));
        }
      }
      else {

      } return this.dataSource.data;

    })
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetAllClientProgressNotes();
  }
  DeleteModal(progressID, _e) {

    this.deleteProgressId = progressID;
  }
  DeleteProgressNotes(event) {
    this.ProgressNotesInfoModel.Id = this.deleteProgressId;
    this.clientservice.DeleteClientProgressNotes(this.ProgressNotesInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notification.Success({ message: data.message, title: null });
        this.GetAllClientProgressNotes();
      }
      else {
        this.notification.Error({ message: data.message, title: null });
      }

    })
  }

}

export interface AllClientProgressNotes {
  Id?: number;
  clientId?: number;
  name?: string;
  patientName?: string;
  date?: string;
  progressNote?: Date;
  
}

