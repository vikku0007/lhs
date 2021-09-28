import { Component, OnInit, ViewChild, ElementRef, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm, FormControl } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NotificationService } from 'projects/core/src/projects';
import { Paging } from 'projects/viewmodels/paging';
import { ClientService } from '../../services/client.service';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { AddClientDetails } from '../../view-models/add-client-details';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { CommonService } from 'projects/lhs-service/src/lib/service/common/common.service';
import { Constants } from '../../config/constants';
import { environment } from 'src/environments/environment';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { EmpServiceService } from 'projects/dashboard/src/lib/services/emp-service.service';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { AppDateAdapter } from 'projects/lhs-directives/src/projects';
import { APP_DATE_FORMATS } from 'projects/lhs-directives/src/lib/directives/date-format.directive';

// For Language
interface Language {
  value: string;
  name: string;
}
interface Gender {
  value: string;
  name: string;
}
export interface PeriodicElement {
  clientName: string;
  gender: string;
  age: number;
  email: string;
  telephone: string;
  location: string;
  address: string;
}

const ELEMENT_DATA: PeriodicElement[] = [
  { clientName: 'Mario Speedwagon', gender: 'F', age: 25, email: 'marios1988@gmail.com', telephone: '202-555-0166', location: '3 Rock Daisy Drive', address: '8 Rock Daisy Drive Cranbourne West VIC, Australia' },
  { clientName: 'Petey Cruiser', gender: 'F', age: 24, email: 'peteyc@199@gmail.com', telephone: '202-555-0146', location: '5 Rock Daisy Drive', address: '5 Rock Daisy Drive Cranbourne East VIC, Australia' },
  { clientName: 'Anna Sthesia', gender: 'F', age: 26, email: 'annast007@gmail.com', telephone: '202-555-0198', location: '8 Rock Daisy Drive', address: '4 Rock Daisy Drive Cranbourne North VIC, Australia' },
  { clientName: 'Paul Molive', gender: 'M', age: 28, email: 'paulmolive21@gmail.com', telephone: '202-555-0150', location: '11 Rock Daisy Drive', address: '2 Rock Daisy Drive Cranbourne West VIC, Australia' },
  { clientName: 'Gail Forcewind', gender: 'M', age: 30, email: 'gailforcewind@gmail.com', telephone: '202-555-0123', location: '2 Rock Daisy Drive', address: '10 Rock Daisy Drive Cranbourne East VIC, Australia' },
];


@Component({
  selector: 'lib-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.scss'],
  providers: [
    {
      provide: DateAdapter, useClass: AppDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS
    }
  ]
})
export class ClientListComponent implements OnInit {
  getErrorMessage: 'Please Enter Value';

  //For Language
  languages: Language[] = [
    { value: '', name: 'Select Language' },
    { value: 'lang-1', name: 'English' },
    { value: 'lang-2', name: 'Spanish' },
    { value: 'lang-3', name: 'French' }
  ];
  selectedLang = 'lang-1';
  genders: Gender[] = [
    { value: '', name: 'Select Gender' },
    { value: 'male', name: 'Male' },
    { value: 'female', name: 'Female' }
  ];
  clientForm: FormGroup;
  paging: Paging = {};
  searchByName: string;
  searchByLocation: string;
  responseModel: ResponseModel = {};
  totalCount: number;
  clientModel: AddClientDetails = {};
  clientArray: AddClientDetails[] = [];
  displayedColumns: string[] = ['fullName', 'emailId', 'mobileNo', 'address', 'status', 'action'];
  dataSource = new MatTableDataSource(this.clientArray);
  todayDatemax = new Date();
  todayDate: any;
  LocationList: any;
  SalutationList: any;
  GenderList: any;
  @ViewChild('btnCancel') cancel: ElementRef;
  @ViewChild('clientName') clientName: ElementRef;
  @ViewChild('clientLocation') clientLocation: ElementRef;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('formDirective') private formDirective: NgForm;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  deleteclientId: any;
  clientPic: any;
  baseUrl: string = environment.baseUrl;
  deleteClientId: any;
  orderBy: number;
  orderColumn: number;
  LocationTypeList: any;
  list: any;
  selectedType: any;

  constructor(private clientService: ClientService, private empService: EmpServiceService, private notificationService: NotificationService,
    private fb: FormBuilder, private commonservice: CommonService) { }
  isShownOtherLocation: boolean = false;
  isShownLocatiodropdown: boolean = false;

  ngOnInit(): void {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
    this.createForm();
    this.getAllClients();
    this.GetGender();
    this.GetSalutation();
    this.getLocationType();
    // this.GetLocation();
    this.dataSource.sort = this.sort;
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getAllClients())
        )
        .subscribe();
    }, 2000);

  }
  createForm() {
    this.clientForm = this.fb.group({
      salutation: [null, Validators.required],
      firstName: [null, Validators.required],
      middleName: [null, Validators.nullValidator],
      lastName: [null, Validators.compose([Validators.required, Validators.pattern(/^[A-Za-z]+$/), Validators.maxLength(25)])],
      emailId: [null, Validators.compose([Validators.nullValidator, Validators.pattern(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)])],
      //mobileNo: [null, Validators.compose([Validators.required,Validators.pattern(/^(\+?\(61\)|\(\+?61\)|\+?61|\(0[1-9]\)|0[1-9])?( ?-?[0-9]){7,9}$/)])],
      mobileNo: ['', [Validators.maxLength(16), Validators.nullValidator]],
      address: [null, Validators.required],
      dateOfBirth: [null, Validators.required],
      gender: ['', Validators.required],


    });
  }
  getLocationType() {
    this.commonservice.getLocationType().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.LocationTypeList = this.responseModel.responseData || [];

      } else {

      }
    }));
  }
  get address() {
    return this.clientForm.get('address');
  }
  selectChangeHandler(event: any) {
    this.list = this.LocationTypeList;
    this.selectedType = event;
    if (this.selectedType == 5) {
      this.GetLocation()
      this.isShownLocatiodropdown = true;
      this.isShownOtherLocation = false;
      this.clientForm.controls['otherlocation'].patchValue("");
    }
    else {
      this.isShownLocatiodropdown = false;
      this.isShownOtherLocation = true;
    }
  }

  getAllClients() {
    this.getSortingOrder();
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      searchTextByName: this.searchByName,
      searchTextByLocation: this.searchByLocation,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.clientService.getClientList(data).subscribe((res) => {
      this.responseModel = res;
      this.totalCount = this.responseModel.total;
      switch (this.responseModel.status) {
        case 1:
          this.clientArray = this.responseModel.responseData;
          this.dataSource = new MatTableDataSource(this.clientArray);
          break;
        case 0:
          this.notificationService.Warning({ message: this.responseModel.message, title: null });
          break;
        default:
          this.notificationService.Error({ message: 'Some error occured while fetching client listing', title: null });
          break;
      }
    })
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'createdDate' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
      case 'fullName':
        this.orderColumn = 0;
        break;
      case 'emailId':
        this.orderColumn = 2;
        break;
      case 'mobileNo':
        this.orderColumn = 1;
        break;
      case 'address':
        this.orderColumn = 3;
        break;
      case 'createdDate':
        this.orderColumn = 4;
        break;

      default:
        break;
    }
  }
  AddClientDetails() {
    if (this.clientForm.valid) {
      this.clientModel = this.clientForm.value;
      this.clientModel.mobileNo = this.clientModel.mobileNo.toString();
      this.clientModel.status = true;
      this.clientService.addClientDetails(this.clientModel).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.notificationService.Success({ message: 'Client added successfully', title: '' })
            this.clientArray.unshift(this.responseModel.responseData);
            this.clientArray[0].fullName = this.clientArray[0].firstName + ' ' +
              (this.clientArray[0]?.middleName ? this.clientArray[0].middleName : ' ') + ' ' +
              this.clientArray[0].lastName;
            this.dataSource = new MatTableDataSource(this.clientArray);
            this.getAllClients();
            this.clientForm.reset();
            this.formDirective.resetForm();
            this.cancel.nativeElement.click();
            // this.isShownLocatiodropdown=false;
            // this.isShownOtherLocation=false;
            break;
          case 0:
            this.notificationService.Error({ message: this.responseModel.message, title: '' });
            this.cancel.nativeElement.click();
          default:
            // this.notificationService.Error({ message: 'some error occured', title: '' });
            break;
        }
      });
    } else {
      // this.notificationService.Warning({ message: 'Validation missing', title: '' });
    }
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getAllClients();
  }

  // applyFilter(event: Event) {
  //   const filterValue = (event.target as HTMLInputElement).value;
  //   this.dataSource.filter = filterValue.trim().toLowerCase();
  // }

  search() {
    this.searchByName = this.clientName.nativeElement.value;
    this.searchByLocation = "";
    this.getAllClients();
  }
  getAge() {
    this.todayDate = this.clientForm.value.dateOfBirth;
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
      this.notificationService.Warning({ message: 'DOB Should be greater than 18 years', title: '' });
      this.clientForm.controls.dateOfBirth.setValue("");
      return;
    } else {


    }


  }
  // delete(elm) {
  //   const data = {
  //     id: elm.id
  //   }
  //   this.clientService.deleteClientDetails(data).subscribe(res => {
  //     this.responseModel = res;
  //     switch (this.responseModel.status) {
  //       case 1:
  //         this.dataSource.data = this.dataSource.data.filter(i => i !== elm)
  //         this.notificationService.Success({ message: 'Client details deleted successfully', title: '' })
  //         break;
  //       case 0:
  //         this.notificationService.Error({ message: this.responseModel.message, title: '' });
  //       default:
  //         break;
  //     }
  //   });
  // }

  DeleteModal(ClientID, _e) {

    this.deleteClientId = ClientID;
  }


  deleteClient(event) {
    const data = {
      id: this.deleteClientId
    }
    this.clientService.deleteClientDetails(data).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          //this.dataSource.data = this.dataSource.data.filter(i => i !== elm)
          this.notificationService.Success({ message: 'Client details deleted successfully', title: '' })
          this.getAllClients();
          break;
        case 0:
          this.notificationService.Error({ message: this.responseModel.message, title: '' });
        default:
          break;
      }
    });
  }

  cancelForm() {
    this.clientForm.reset();
    this.formDirective.resetForm();
  }
  GetLocation() {
    this.commonservice.getLocation().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.LocationList = this.responseModel.responseData || [];

      } else {

      }
    }));
  }
  GetSalutation() {
    this.commonservice.getSalutation().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.SalutationList = this.responseModel.responseData || [];

      } else {

      }
    }));
  }
  GetGender() {
    this.commonservice.getGenderList().subscribe((res => {
      if (res) {
        this.responseModel = res;
        this.GenderList = this.responseModel.responseData || [];

      } else {

      }
    }));
  }
  UpdateClientStatus(elem) {
    debugger;
    const data = {
      id: elem.id,
      Status: elem.isActive == true ? false : true
    }
    this.clientService.UpdateClientActiveStatus(data).subscribe(res => {
      this.responseModel = res;
      switch (this.responseModel.status) {
        case 1:
          //this.dataSource.data = this.dataSource.data.filter(i => i !== elm)
          this.notificationService.Success({ message: 'Client Status Updated successfully', title: '' })
          this.getAllClients();
          break;
        case 0:
          this.notificationService.Warning({ message: this.responseModel.message, title: '' });
        default:
          break;
      }
    });
  }
  formattedaddress = " ";
  public handleAddressChange(address: any) {
    debugger;
    this.formattedaddress = address.address1
    this.clientForm.controls['address'].setValue(this.formattedaddress);
  }

}
