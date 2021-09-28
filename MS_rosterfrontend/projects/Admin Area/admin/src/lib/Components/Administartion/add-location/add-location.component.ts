import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { Paging } from 'projects/viewmodels/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LocationDetails } from '../../../viewmodel/Location-details';
import { LocationService } from '../../../services/location-service.service';
import { isBuffer } from 'util';
import { Router } from '@angular/router';
import { CommonService } from 'projects/lhs-service/src/projects';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { LatLongLocationDetails } from '../../../viewmodel/latlonglocation';
import { NotificationService } from 'projects/core/src/projects';
@Component({
  selector: 'app-add-location',
  templateUrl: './add-location.component.html',
  styleUrls: ['./add-location.component.scss']
})
export class AddLocationComponent implements OnInit {

  displayedColumns: string[] = ['name', 'address',  'status', 'action'];
  // dataSource = new MatTableDataSource(ELEMENT_DATA);
  dataSource: any;
  totalCount: number;
  paging: Paging = {};
  responseModel: ResponseModel = {};
  locationDetailModel: LatLongLocationDetails[];
  locationInfoModel: LatLongLocationDetails = {};
  rForm: FormGroup;
  rFormEdit: FormGroup;
  searchByName = null;
  searchByLocation = null;
  deletelocationId : number;
  getErrorMessage:'Please Enter Value';
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('btnCancel') cancel: ElementRef;
  @ViewChild('btnEditCancel') editCancel:ElementRef;
  @ViewChild('empName') empName: ElementRef;
  @ViewChild('codeDescription') searchcodedescription: ElementRef;
  Typelist: any;
  orderBy: number;
  orderColumn: number;
  masterId: number;
 // @ViewChild('Location') Location: ElementRef;
  constructor(private fb: FormBuilder,private commonService:CommonService, private locationService: LocationService,private router:Router, private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }
  
  ngOnInit() {
    this.createForm();
    this.createEditForm(null);
    this.getLocationList();
    this.getCodeType();
  }

  // ngAfterViewInit(): void {
  //   // this.dataSource.paginator = this.paginator;
  //   // this.dataSource.sort = this.sort;
  // }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.dataSource !== undefined ? this.dataSource.sort = this.sort : this.dataSource;
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          tap(() => this.getLocationList())
        )
        .subscribe();
    }, 2000);

  }
  getCodeType(){
    this.commonService.getCodeType().subscribe((res=>{
      if(res){
        this.responseModel = res;
        this.Typelist=this.responseModel.responseData||[];
       
      }else{

      }
    }));
  }
  EditModal(element,_e)
  {
    debugger;
    //this.router.navigate(['/admin/edit-location'], { queryParams: { Id: LocationID } });
    this.masterId=element.locationId;
    this.createEditForm(element);
  }
  createEditForm(element) {
    if (element != null) {
     this.rFormEdit = this.fb.group({
      Name: [element.name, Validators.required],
      Address: [element.address, Validators.required],
      Latitude: [element.latitude, Validators.required],
      Longitude: [element.longitude, Validators.required],
      JobCode: [element.jobCode, Validators.required]
     });
   }
   else {
     this.rFormEdit = this.fb.group({
      Name: ['', Validators.required],
      Address: ['', Validators.required],
      Latitude: ['', Validators.required],
      Longitude: ['', Validators.required],
      JobCode: ['', Validators.required]
     });
   }
 }
  createForm() {
    this.rForm = this.fb.group({
      Name: ['', Validators.required],
      Address: [null, Validators.required],
      Latitude: [null, Validators.required],
      Longitude: [null, Validators.required],
      JobCode: ['', Validators.required]
    });
  }

  getLocationList() {
    this.getSortingOrder();
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      searchTextByName: this.searchByName,
      searchTextByLocation: this.searchByLocation,
      OrderBy: this.orderColumn,
      SortOrder: this.orderBy
    };
    this.locationService.getLocationList(data).subscribe((res) => {
      this.responseModel = res;
      this.totalCount = this.responseModel.total;
      switch (this.responseModel.statusCode) {
        case 200:
          this.locationDetailModel = this.responseModel.responseData;
         // this.locationDetailModel.forEach(x => x.age = 0);
          this.dataSource = new MatTableDataSource(this.locationDetailModel);
          break;
        case 400:
          alert('some Error occured');
          break;
        default:
          alert('Some error occured while fetching employee listing');
          break;
      }
    })
  }
  getSortingOrder() {
    const sortColumn = this.sort.active == undefined ? 'name' : this.sort.active;
    this.orderBy = this.sort.direction === 'asc' ? 0 : 1;
    switch (sortColumn) {
     
      case 'name':
        this.orderColumn = 0;
        break;
      case 'address':
        this.orderColumn = 1;
        break;
      case 'status':
          this.orderColumn = 2;
          break;
      case 'action':
            this.orderColumn = 3;
            break;
      //  case 'createdDate':
      //       this.orderColumn = 4;
      //       break;
      default:
        break;
    }
  }
  AddLocation() {
    if (this.rForm.valid) {
      debugger;
      this.locationInfoModel = this.rForm.value;
      this.locationService.addLatLongLocationDetails(this.locationInfoModel).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.rForm.reset();
            this.cancel.nativeElement.click();
            this.getLocationList();
            this.notificationService.Success( {message: 'Location Added successfully', title: null});
            break;

          default:
            this.notificationService.Warning({ message: 'Error occured', title: null });
            break;
        }
      });
      this.locationDetailModel.push(this.locationInfoModel);
      this.dataSource = new MatTableDataSource(this.locationDetailModel);
    } else {
      this.validateAllFields(this.rForm)
    }
  }
  EditLocation() {
    if (this.rFormEdit.valid) {
      debugger;
      this.locationInfoModel = this.rFormEdit.value;
      this.locationInfoModel.LocationId=this.masterId;
      this.locationService.editLatLongLocationDetails(this.locationInfoModel).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.rFormEdit.reset();
            this.editCancel.nativeElement.click();
            this.getLocationList();
            this.notificationService.Success( {message: 'Location Updated successfully', title: null});
            break;

          default:
            this.notificationService.Warning({ message: 'Error occured', title: null });
            break;
        }
      });
      this.locationDetailModel.push(this.locationInfoModel);
      this.dataSource = new MatTableDataSource(this.locationDetailModel);
    } else {
      this.validateAllEditFields(this.rFormEdit)
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
  validateAllEditFields(formGroup: FormGroup) {
    Object.keys(this.rFormEdit.controls).map(controlName => {
      this.rFormEdit.get(controlName).markAsTouched({ onlySelf: true })
    });
    Object.keys(this.rFormEdit.controls).map(controlName => {
      this.rFormEdit.get(controlName).markAsDirty({ onlySelf: true })
    });
  }
  Search() {
    this.searchByName = this.empName.nativeElement.value;
   // this.searchByLocation = this.Location.nativeElement.value;
    this.paging.pageNo = 1;
    this.getLocationList();
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getLocationList();
  }
  DeleteModal(locationID,_e)
  {

    this.deletelocationId = locationID;
  }
  Deletelocation(event){
   
    this.locationInfoModel.LocationId=this.deletelocationId;
    this.locationService.DeleteLocationDetails(this.locationInfoModel).subscribe((data: any) => {
      if (data.statusCode == 200) {
        alert('Location Deleted successfully');
        this.getLocationList();
      }
      else {
        alert("Some error occured");
      }

    })
  }
}
