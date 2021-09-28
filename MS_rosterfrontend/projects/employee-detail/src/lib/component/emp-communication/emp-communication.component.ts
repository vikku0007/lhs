import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, Validators, FormBuilder, NgForm, FormControl } from '@angular/forms';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { MatOption } from '@angular/material/core';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { merge, ReplaySubject, Subject } from 'rxjs';
import { tap, takeUntil } from 'rxjs/operators';
import { EmployeeCommunicationInfo } from '../../view-models/employee-communication-info';
import { CommunicationRecepientmodel } from 'projects/dashboard/src/lib/viewmodel/employee-communication-info';
import { EmpDetailService } from '../../services/emp-detail.service';
import { MatSelect } from '@angular/material/select';

@Component({
  selector: 'lib-emp-communication',
  templateUrl: './emp-communication.component.html',
  styleUrls: ['./emp-communication.component.scss']
})
export class EmpCommunicationComponent implements OnInit {
  rForm: FormGroup;
  rForm1: FormGroup;
  response: ResponseModel = {};
  getErrorMessage: 'Please Enter Value';
  communicationList: EmployeeCommunicationInfo[] = [];
  EditList: EmployeeCommunicationInfo[] = [];
  communicationModel: EmployeeCommunicationInfo = {};
  communicationdata: EmployeeCommunicationInfo[];
  dateFormat = 'dd/MM/yyyy';
   responseModel: ResponseModel = {};
   deleteCommunicationId : number;
   CommunicationInfoModel: AllEmployeeCommunication = {};
   textName:string;
   EditCommunicationId : number;
   @ViewChild('formDirective') private formDirective: NgForm;
   @ViewChild('allSelected') private allSelected: MatOption;
 
  paging: Paging = {};
  //For Discussion
  displayedColumnsDiscussion: string[] = ['sr',  'assignedToName','subject','createdDate', 'message','action'];
  dataSourceDiscussion = new MatTableDataSource(this.communicationList);
  employeeId = 0;
  employeePrimaryInfo: {}
  ReportedToList: any;
  EditId: any;
  commlist:CommunicationRecepientmodel[];
  SubjectShow: any;
  AssignedToShow: any;
  MessageShow: any;
  selecttextName: string;
  totalCount: number;
  orderBy: number;
  orderColumn: number;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSourceDiscussion.filter = filterValue.trim().toLowerCase();
  }
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild('Select') select: MatSelect;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('btnEditcommunicationCancel') editCancel: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  constructor(private fb: FormBuilder, private empService: EmpDetailService,private loaderService: LoaderService, private activatedRoute: ActivatedRoute,
    private notificationService: NotificationService,private commonservice:CommonService,private router: Router) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe((params: any) => {
      this.employeeId = Number(params.params.id);
      this.selecttextName="Select All";
    });
    this.getCommunicationList();
    this.dataSourceDiscussion.sort = this.sort;
    this.createForm();
    this.getReportedTo();
    this.searchreportedtype();
}
ngAfterViewInit(): void {
  setTimeout(() => {
    this.dataSourceDiscussion !== undefined ? this.dataSourceDiscussion.sort = this.sort : this.dataSourceDiscussion;
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap(() => this.getCommunicationList())
      )
      .subscribe();
  }, 2000);

}

  createForm() {
    this.rForm = this.fb.group({
      subject: ['', Validators.required],
      assignedTo: ['', Validators.required],
      message: ['', Validators.required]
    });
  }
  searchreportedtype(){
    this.control.valueChanges
    .pipe(takeUntil(this._onDestroy))
    .subscribe(() => {
      this.filterreportedtype();
    });
  }
  
  
  private filterreportedtype() {
    if (!this.ReportedToList) {
      return;
    }
    let search = this.control.value;
    if (!search) {
      this.filteredRecords.next(this.ReportedToList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
     this.filteredRecords.next(
      this.ReportedToList.filter(ServiceType => ServiceType.fullName.toLowerCase().indexOf(search) > -1)
     );
    }
  }
  getReportedTo(){
    this.commonservice.getReportedTo().subscribe((res=>{
      if(res){
        this.response = res;
        this.ReportedToList=this.response.responseData||[];
        this.filteredRecords.next(this.ReportedToList.slice());
      }else{

      }
    }));
  }
  openEmployeeDetail() {
    this.router.navigate(['employeedetail/employeedetails', this.employeeId]);
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      
      case 'subject':
        this.orderColumn = 0;
        break;
      case 'createdDate':
        this.orderColumn = 1;
        break;
        case 'message':
          this.orderColumn = 2;
          break;
       case 'createdDate':
        this.orderColumn = 3;
        break;

      default:
        break;
    }
  }
  getCommunicationList() {
    this.getSortingOrder()
    const data = {
      EmployeeId: Number(this.employeeId),
      SearchTextBySubject: '',
      SearchTextByAssignedTo: '',
      PageSize: this.paging.pageSize,
      PageNo: this.paging.pageNo,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.empService.getCommunicationInfo(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      switch (this.response.status) {
        case 1:
         this.communicationList = this.response.responseData;
          this.dataSourceDiscussion = new MatTableDataSource(this.communicationList);
          break;

        default:
          this.dataSourceDiscussion = new MatTableDataSource(this.communicationdata);
          break;
      }
    });
  }

  AddCommunication() {
    if (this.rForm.valid) {
       this.communicationModel.assignedTo = (this.rForm.get('assignedTo').value);
       const data = {
        'EmployeeId': Number(this.employeeId),
        'Subject': this.rForm.get('subject').value,
        'AssignedTo':  this.communicationModel.assignedTo,
        'Message': this.rForm.get('message').value
      };

      this.empService.addCommunicationInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.rForm.controls['assignedTo'].setValue('');
            this.rForm.controls['message'].setValue('');
            this.notificationService.Success({ message: this.response.message, title: null });
            this.communicationModel = this.response.responseData;
            this.communicationList.push(this.communicationModel);
            this.getCommunicationList();
             this.dataSourceDiscussion = new MatTableDataSource(this.communicationList);
            this.rForm.reset();
            this.formDirective.resetForm();
           
            break;

          default:
            break;
        }
       
      });
    }
  }
  DeleteModal(CommunicationId,_e)
  {
    this.deleteCommunicationId = CommunicationId;
  }
  
DeleteCommunicationInfo(event){
    this.CommunicationInfoModel.Id=this.deleteCommunicationId;
    this.empService.DeleteCommunicationDetails(this.CommunicationInfoModel).subscribe((data: any) => {
      if (data.status == 1) {
        this.notificationService.Success({ message: data.message, title: null });
        this.getCommunicationList();
      }
      else {
        this.notificationService.Error({ message: 'Some error occured', title: '' });
      }

    })
  }
  openEditDetails(elem) {
    document.getElementById("openEditModalButton").click();
    this.EditCommunicationId=elem.id;
    this.SubjectShow=elem.subject;
    this.AssignedToShow=(elem.communicationRecepientmodel.map(x=>x.assignedToName));
    this.MessageShow=(elem.message);
    
  }
 
  toggleAllSelection() {
   if (this.allSelected.selected) {
    this.selecttextName="UnSelect All";
      this.rForm.controls.assignedTo
        .patchValue([...this.ReportedToList.map(item => item.id), 0]);
    } else {
      this.selecttextName="Select All";
      this.rForm.controls.assignedTo.patchValue([]);
    }
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getCommunicationList();
  }
}
export interface AllEmployeeCommunication {
  Id?:number;
  
}