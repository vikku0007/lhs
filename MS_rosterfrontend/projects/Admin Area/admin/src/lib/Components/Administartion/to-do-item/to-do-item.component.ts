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
import { TodoDetails } from '../../../viewmodel/to-do-item';
@Component({
  selector: 'app-to-do-item',
  templateUrl: './to-do-item.component.html',
  styleUrls: ['./to-do-item.component.scss']
})

export class ToDoItemComponent implements OnInit {
  masterInfo: TodoDetails[];
  masterModel: TodoDetails = {};
  rForm: FormGroup;
  rForm1: FormGroup;
  totalCount: number;
  paging: Paging = {};
  getErrorMessage: 'Please Enter Value';
  response: ResponseModel = {};
  responseModel: ResponseModel = {};
  displayedColumnsToDo: string[] = ['shifttype','description','action'];
  dataSourceTodo: any;
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
  list: any;
  searchByCodeDescription: any;
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
    this.getShiftType();
    this.GetList();
    }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSourceTodo !== undefined ? this.dataSourceTodo.sort = this.sort : this.dataSourceTodo;
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
      case 'shifttype':
        this.orderColumn = 0;
        break;
      case 'description':
        this.orderColumn = 1;
        break;
      case 'createdDate':
            this.orderColumn = 2;
            break;
      default:
        break;
    }
  }
 
  getShiftType(){
    this.commonService.GetShiftTime().subscribe((res=>{
      if(res){
      this.responseModel = res;
      this.Typelist=this.responseModel.responseData||[];
     }
    }));
  }
  createForm() {
    this.rForm1 = this.fb.group({
      AddShifttype: ['', Validators.required],
      AddDescription: ['', Validators.required]
    });
  }
  
  openModal(open: boolean) {
    document.getElementById("openModalButtonMaster").click();
  }
  createEditForm(index) {
     if (index != null) {
      this.rForm = this.fb.group({
        ShifttypeEdit: [this.masterInfo[index].shiftType, Validators.required],
        DescriptionEdit: [this.masterInfo[index].description, Validators.required]
      });
    }
    else {
      this.rForm = this.fb.group({
        ShifttypeEdit: ['', Validators.required],
        DescriptionEdit: ['', Validators.required]
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
     this.adminService.GetShiftItemList(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      if (this.response.responseData) {
        this.masterInfo = this.response.responseData;
        this.dataSourceTodo = new MatTableDataSource(this.masterInfo);
         }
      else {
        this.dataSourceTodo = new MatTableDataSource(this.Masterentrylist);
      }
    });
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetList();
  }
  
  addToDoInfo() {
   if (this.rForm1.valid) {
      const data = {
        'ShiftType': this.rForm1.get('AddShifttype').value,
        'Description': this.rForm1.get('AddDescription').value
     };
        this.adminService.AddToDolistItem(data).subscribe(res => {
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
  updateTodoEntries() { 
    if (this.rForm.valid) {
      const data = {
        'ID': this.masterId,
        'ShiftType': this.rForm.get('ShifttypeEdit').value,
        'Description': this.rForm.get('DescriptionEdit').value
      };
      this.adminService.UpdateToDolistItem(data).subscribe(res => {
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

  DeleteToDoInfo(event) {
    this.masterModel.id = this.deletemasterId;
    this.adminService.DeleteToDolistItem(this.masterModel).subscribe((data: any) => {
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
