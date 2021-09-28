import { Component, OnInit, ViewChild, ElementRef, Input } from '@angular/core';
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
import { EmployeePayRates } from 'projects/dashboard/src/lib/viewmodel/employee-payrates';
@Component({
  selector: 'app-global-payrate',
  templateUrl: './global-payrate.component.html',
  styleUrls: ['./global-payrate.component.scss']
})
export class GlobalPayrateComponent implements OnInit {
  rForm: FormGroup;
  rForm1: FormGroup;
  totalCount: number;
  paging: Paging = {};
  getErrorMessage: 'Please Enter Value';
  employeeId: any;
  response: ResponseModel = {};
  leaveId: any;
  responseModel: ResponseModel = {};
  displayedColumnsLeave: string[] = ['action','level','MonToFri12To6AM','Sat12To6AM','Sun12To6AM','Holiday12To6AM','MonToFri6To12AM','Sat6To12AM','Sun6To12AM','Holiday6To12AM','ActiveNightsAndSleep','HouseCleaning','TransportPetrol'];
  dataSourcepayrate: any;
  employeePrimaryInfo: {};
  Masterentrylist = [];
  orderBy: number;
  orderColumn: number;
  @Input() empPayRates: EmployeePayRates[];

  @ViewChild('btnPayRatesCancel') cancel: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  EditId: any;
  levelList: any;
  globalPayRate: any = {};
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private loaderService: LoaderService,private commonService:CommonService, private adminService: AdminService, private notificationService: NotificationService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }
  ngOnInit(): void {
    this.createForm();
    this.getlevelList();
    this.GetList();
  }
  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.GetList();
  }
  GetList() {
     const data = {
      pageNo: this.paging.pageNo,
      pageSize: this.paging.pageSize,
    };
    this.adminService.getpayrateInfo(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      if (this.response.responseData) {
        this.empPayRates = this.response.responseData;
        this.dataSourcepayrate = new MatTableDataSource(this.empPayRates);
        
         }
      else {
        this.dataSourcepayrate = new MatTableDataSource(this.Masterentrylist);
      }
    });
  }
  createForm() {
    this.rForm = this.fb.group({
      level: [null, Validators.compose([Validators.required, 
      Validators.minLength(1),
      ])],
      monToFri6To12AM: [null, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      sat6To12AM: [null, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      sun6To12AM: [null, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      holiday6To12AM: [null, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      monToFri12To6AM: [null, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      sat12To6AM: [null, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      sun12To6AM: [null, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      holiday12To6AM: [null, Validators.compose([Validators.required,
      Validators.maxLength(8),
      ])],
      activenights: [null, Validators.compose([Validators.required,
        Validators.maxLength(8),
        ])],
        housecleaning: [null, Validators.compose([Validators.required,
          Validators.maxLength(8),
          ])],
          transportpetrol: [null, Validators.compose([Validators.required,
            Validators.maxLength(8),
            ])],
    });
  }

  get level() {
    return this.rForm.get('level');
  }
  get monToFri6To12AM() {
    return this.rForm.get('monToFri6To12AM');
  }
  get sat6To12AM() {
    return this.rForm.get('sat6To12AM');
  }
  get sun6To12AM() {
    return this.rForm.get('sun6To12AM');
  }

  get holiday6To12AM() {
    return this.rForm.get('holiday6To12AM');
  }

  get monToFri12To6AM() {
    return this.rForm.get('monToFri12To6AM');
  }

  get sat12To6AM() {
    return this.rForm.get('sat12To6AM');
  }
  get sun12To6AM() {
    return this.rForm.get('sun12To6AM');
  }
  get holiday12To6AM() {
    return this.rForm.get('holiday12To6AM');
  }
  get activenights() {
    return this.rForm.get('activenights');
  }
  get housecleaning() {
    return this.rForm.get('housecleaning');
  }
  get transportpetrol() {
    return this.rForm.get('transportpetrol');
  }
  getlevelList(){
    this.commonService.getLevel().subscribe((res=>{
      if(res){
        this.response = res;
        this.levelList=this.response.responseData || [];
       
      }else{

      }
    }));
  }
  UpdateEmpPayrate() {
    if (this.rForm.valid) {
      const data = {
        Id: this.EditId,
        Level: parseInt(this.level.value),
        MonToFri6To12AM: parseFloat(this.monToFri6To12AM.value),
        Sat6To12AM: parseFloat(this.sat6To12AM.value),
        Sun6To12AM: parseFloat(this.sun6To12AM.value),
        Holiday6To12AM: parseFloat(this.holiday6To12AM.value),
        MonToFri12To6AM: parseFloat(this.monToFri12To6AM.value),
        Sat12To6AM: parseFloat(this.sat12To6AM.value),
        Sun12To6AM: parseFloat(this.sun12To6AM.value),
        Holiday12To6AM: parseFloat(this.holiday12To6AM.value),
        ActivenightsandSleep:parseFloat(this.activenights.value),
        HouseCleaning:parseFloat(this.housecleaning.value),
        TransportPetrol:parseFloat(this.transportpetrol.value),
      };
      this.adminService.EditpayrateInfo(data).subscribe(res => {
        this.response = res;
        switch (this.response.status) {
          case 1:
            this.empPayRates = this.response.responseData;
            this.cancel.nativeElement.click();
            this.GetList();
            this.notificationService.Success({ message: 'PayRates updated successfully', title: null });
            break;

          default:
            break;
        }
      });
    }
  }
  openEditModal(elem){
    document.getElementById("openPayModalButton").click();
    this.EditId=elem.id;
    this.rForm.controls['level'].patchValue(elem.level);
    this.rForm.get('monToFri6To12AM').setValue(elem.monToFri6To12AM);
    this.rForm.get('sat6To12AM').setValue(elem.sat6To12AM);
    this.rForm.get('sun6To12AM').setValue(elem.sun6To12AM);
    this.rForm.get('holiday6To12AM').setValue(elem.holiday6To12AM);
    this.rForm.get('monToFri12To6AM').setValue(elem.monToFri12To6AM);
    this.rForm.get('sat12To6AM').setValue(elem.sat12To6AM);
    this.rForm.get('sun12To6AM').setValue(elem.sun12To6AM);
    this.rForm.get('holiday12To6AM').setValue(elem.holiday12To6AM);
    this.rForm.get('activenights').setValue(elem.activeNightsAndSleep);
    this.rForm.get('housecleaning').setValue(elem.houseCleaning);
    this.rForm.get('transportpetrol').setValue(elem.transportPetrol);
  }
  selected() {
  
    if(Number(this.level.value))
    {
     this.getGlobalPayRate(Number(this.level.value));
    }
    
    
 }
   getGlobalPayRate(level: number){
    
     this.commonService.getGlobalPayrate(level).subscribe((res=>{
       if(res){
         this.response = res;
         this.globalPayRate=this.response.responseData || {};
         this.rForm.get('monToFri6To12AM').setValue(this.globalPayRate.monToFri6To12AM);
         this.rForm.get('sat6To12AM').setValue(this.globalPayRate.sat6To12AM);
           this.rForm.get('sun6To12AM').setValue(this.globalPayRate.sun6To12AM);
             this.rForm.get('holiday6To12AM').setValue(this.globalPayRate.holiday6To12AM);
               this.rForm.get('monToFri12To6AM').setValue(this.globalPayRate.monToFri12To6AM);
                 this.rForm.get('sat12To6AM').setValue(this.globalPayRate.sat12To6AM);
                   this.rForm.get('sun12To6AM').setValue(this.globalPayRate.sun12To6AM);
                     this.rForm.get('holiday12To6AM').setValue(this.globalPayRate.holiday12To6AM);
                     this.rForm.get('activenights').setValue(this.globalPayRate.activeNightsAndSleep);
                     this.rForm.get('housecleaning').setValue(this.globalPayRate.houseCleaning);
                     this.rForm.get('transportpetrol').setValue(this.globalPayRate.transportPetrol);
       }else{
 
       }
     }));
   }
}
