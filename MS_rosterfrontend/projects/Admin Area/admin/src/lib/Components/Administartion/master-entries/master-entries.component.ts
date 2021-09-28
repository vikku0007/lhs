import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder, Validators, NgForm, FormControl } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { CommonService } from 'projects/lhs-service/src/projects';
import * as moment from 'moment';
import { merge, Subject, ReplaySubject } from 'rxjs';
import { tap, takeUntil } from 'rxjs/operators';
import { MasterDetails } from '../../../viewmodel/master-details';
import { AdminService } from '../../../admin.service';
import { MatSelect } from '@angular/material/select';

@Component({
  selector: 'app-master-entries',
  templateUrl: './master-entries.component.html',
  styleUrls: ['./master-entries.component.scss']
})

export class MasterEntriesComponent implements OnInit {
  masterInfo: MasterDetails[];
  masterModel: MasterDetails = {};
  rForm: FormGroup;
  rForm1: FormGroup;
  totalCount: number;
  paging: Paging = {};
  getErrorMessage: 'Please Enter Value';
  employeeId: any;
  response: ResponseModel = {};
  leaveId: any;
  responseModel: ResponseModel = {};
  displayedColumnsLeave: string[] = ['codeData','codeDescription','createdDate','status','action'];
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
  codeTypelist: any;
  selectedType: any;
  codeData: any[];
  list: any;
  codeId: any;
  searchByCodeData: any;
  searchByCodeDescription: any;
  CodedataID: any;
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild('Select') select: MatSelect;

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
    this.getCodeType();
    this.GetList();
    this.searchcodetype();
    this.searchEditcodetype();
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
     
      case 'codeData':
        this.orderColumn = 0;
        break;
      case 'codeDescription':
        this.orderColumn = 1;
        break;
       case 'createdDate':
            this.orderColumn = 3;
            break;
      default:
        break;
    }
  }
 
  getCodeType(){
    this.commonService.getCodeType().subscribe((res=>{
      if(res){
        this.responseModel = res;
      this.Typelist=this.responseModel.responseData||[];
      this.codeTypelist=this.responseModel.responseData||[];
      this.filteredRecords.next(this.Typelist.slice());

      }else{

      }
    }));
  }
  searchcodetype(){
    this.control.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filtercodetype();
    });
  }

 private filtercodetype() {
    if (!this.codeTypelist) {
      return;
    }
    let search = this.control.value;
    if (!search) {
      this.filteredRecords.next(this.codeTypelist.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecords.next(
      this.codeTypelist.filter(department => department.codeData.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  searchEditcodetype(){
    this.searchcontrol.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterEditcodetype();
    });
  }

 private filterEditcodetype() {
    if (!this.codeTypelist) {
      return;
    }
    let search = this.searchcontrol.value;
    if (!search) {
      this.filteredRecords.next(this.codeTypelist.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecords.next(
      this.codeTypelist.filter(department => department.codeData.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  createForm() {
    this.rForm1 = this.fb.group({
      AddType: ['', Validators.required],
      AddDescription: ['', Validators.required],
     
    });
  }
  
  
  openModal(open: boolean) {
    document.getElementById("openModalButtonMaster").click();
  }
  createEditForm(index) {
     if (index != null) {
      this.rForm = this.fb.group({
       TypeEdit: [this.codeId, Validators.required],
        DescriptionEdit: [this.masterInfo[index].codeDescription, Validators.required],
        
      });
    }
    else {
      this.rForm = this.fb.group({
        TypeEdit: ['', Validators.required],
        DescriptionEdit: ['', Validators.required],
        

      });
    }
  }
  openEditModal(elem) {
    this.list=this.Typelist;
   document.getElementById("openEditModalButton").click();
   const index1 = this.list.findIndex(x => x.codeData == elem.codeData);
   this.codeId= this.list[index1].id;
   this.codeData=this.list[index1].codeData;
   const index = this.masterInfo.findIndex(x => x.id == elem.id);
   this.createEditForm(index);
   this.masterId = this.masterInfo[index].id;
   this.CodedataID=elem.codeDataId
  
 }
 
  Search() {
    this.searchByCodeData = this.searchcodedata.nativeElement.value;
    this.searchByCodeDescription = this.searchcodedescription.nativeElement.value;
    this.paging.pageNo = 0;
    this.GetList();
  }
  GetList() {
    this.getSortingOrder();
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      searchTextByCodeData: this.searchByCodeData,
      searchTextByCodeDescription: this.searchByCodeDescription,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.adminService.getAllMasterEntriesList(data).subscribe(res => {
      console.log("res accident", res);
      this.response = res;
      this.totalCount = this.response.total;
      if (this.response.responseData) {
        debugger;
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
        'CodeData': this.codeData,
        'CodeDescription': this.rForm1.get('AddDescription').value,
        'CodeDataId':this.selectedType
      };

      this.adminService.AddMasterentries(data).subscribe(res => {
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
        'CodeData': this.codeData,
        'CodeDescription': this.rForm.get('DescriptionEdit').value,
        'CodeDataId':this.selectedType
      };
      this.adminService.UpdateMasterentries(data).subscribe(res => {
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

  // DeleteMasterInfo(event) {
  //   this.masterModel.id = this.deletemasterId;
  //   this.adminService.DeleteMasterEntries(this.masterModel).subscribe((data: any) => {
  //     if (data.status == 1) {
  //       this.notificationService.Success({ message: data.message, title: null });
  //       this.GetList();
  //     }
  //     else {

  //       this.notificationService.Success({ message: 'Some error occured', title: null });
  //     }

  //   })
  // }
  UpdateStatus(elem){
    const data = {
     id: elem.id,
     Status:elem.isActive==true?false:true
   }
   this.adminService.UpdateMasterEntriesStatus(data).subscribe(res => {
     this.responseModel = res;
     switch (this.responseModel.status) {
       case 1:
         //this.dataSource.data = this.dataSource.data.filter(i => i !== elm)
         this.notificationService.Success({ message: 'Status Updated successfully', title: '' })
         this.GetList();
         break;
       case 0:
         this.notificationService.Warning({ message: this.responseModel.message, title: '' });
       default:
         break;
     }
   });
 }
}
export interface LeaveElementInfo {
  Id?: number
}
