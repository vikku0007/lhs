import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { CommonService } from 'projects/lhs-service/src/projects';
import * as moment from 'moment';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MasterDetails } from '../../../viewmodel/master-details';
import { AdminService } from '../../../admin.service';
import { PublicHoliday } from '../../../viewmodel/public-holiday';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
@Component({
  selector: 'app-public-holiday',
  templateUrl: './public-holiday.component.html',
  styleUrls: ['./public-holiday.component.scss'],
  providers: [
    {
        provide: DateAdapter, useClass: AppDateAdapter
    },
    {
        provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
    ]
})

export class PublicHolidayComponent implements OnInit {
  masterInfo: PublicHoliday[];
  masterModel: PublicHoliday = {};
  rForm: FormGroup;
  rForm1: FormGroup;
  totalCount: number;
  paging: Paging = {};
  getErrorMessage: 'Please Enter Value';
  employeeId: any;
  response: ResponseModel = {};
  leaveId: any;
  responseModel: ResponseModel = {};
  displayedColumnsLeave: string[] = ['Year','Holiday','DateFrom','DateTo','action'];
  dataSourceLeave: any;
  employeePrimaryInfo: {};
  Masterentrylist = [];
  orderBy: number;
  orderColumn: number;
  @ViewChild('btnAddEducationCancel') cancel: ElementRef;
  @ViewChild('btnEditEducationCancel') editCancel: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  EditId: number;
  todayDate = new Date();
  masterId: number;
  deletemasterId: any;
  Typelist: [];
  selectedType: any;
  codeData: any[];
  list: any;
  codeId: any;
  searchByCodeData: any;
  searchByCodeDescription: any;
  //newDate = moment(this.todayDate).subtract(1, 'day')
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSourceLeave.filter = filterValue.trim().toLowerCase();
  }

  delete(elm) {
    this.dataSourceLeave.data = this.dataSourceLeave.data.filter(i => i !== elm)
  }
  @ViewChild('codeData') searchcodedata: ElementRef;
  @ViewChild('codeDescription') searchcodedescription: ElementRef;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('formDirective') private formDirective: NgForm;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private loaderService: LoaderService,private commonService:CommonService, private adminService: AdminService, private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
   
    this.createForm();
    this.createEditForm(null);
    this.getYear();
    this.GetList();
    
     
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceLeave !== undefined ? this.dataSourceLeave.sort = this.sort : this.dataSourceLeave;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.GetList())
        )
        .subscribe();
    }, 2000);

  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
     
      case 'Year':
        this.orderColumn = 0;
        break;
      case 'Holiday':
        this.orderColumn = 1;
        break;
      case 'DateFrom':
          this.orderColumn = 2;
          break;
      case 'DateTo':
            this.orderColumn = 3;
            break;
       case 'createdDate':
            this.orderColumn = 4;
            break;
      default:
        break;
    }
  }
 
  getYear(){
    this.commonService.getYear().subscribe((res=>{
      if(res){
        this.responseModel = res;
        debugger;
      this.Typelist=this.responseModel.responseData||[];
       
      }else{

      }
    }));
  }
  createForm() {
    this.rForm1 = this.fb.group({
      AddYear: ['', Validators.required],
      AddHoliday: ['', Validators.required],
      AddDateFrom: ['', Validators.required],
      AddDateTo: ['', Validators.required],
     
    });
  }
  
  
  openModal(open: boolean) {
    document.getElementById("openModalButtonMaster").click();
  }
  createEditForm(index) {
     if (index != null) {
      this.rForm = this.fb.group({
        YearEdit: [this.masterInfo[index].year, Validators.required],
        HolidayEdit: [this.masterInfo[index].holiday, Validators.required],
        DateFromEdit: [this.masterInfo[index].dateFrom, Validators.required],
        DateToEdit: [this.masterInfo[index].dateTo, Validators.required],
        
      });
    }
    else {
      this.rForm = this.fb.group({
        YearEdit: ['', Validators.required],
        HolidayEdit: ['', Validators.required],
        DateFromEdit: ['', Validators.required],
        DateToEdit: ['', Validators.required],
        

      });
    }
  }
  openEditModal(elem) {
    this.list=this.Typelist;
   document.getElementById("openEditModalButton").click();
   const index = this.masterInfo.findIndex(x => x.id == elem.id);
   this.createEditForm(index);
   this.masterId = this.masterInfo[index].id;
  
 }
 
  Search() {
    this.searchByCodeDescription = this.searchcodedescription.nativeElement.value;
    this.paging.pageNo = 0;
    this.GetList();
  }
  GetList() {
    this.getSortingOrder();
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      searchTextByCodeDescription: this.searchByCodeDescription,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
   
    this.adminService.GetAllPublicHoliday(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      if (this.response.responseData) {
        this.masterInfo = this.response.responseData;
        this.dataSourceLeave = new MatTableDataSource(this.masterInfo);
         }
      else {
        this.dataSourceLeave = new MatTableDataSource(this.Masterentrylist);
      }
    });
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetList();
  }
  selectChangeHandler(event:any) {
    this.list=this.Typelist;
    this.selectedType = event;
    const index = this.list.findIndex(x => x.id == this.selectedType);
    this.codeData= this.list[index].codeData;
    //this.codeData=(this.list.map(x=>x.codeData));
  }
  addMasterInfo() {
   if (this.rForm1.valid) {
      const data = {
        'Year': this.rForm1.get('AddYear').value,
        'Holiday': this.rForm1.get('AddHoliday').value,
        'DateFrom': moment(this.rForm1.get('AddDateFrom').value).format('YYYY-MM-DD'),
        'DateTo': moment(this.rForm1.get('AddDateTo').value).format('YYYY-MM-DD')
      };

      this.adminService.AddPublicHoliday(data).subscribe(res => {
        this.response = res;        
        switch (this.response.status) {
          case 1:
            this.masterModel = this.response.responseData;
            if (!this.masterInfo) {
              this.masterInfo = [];
            }
           this.GetList();
            this.cancel.nativeElement.click();
           this.notificationService.Success({ message: this.response.message, title: null });
           this.rForm1.reset();
           this.formDirective.resetForm();
           this.GetList();
            break;

          default:
            this.notificationService.Warning({ message: this.response.message, title: null });
            break;
        }
      });
    }

  }
  updateMasterEntries() { 
    if (this.rForm.valid) {
      const data = {
        'ID': this.masterId,
        'Year': this.rForm.get('YearEdit').value,
        'Holiday': this.rForm.get('HolidayEdit').value,
        'DateFrom': moment(this.rForm.get('DateFromEdit').value).format('YYYY-MM-DD'),
        'DateTo': moment(this.rForm.get('DateToEdit').value).format('YYYY-MM-DD')
        
      };
      this.adminService.UpdatePublicHoliday(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.editCancel.nativeElement.click();
            this.GetList();
            this.notificationService.Success({ message: this.response.message, title: null });
            break;
           default:
            this.notificationService.Warning({ message: this.response.message, title: null });
            break;
        }
      });
    }
  }
 
  DeleteModal(masterID, _e) {

    this.deletemasterId = masterID;
  }

  DeleteHolidayInfo(event) {
    this.masterModel.id = this.deletemasterId;
    this.adminService.DeletePublicHoliday(this.masterModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.GetList();
      }
      else {

        this.notificationService.Success({ message: 'Some error occured', title: null });
      }

    })
  }
 
}
