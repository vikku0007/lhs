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

@Component({
  selector: 'lib-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.css']
})
export class LocationComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['name', 'address', 'weekday',  'status', 'action'];
  // dataSource = new MatTableDataSource(ELEMENT_DATA);
  dataSource: any;
  totalCount: number;
  paging: Paging = {};
  responseModel: ResponseModel = {};
  locationDetailModel: LocationDetails[];
  locationInfoModel: LocationDetails = {};
  rForm: FormGroup;
  searchByName = null;
  searchByLocation = null;
  deletelocationId : number;
  getErrorMessage:'Please Enter Value';
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('btnCancel') cancel: ElementRef;
  @ViewChild('empName') empName: ElementRef;
  Typelist: any;
 // @ViewChild('Location') Location: ElementRef;
  constructor(private fb: FormBuilder,private commonService:CommonService, private locationService: LocationService,private router:Router) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }
  
  ngOnInit() {
    this.createForm();
    this.getLocationList();
    this.getCodeType();
  }

  ngAfterViewInit(): void {
    // this.dataSource.paginator = this.paginator;
    // this.dataSource.sort = this.sort;
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
  EditModal(LocationID,_e)
  {
    this.router.navigate(['/admin/edit-location'], { queryParams: { Id: LocationID } });
    
  }
  createForm() {
    this.rForm = this.fb.group({
      Name: ['', Validators.required],
      Address: [null, Validators.required],
      CalenderView: [null, Validators.required],
      Weekday: [null, Validators.required],
      ExternalCode: [],
      InvoicePrefix: [],
      ManagerName: [null, Validators.required],
      ManagerContact: [null, Validators.required],
      Description: [],
      Status: [null, Validators.required],
      City: [null, Validators.required],
      Country: [null, Validators.required],
      State: [null, Validators.required],
      AdditionalSetting: [null, Validators.nullValidator],
      AddType: [null, Validators.nullValidator]
    });
  }

  get Name() {
    return this.rForm.get('Name');
  }

  get Address() {
    return this.rForm.get('address');
  }

  get CalenderView() {
    return this.rForm.get('CalenderView');
  }

  get Weekday() {
    return this.rForm.get('Weekday');
  }

  get InvoicePrefix() {
    return this.rForm.get('InvoicePrefix');
  }

  get ManagerName() {
    return this.rForm.get('ManagerName');
  }

  get ManagerContact() {
    return this.rForm.get('ManagerContact');
  }

  get Description() {
    return this.rForm.get('Description');
  }
   get Status() {
    return this.rForm.get('Status');
   }
  get City() {
    return this.rForm.get('City');
  }
  get State() {
    return this.rForm.get('State');
  }
  get Country() {
    return this.rForm.get('Country');
  }
  get AdditionalSetting() {
    return this.rForm.get('AdditionalSetting');
  }
  getLocationList() {
    const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
      searchTextByName: this.searchByName,
      searchTextByLocation: this.searchByLocation
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

  AddLocation() {
    if (this.rForm.valid) {
      this.locationInfoModel = this.rForm.value;
      this.locationInfoModel.ExternalCode=parseInt(this.rForm.value.ExternalCode);
      this.locationInfoModel.ManagerId=parseInt(this.rForm.value.ManagerName);
      if(this.rForm.value.Status === "1"){
        this.locationInfoModel.Status=true;
      }
      else{
        this.locationInfoModel.Status=false;
      }
      
      this.locationService.addLocationDetails(this.locationInfoModel).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.rForm.reset();
            this.cancel.nativeElement.click();
            alert('Location added successfully');
            break;

          default:
            break;
        }
      });
      this.locationDetailModel.push(this.locationInfoModel);
      this.dataSource = new MatTableDataSource(this.locationDetailModel);
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
      if (data.statusCode == 400) {
        alert('Location Deleted successfully');
        this.getLocationList();
      }
      else {
        alert("Some error occured");
      }

    })
  }
}

