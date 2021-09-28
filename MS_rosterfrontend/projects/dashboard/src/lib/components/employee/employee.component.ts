import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { EmpServiceService } from '../../services/emp-service.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { EmployeeDetails } from '../../viewmodel/employee-details';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { FormGroup, FormBuilder, Validators, NgForm, FormControl } from '@angular/forms';
import { NotificationService } from 'projects/core/src/lib/services/notification-service/notification.service';
import { ErrorStateMatcher, DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { $ } from 'protractor';
import { Gender, Salutation, EmpLevel } from 'projects/lhs-service/src/lib/viewmodels/gender';
import { CommonService } from 'projects/lhs-service/src/projects';
import { environment } from 'src/environments/environment';
import { merge, Subject, ReplaySubject } from 'rxjs';
import { tap, takeUntil } from 'rxjs/operators';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';
import { MatSelect } from '@angular/material/select';


export interface Response {
  result: any;
}

@Component({
  selector: 'lib-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss'],
  providers: [
    {
      provide: DateAdapter, useClass: AppDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
  ]
})
export class EmployeeComponent implements OnInit, AfterViewInit {
  todayDate = new Date();
  todayDatemax = new Date();
  displayedColumns: string[] = ['firstname', 'emailId', 'mobileNo', 'dateOfJoining', 'status', 'action'];
  dataSource: any;
  totalCount: number;
  paging: Paging = {};
  responseModel: ResponseModel = {};
  empDetailModel: EmployeeDetails[];
  empInfoModel: EmployeeDetails = {};
  rForm: FormGroup;
  searchByName = null;
  searchByLocation = null;
  getErrorMessage: 'Please Enter Value';
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('btnCancel') cancel: ElementRef;
  @ViewChild('empName') employeeName: ElementRef;
  @ViewChild('empLocation') employeeLocation: ElementRef;
  @ViewChild('formDirective') formDirective: NgForm;
  public control: FormControl = new FormControl();
  public searchcontrol: FormControl = new FormControl();
  private _onDestroy = new Subject<void>();
  public filteredRecords: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  @ViewChild('Select') select: MatSelect;
  matcher: ErrorStateMatcher;
  genderList: Gender[] = [];
  salutationList: Salutation[] = [];
  levelList: EmpLevel[] = [];
  baseUrl: string = environment.baseUrl;
  deletelId: any;
  orderBy: number;
  orderColumn: number;
  RoleList: any;
  isLevelShown: boolean = false;

  constructor(private fb: FormBuilder, private empService: EmpServiceService, private notificationService: NotificationService,
    private commonService: CommonService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.getGender();
    this.getSalutation();
    this.getLevel();
    this.createForm();
    this.getEmployeeList();
    this.GetRole();
    this.searchroletype();
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getEmployeeList())
        )
        .subscribe();
    }, 2000);

  }
  GetRole() {
    this.commonService.getRoles().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.RoleList = this.responseModel.responseData || [];
        this.filteredRecords.next(this.RoleList.slice());
      } else {

      }
    }));
  }
  searchroletype() {
    this.control.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.filterroletype();
      });
  }
  private filterroletype() {
    if (!this.RoleList) {
      return;
    }

    let search = this.control.value;
    if (!search) {
      this.filteredRecords.next(this.RoleList.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    if (search.length >= 1) {
      this.filteredRecords.next(
        this.RoleList.filter(role => role.codeDescription.toLowerCase().indexOf(search) > -1)
      );
    }
  }
  get formControls() { return this.rForm.controls; }

  createForm() {
    this.rForm = this.fb.group({
      salutation: ['', Validators.required],
      firstname: [null, Validators.required],
      middleName: [],
      lastName: [null, Validators.compose([Validators.required, Validators.pattern(/^[A-Za-z]+$/), Validators.maxLength(25)])],
      gender: [null, Validators.required],
      emailId: [null, Validators.compose([Validators.required, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      mobileNo: ['', [Validators.required]],
      employeeLevel: [null, Validators.nullValidator],
      address1: [null, Validators.nullValidator],
      // status: [null, Validators.required],
      dateOfBirth: [null, Validators.required],
      role: [null, Validators.required],
    });
  }

  get salutation() {
    return this.rForm.get('salutation');
  }

  get firstname() {
    return this.rForm.get('firstname');
  }

  get middleName() {
    return this.rForm.get('middleName');
  }

  get lastName() {
    return this.rForm.get('lastName');
  }

  get gender() {
    return this.rForm.get('gender');
  }

  get email() {
    return this.rForm.get('emailId');
  }

  get mobileNo() {
    return this.rForm.get('mobileNo');
  }



  get level() {
    return this.rForm.get('employeeLevel');
  }

  // get status() {
  //   return this.rForm.get('status');
  // }


  get dateOfBirth() {
    return this.rForm.get('dateOfBirth');
  }
  get role() {
    return this.rForm.get('role');
  }
  getEmployeeList() {
    this.getSortingOrder();
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      searchTextByName: this.searchByName,
      searchTextByLocation: this.searchByLocation,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.empService.getEmployeeList(data).subscribe((res) => {
      this.responseModel = res;
      this.totalCount = this.responseModel.total;
      switch (this.responseModel.status) {
        case 1:
          this.empDetailModel = this.responseModel.responseData;
          this.dataSource = new MatTableDataSource(this.empDetailModel);
          break;
        case 0:
          this.notificationService.Warning({ message: this.responseModel.message, title: null });
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
      case 'firstname':
        this.orderColumn = 0;
        break;
      case 'emailId':
        this.orderColumn = 2;
        break;
      case 'dateOfJoining':
        this.orderColumn = 3;
        break;
      case 'createdDate':
        this.orderColumn = 4;
        break;

      default:
        break;
    }
  }

  AddEmployee() {
    if (this.rForm.valid) {
      // this.empInfoModel = this.rForm.value;
      this.empInfoModel.employeeLevel = this.level.value;
      this.empInfoModel.saluation = this.salutation.value;
      this.empInfoModel.mobileNo = String(this.mobileNo.value);
      this.empInfoModel.emailId = this.email.value;
      this.empInfoModel.firstName = this.firstname.value;
      this.empInfoModel.middleName = this.middleName.value;
      this.empInfoModel.lastName = this.lastName.value;
      this.empInfoModel.gender = this.gender.value;
      this.empInfoModel.dateOfBirth = this.dateOfBirth.value;
      this.empInfoModel.status = true;
      this.empInfoModel.role = this.role.value;
      this.empService.addEmployeeDetails(this.empInfoModel).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.rForm.reset();
            this.formDirective.resetForm();
            this.cancel.nativeElement.click();
            this.notificationService.Success({ message: 'Employee added successfully', title: null });
            this.getEmployeeList();
            break;
          case 0:
            this.notificationService.Success({ message: this.responseModel.message, title: null });
          default:
            break;
        }
      });

    } else {
      this.validateAllFields(this.rForm)
    }
  }

  validateAllFields(formGroup: FormGroup) {
    Object.keys(this.rForm.controls).map(controlName => {
      this.rForm.get(controlName).markAsTouched({ onlySelf: true })
    });
    Object.keys(this.rForm.controls).map(controlName => {
      this.rForm.get(controlName).markAsDirty({ onlySelf: true })
    });
  }

  DeleteModal(EmpID, _e) {

    this.deletelId = EmpID;
  }

  deleteEmployee(event) {
    const data = {
      id: this.deletelId
    }
    this.empService.DeleteEmployeedetails(data).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          //this.dataSource.data = this.dataSource.data.filter(i => i !== elm)
          this.notificationService.Success({ message: 'Employee details deleted successfully', title: '' })
          this.getEmployeeList();
          break;
        case 0:
          this.notificationService.Warning({ message: this.responseModel.message, title: '' });
        default:
          break;
      }
    });
  }
  Search() {
    this.searchByName = this.employeeName.nativeElement.value;
    this.searchByLocation = "";
    this.paging.pageNo = 0;
    this.getEmployeeList();
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getEmployeeList();
  }

  cancelForm() {
    this.rForm.reset();
    this.formDirective.resetForm();
  }

  getGender() {
    this.commonService.getGenderList().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.genderList = this.responseModel.responseData;
      }
    }));
  }

  getSalutation() {
    this.commonService.getSalutation().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.salutationList = this.responseModel.responseData;
      }
    }));
  }

  getLevel() {
    this.commonService.getLevel().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.levelList = this.responseModel.responseData;
      }
    }));
  }
  getAgedob() {

    this.todayDate = this.rForm.value.dateOfBirth;
    // var newDate = moment(this.todayDate).add(1, 'year').format('YYYY-MM-DD').toString();
    // var dateString = document.getElementById("date");
    var today = new Date();
    var birthDate = new Date(this.todayDate);
    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    var da = today.getDate() - birthDate.getDate();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    if (m < 0) {
      m += 12;
    }
    if (da < 0) {
      da += 30;
    }

    if (age < 18 || age > 100) {
      this.notificationService.Warning({ message: 'DOB should be greater than 18 years', title: '' });
      this.rForm.controls.dateOfBirth.setValue("");
      return;
    } else {


    }
  }
  onRowClicked(element) {
    this.getEmployeeList();
  }
  UpdateStatus(elem) {
    const data = {
      id: elem.id,
      Status: elem.status == true ? false : true
    }
    this.empService.UpdateActiveInActiveStatus(data).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          //this.dataSource.data = this.dataSource.data.filter(i => i !== elm)
          this.notificationService.Success({ message: 'Employee Status Updated successfully', title: '' })
          this.getEmployeeList();
          break;
        case 0:
          this.notificationService.Warning({ message: this.responseModel.message, title: '' });
        default:
          break;
      }
    });
  }

  selectChangeHandler(data) {
    if (data == 16) {
      this.isLevelShown = true;
      this.rForm.controls["employeeLevel"].setValidators([Validators.required]);
      this.rForm.controls["employeeLevel"].updateValueAndValidity();
    } else {
      this.rForm.controls["employeeLevel"].setValidators(null);
      this.rForm.controls["employeeLevel"].updateValueAndValidity();
    }
  }
}