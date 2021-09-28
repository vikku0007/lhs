import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'projects/core/src/projects';
import { EmpDashboardService } from 'projects/employee-dashboard/src/lib/services/emp-dashboard.service';
import { CommonService, LoaderService } from 'projects/lhs-service/src/projects';
import { ShiftInfoViewModel } from 'projects/roster/src/lib/viewmodel/roster-shift-info-viewModel';
import { Paging } from 'projects/viewmodels/paging';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { OtherService } from '../../services/other.service';

export interface RequiredComplianceElement {
  date: string;
  time: string;
  client: string;
  staff: string;
  location: string;
  allowance: string;
  status: string;
}

@Component({
  selector: 'lib-incident-reported-new',
  templateUrl: './incident-reported-new.component.html',
  styleUrls: ['./incident-reported-new.component.scss']
})
export class IncidentReportedNewComponent implements OnInit {
  displayedColumnsRequired: string[] = ['description', 'date', 'time', 'duration', 'location', 'status', 'client', 'action'];
  shiftList: ShiftInfoViewModel[];
  responseModel: ResponseModel = {};
  rForm: FormGroup;
  LocationList: [];
  EmployeeNameList: [];
  clientNameList: [];
  shiftStatusList: [];
  dataSource: any;
  totalCount: number;
  paging: Paging = {};
  searchByEmpName = 0;
  searchByClientName = 0;
  searchBylocation = 0;
  deletelId = 0;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  show: boolean = false;
  empId: number;
  orderBy: number;
  orderColumn: number;

  constructor(private fb: FormBuilder, private notificationService: NotificationService, private route: ActivatedRoute,
    private otherService: OtherService, private commonService: CommonService, private loaderService: LoaderService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.empId = Number(params.params.id);
    });
    this.createForm();
    this.bindDropDown();
    this.getEmployeeShiftList();
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getEmployeeShiftList())
        )
        .subscribe();
    }, 2000);

  }

  ClearFilter() {
    this.rForm.reset();
  }

  bindDropDown() {
    //this.getAllClientNameList();
    this.getLocation();
    this.getShiftStatusList();
  }
  createForm() {
    this.rForm = this.fb.group({
      clientId: [null, Validators.nullValidator],
      shiftType: [null, Validators.nullValidator],
      shiftStatus: [null, Validators.nullValidator],
      locationId: [null, Validators.nullValidator],
      otherLocation: [null, Validators.nullValidator]

    });
  }

  get shiftStatus() {
    return this.rForm.get('shiftStatus');
  }

  get otherLocation() {
    return this.rForm.get('otherLocation');
  }
  get locationId() {
    return this.rForm.get('locationId');
  }
  toggleFilters() {
    this.show = !this.show;
  }

  getEmployeeShiftList() {
    this.getSortingOrder();
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      SearchTextBylocation: this.locationId.value ? this.locationId.value : 0,
      SearchTextByStatus: this.shiftStatus.value ? this.shiftStatus.value : 0,
      SearchTextByManualAddress: this.otherLocation.value,
      EmployeeId: this.empId,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.loaderService.start();
    this.otherService.GetEmployeeShiftList(data).subscribe((res) => {
      this.loaderService.stop();
      this.responseModel = res;
      this.totalCount = this.responseModel.total;
      switch (this.responseModel.status) {
        case 1:
          this.shiftList = this.responseModel.responseData;
          this.dataSource = new MatTableDataSource(this.shiftList);
          break;
        case 0:
          this.notificationService.Warning({ message: this.responseModel.message, title: null });
          this.dataSource = new MatTableDataSource([]);
          break;
        default:
          // this.notificationService.Error({ message: 'Some error occured while fetching employee listing', title: null });
          break;
      }
    })
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'description':
        this.orderColumn = 0;
        break;
      case 'location':
        this.orderColumn = 1;
        break;
      case 'status':
        this.orderColumn = 2;
        break;
      case 'createdDate':
        this.orderColumn = 3;
        break;
      default:
        break;
    }
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getEmployeeShiftList();
  }
  getLocation() {
    this.commonService.getLocation().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.LocationList = this.responseModel.responseData || [];

      } else {
        this.notificationService.Error({ message: 'Something went wrong! location not found', title: null });
      }
    }));
  }

  getAllClientNameList() {
    this.commonService.getAllClientNameList().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.clientNameList = this.responseModel.responseData || [];

      } else {
        this.notificationService.Error({ message: 'Something went wrong! location not found', title: null });
      }
    }));
  }
  getShiftStatusList() {
    this.commonService.getShiftStatusList().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.shiftStatusList = this.responseModel.responseData || [];

      } else {
        this.notificationService.Error({ message: 'Something went wrong! location not found', title: null });
      }
    }));
  }

  formattedaddress = " ";
  public handleAddressChange(address: any) {
    debugger;
    this.formattedaddress = address.address1
    this.rForm.controls['otherLocation'].setValue(this.formattedaddress);
  }

}
