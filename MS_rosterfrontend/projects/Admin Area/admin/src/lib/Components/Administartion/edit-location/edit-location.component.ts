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
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'lib-edit-location',
  templateUrl: './edit-location.component.html',
  styleUrls: ['./edit-location.component.css']
})

export class EditLocationComponent implements OnInit, AfterViewInit {

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
  locationlist=[];
  getErrorMessage:'Please Enter Value';
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('btnCancel') cancel: ElementRef;
  @ViewChild('empName') empName: ElementRef;
  LocationId: any;
  Statusval:any;
 // @ViewChild('Location') Location: ElementRef;
  constructor(private fb: FormBuilder, private locationService: LocationService,private activatedRoute: ActivatedRoute) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe(params => {
      this.LocationId = params['Id'];
     
   });
    this.createForm();
    this.getLocationDetailbyId();
  }

  ngAfterViewInit(): void {
    // this.dataSource.paginator = this.paginator;
    // this.dataSource.sort = this.sort;
  }
  getLocationDetailbyId() {
    
    this.locationInfoModel.LocationId=parseInt(this.LocationId);
    this.locationService.getLocationDetailbyId(this.locationInfoModel).subscribe((data:any)=>{
      if(data)
      {
       
        let locationarray = [];
       
        if(data.responseData.length>0)
        {
        this.locationlist=data.responseData;
       
      if(this.locationlist[0]["status"]===true){
        this.Statusval="1";
      }
      else{
        this.Statusval="0";
      }
        
        this.rForm.patchValue({
          Name: this.locationlist[0]["name"],
          Address: this.locationlist[0]["address"],
          CalenderView: this.locationlist[0]["calenderView"],
          Weekday: this.locationlist[0]["weekDay"],
          ExternalCode:this.locationlist[0]["externalCode"],
          InvoicePrefix:this.locationlist[0]["invoicePrefix"],
          ManagerName:this.locationlist[0]["managerId"],
          ManagerContact:this.locationlist[0]["managerContact"],
          Description:this.locationlist[0]["description"],
          Status:this.Statusval,
          City:this.locationlist[0]["city"],
          Country:this.locationlist[0]["country"],
          State:this.locationlist[0]["state"],
          AdditionalSetting:this.locationlist[0]["additionalSetting"],
       
         
        });
        
        }
        
    }
      else{
        
      }
      
    })
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
      AdditionalSetting: [null, Validators.nullValidator]
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
 

  EditLocation() {
    if (this.rForm.valid) {
     this.locationInfoModel = this.rForm.value;
      this.locationInfoModel.ExternalCode=parseInt(this.rForm.value.ExternalCode);
      this.locationInfoModel.ManagerId=parseInt(this.rForm.value.ManagerName);
      this.locationInfoModel.LocationId=parseInt(this.LocationId);
      if(this.rForm.value.Status === "1"){
        this.locationInfoModel.Status=true;
      }
      else{
        this.locationInfoModel.Status=false;
      }
      
      this.locationService.EditLocationDetails(this.locationInfoModel).subscribe(res => {
        this.responseModel = res;
        switch (this.responseModel.status) {
          case 1:
            this.rForm.reset();
            this.cancel.nativeElement.click();
            alert('Location Updated successfully');
            this.getLocationDetailbyId();
            break;

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

}